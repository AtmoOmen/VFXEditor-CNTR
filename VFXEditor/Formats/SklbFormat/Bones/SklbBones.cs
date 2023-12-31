﻿using Dalamud.Interface;
using Dalamud.Logging;
using FFXIVClientStructs.Havok;
using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using VfxEditor.DirectX;
using VfxEditor.FileManager;
using VfxEditor.Interop.Havok;
using VfxEditor.Interop.Havok.SkeletonBuilder;
using VfxEditor.Interop.Structs.Animation;
using VfxEditor.SklbFormat.Mapping;
using VfxEditor.Utils;
using VfxEditor.Utils.Gltf;

namespace VfxEditor.SklbFormat.Bones {
    public enum BoneDisplay {
        Connected,
        Blender_Style_Inline,
        Blender_Style_Perpendicular
    }

    public unsafe class SklbBones : HavokBones {
        private static readonly BoneDisplay[] BoneDisplayOptions = ( BoneDisplay[] )Enum.GetValues( typeof( BoneDisplay ) );

        private readonly SklbFile File;
        private static BoneNamePreview SklbPreview => Plugin.DirectXManager.SklbPreview;

        private bool DrawOnce = false;
        private SklbBone Selected;
        private SklbBone DraggingBone;
        private string SearchText = "";

        public readonly List<SklbMapping> Mappings = new();
        public readonly SklbMappingDropdown MappingView;

        public SklbBones( SklbFile file, string loadPath ) : base( loadPath ) {
            File = file;
            MappingView = new( Mappings );
        }

        protected override void OnLoad() {
            base.OnLoad();

            var variants = Container->NamedVariants;
            for( var i = 0; i < variants.Length; i++ ) {
                var variant = variants[i];
                if( variant.ClassName.String == "hkaSkeletonMapper" ) {
                    var mapper = ( SkeletonMapper* )variant.Variant.ptr;
                    Mappings.Add( new( this, mapper ) );
                }
            }
        }

        // ========== DRAWING ============

        public void Draw() {
            if( SklbPreview.CurrentFile != File ) UpdatePreview();

            var expandAll = false;
            var searchSet = GetSearchSet();

            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                if( ImGui.Button( "导出" ) ) ImGui.OpenPopup( "ExportPopup" );

                using( var popup = ImRaii.Popup( "ExportPopup" ) ) {
                    if( popup ) {
                        if( ImGui.Selectable( "GLTF" ) ) ExportGltf();
                        if( ImGui.Selectable( "HKX" ) ) ExportHavok();
                    }
                }

                ImGui.SameLine();
                if( ImGui.Button( "替换" ) ) ImportDialog();

                ImGui.SameLine();
                UiUtils.WikiButton( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki/Using-Blender-to-Edit-Skeletons-and-Animations" );
            }

            ImGui.SameLine();
            if( ImGui.Checkbox( "显示骨骼名称", ref Plugin.Configuration.ShowBoneNames ) ) Plugin.Configuration.Save();

            ImGui.SameLine();
            ImGui.SetNextItemWidth( 200f );
            if( UiUtils.EnumComboBox( "##BoneDisplay", BoneDisplayOptions, Plugin.Configuration.SklbBoneDisplay, out var newBoneDisplay ) ) {
                Plugin.Configuration.SklbBoneDisplay = newBoneDisplay;
                Plugin.Configuration.Save();
                UpdatePreview();
            }

            ImGui.Separator();

            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 0, 4 ) ) ) {
                ImGui.Columns( 2, "列", true );

                using( var spacing = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                    // New bone
                    using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                        if( ImGui.Button( FontAwesomeIcon.Plus.ToIconString() ) ) {
                            var newId = BONE_ID++;
                            var newBone = new SklbBone( newId );
                            newBone.Name.Value = $"bone_{newId}";
                            CommandManager.Sklb.Add( new GenericAddCommand<SklbBone>( Bones, newBone ) );
                        }
                    }
                    UiUtils.Tooltip( "在根部创建新骨骼" );

                    // Expand
                    ImGui.SameLine();
                    using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                        if( ImGui.Button( FontAwesomeIcon.Expand.ToIconString() ) ) {
                            expandAll = true;
                        }
                    }
                    UiUtils.Tooltip( "展开所有树状节点" );

                    // Search
                    ImGui.SameLine();
                    ImGui.InputTextWithHint( "##Search", "搜索", ref SearchText, 255 );
                }

                using var left = ImRaii.Child( "Left" );
                style.Pop();

                using var indent = ImRaii.PushStyle( ImGuiStyleVar.IndentSpacing, 9 );

                // Draw left column
                Bones.Where( x => x.Parent == null ).ToList().ForEach( x => DrawTree( x, searchSet, expandAll ) );

                // Drag-drop to root
                using var rootStyle = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 0 ) );
                rootStyle.Push( ImGuiStyleVar.FramePadding, new Vector2( 0 ) );

                ImGui.BeginChild( "EndChild", new Vector2( ImGui.GetContentRegionAvail().X, 1 ), false );
                ImGui.EndChild();

                using var dragDrop = ImRaii.DragDropTarget();
                if( dragDrop ) StopDragging( null );
            }

            if( !DrawOnce ) {
                ImGui.SetColumnWidth( 0, 200 );
                DrawOnce = true;
            }
            ImGui.NextColumn();

            using( var right = ImRaii.Child( "Right" ) ) {
                // Draw right column
                if( Selected != null ) {
                    using var font = ImRaii.PushFont( UiBuilder.IconFont );
                    if( UiUtils.TransparentButton( FontAwesomeIcon.Times.ToIconString(), new( 0.7f, 0.7f, 0.7f, 1 ) ) ) {
                        ClearSelected();
                        UpdatePreview();
                    }
                }

                if( Selected != null ) {
                    DrawParentCombo( Selected );
                    Selected.DrawBody( Bones.IndexOf( Selected ) );

                    using var color = ImRaii.PushColor( ImGuiCol.Button, UiUtils.RED_COLOR );
                    if( UiUtils.IconButton( FontAwesomeIcon.Trash, "删除" ) ) DeleteBone( Selected );
                }

                SklbPreview.DrawInline();
            }

            ImGui.Columns( 1 );
        }

        private void DrawParentCombo( SklbBone bone ) {
            using var combo = ImRaii.Combo( "父级", bone.Parent == null ? "[NONE]" : bone.Parent.Name.Value );
            if( !combo ) return;

            if( ImGui.Selectable( "[无]", bone.Parent == null ) ) {
                CommandManager.Sklb.Add( new SklbBoneParentCommand( bone, null ) );
            }

            var idx = 0;

            foreach( var item in Bones ) {
                if( item == bone ) continue;
                using var _ = ImRaii.PushId( idx );
                var selected = bone.Parent == item;

                if( ImGui.Selectable( item.Name.Value, selected ) ) {
                    CommandManager.Sklb.Add( new SklbBoneParentCommand( bone, item ) );
                }

                if( selected ) ImGui.SetItemDefaultFocus();
                idx++;
            }
        }

        private void DrawTree( SklbBone bone, HashSet<SklbBone> searchSet, bool expandAll ) {
            if( searchSet != null && !searchSet.Contains( bone ) ) return;

            var children = Bones.Where( x => x.Parent == bone ).ToList();
            var isLeaf = children.Count == 0;

            var flags =
                ImGuiTreeNodeFlags.DefaultOpen |
                ImGuiTreeNodeFlags.OpenOnArrow |
                ImGuiTreeNodeFlags.OpenOnDoubleClick |
                ImGuiTreeNodeFlags.SpanFullWidth;

            if( isLeaf ) flags |= ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen;
            if( Selected == bone ) flags |= ImGuiTreeNodeFlags.Selected;

            if( expandAll ) ImGui.SetNextItemOpen( true );
            var nodeOpen = ImGui.TreeNodeEx( $"{bone.Name.Value}##{bone.Id}", flags );

            DragDrop( bone );

            if( ImGui.BeginPopupContextItem() ) {
                if( UiUtils.IconSelectable( FontAwesomeIcon.Plus, "创建子骨骼" ) ) {
                    var newId = BONE_ID++;
                    var newBone = new SklbBone( newId );
                    newBone.Name.Value = $"bone_{newId}";
                    var command = new CompoundCommand();
                    command.Add( new GenericAddCommand<SklbBone>( Bones, newBone ) );
                    command.Add( new SklbBoneParentCommand( newBone, bone ) );
                    CommandManager.Sklb.Add( command );
                }

                if( UiUtils.IconSelectable( FontAwesomeIcon.Trash, "删除" ) ) {
                    DeleteBone( bone );
                    ImGui.CloseCurrentPopup();
                }

                bone.Name.Draw( CommandManager.Sklb, 128, "##Rename", ImGuiInputTextFlags.AutoSelectAll );
                ImGui.EndPopup();
            }

            if( ImGui.IsItemClicked( ImGuiMouseButton.Left ) && !ImGui.IsItemToggledOpen() ) {
                Selected = bone;
                UpdatePreview();
            }

            if( !isLeaf && nodeOpen ) {
                children.ForEach( x => DrawTree( x, searchSet, expandAll ) );
                ImGui.TreePop();
            }
        }

        private HashSet<SklbBone> GetSearchSet() {
            if( string.IsNullOrEmpty( SearchText ) ) return null;
            var searchSet = new HashSet<SklbBone>();

            var validBones = Bones.Where( x => x.Name.Value.ToLower().Contains( SearchText.ToLower() ) ).ToList();
            validBones.ForEach( x => PopulateSearchSet( searchSet, x ) );

            return searchSet;
        }

        private static void PopulateSearchSet( HashSet<SklbBone> searchSet, SklbBone bone ) {
            searchSet.Add( bone );
            if( bone.Parent != null ) PopulateSearchSet( searchSet, bone.Parent );
        }

        // ======= DRAGGING ==========

        private void DragDrop( SklbBone bone ) {
            if( ImGui.BeginDragDropSource( ImGuiDragDropFlags.None ) ) {
                StartDragging( bone );
                ImGui.Text( bone.Name.Value );
                ImGui.EndDragDropSource();
            }

            if( ImGui.BeginDragDropTarget() ) {
                StopDragging( bone );
                ImGui.EndDragDropTarget();
            }
        }

        private void StartDragging( SklbBone bone ) {
            ImGui.SetDragDropPayload( "SKLB_BONES", IntPtr.Zero, 0 );
            DraggingBone = bone;
        }

        public unsafe bool StopDragging( SklbBone destination ) {
            if( DraggingBone == null ) return false;
            var payload = ImGui.AcceptDragDropPayload( "SKLB_BONES" );
            if( payload.NativePtr == null ) return false;

            if( DraggingBone != destination ) {
                if( destination != null && destination.IsChildOf( DraggingBone ) ) {
                    PluginLog.Log( "Tried to put bone into itself" );
                }
                else {
                    CommandManager.Sklb.Add( new SklbBoneParentCommand( DraggingBone, destination ) );
                }
            }

            DraggingBone = null;
            return true;
        }

        public void ClearSelected() {
            Selected = null;
        }

        // ======= IMPORT EXPORT ==========

        private void ExportHavok() {
            FileDialogManager.SaveFileDialog( "选择保存位置", ".hkx", "", "hkx", ( bool ok, string res ) => {
                if( ok ) System.IO.File.Copy( Path, res, true );
            } );
        }

        private void ExportGltf() {
            FileDialogManager.SaveFileDialog( "选择保存位置", ".gltf", "skeleton", "gltf", ( bool ok, string res ) => {
                if( ok ) GltfSkeleton.ExportSkeleton( Bones, res );
            } );
        }

        private void ImportDialog() {
            FileDialogManager.OpenFileDialog( "选择文件", "Skeleton{.hkx,.gltf},.*", ( bool ok, string res ) => {
                if( !ok ) return;
                if( res.Contains( ".hkx" ) ) {
                    var importHavok = new HavokBones( res );
                    var newBones = importHavok.Bones;
                    importHavok.RemoveReference();
                    CommandManager.Sklb.Add( new SklbBonesImportCommand( this, newBones ) );
                }
                else {
                    try {
                        var newBones = GltfSkeleton.ImportSkeleton( res, Bones );
                        CommandManager.Sklb.Add( new SklbBonesImportCommand( this, newBones ) );
                    }
                    catch( Exception e ) {
                        PluginLog.Error( e, "无法导入数据" );
                    }
                }
            } );
        }

        // ======= UPDATING ==========

        public void Write() {
            var handles = new List<nint>();

            Mappings.ForEach( x => x.Write( handles ) );

            var bones = new List<hkaBone>();
            var poses = new List<hkQsTransformf>();
            var parents = new List<short>();

            foreach( var bone in Bones ) {
                var parent = ( short )( bone.Parent == null ? -1 : Bones.IndexOf( bone.Parent ) );
                parents.Add( parent );

                bone.ToHavok( out var hkBone, out var hkPose, out var handle );
                bones.Add( hkBone );
                poses.Add( hkPose );
                handles.Add( handle );
            }

            Skeleton->Bones = CreateArray( Skeleton->Bones, bones, out var boneHandle );
            handles.Add( boneHandle );

            Skeleton->ReferencePose = CreateArray( Skeleton->ReferencePose, poses, out var poseHandle );
            handles.Add( poseHandle );

            Skeleton->ParentIndices = CreateArray( Skeleton->ParentIndices, parents, out var parentHandle );
            handles.Add( parentHandle );

            WriteHavok();
            handles.ForEach( Marshal.FreeHGlobal );
        }

        private void UpdatePreview() {
            if( BoneList?.Count == 0 ) {
                SklbPreview.LoadEmpty( File );
            }
            else {
                var selectedIdx = Selected == null ? -1 : Bones.IndexOf( Selected );
                SkeletonMeshBuilder builder = Plugin.Configuration.SklbBoneDisplay switch {
                    BoneDisplay.Connected => new ConnectedSkeletonMeshBuilder( BoneList, selectedIdx ),
                    BoneDisplay.Blender_Style_Perpendicular => new DisconnectedSkeletonMeshBuilder( BoneList, selectedIdx, true ),
                    BoneDisplay.Blender_Style_Inline => new DisconnectedSkeletonMeshBuilder( BoneList, selectedIdx, false ),
                    _ => null
                };

                SklbPreview.LoadSkeleton( File, BoneList, builder.Build() );
            }
        }

        public void Updated() {
            UpdateBones();
            if( File == SklbPreview.CurrentFile ) UpdatePreview();
        }

        private void DeleteBone( SklbBone bone ) {
            var toDelete = new List<SklbBone> {
                bone
            };
            PopulateChildren( bone, toDelete );

            if( toDelete.Contains( Selected ) ) ClearSelected();

            var command = new CompoundCommand();
            foreach( var item in toDelete ) {
                command.Add( new GenericRemoveCommand<SklbBone>( Bones, item ) );
            }
            CommandManager.Sklb.Add( command );
        }

        public void PopulateChildren( SklbBone parent, List<SklbBone> children ) {
            foreach( var bone in Bones ) {
                if( bone.Parent == parent ) {
                    if( children.Contains( bone ) ) continue;
                    children.Add( bone );
                    PopulateChildren( bone, children );
                }
            }
        }

        public void Dispose() {
            if( SklbPreview.CurrentFile == File ) SklbPreview.ClearFile();
        }
    }
}
