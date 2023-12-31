﻿using ImGuiNET;
using OtterGui.Raii;
using SharpGLTF.Schema2;
using System.Collections.Generic;
using VfxEditor.PapFormat.Motion;
using VfxEditor.Ui.Components;
using VfxEditor.Utils;
using VfxEditor.Utils.Gltf;

namespace VfxEditor.PapFormat {
    public unsafe class PapGltfImportModal : Modal {
        private readonly PapMotion Motion;
        private readonly int HavokIndex;
        private readonly string ImportPath;

        private bool Compress = false;
        private int AnimationIndex = 0;
        private readonly List<string> AnimationNames = new();
        private readonly List<string> NodeNames = new();

        public PapGltfImportModal( PapMotion motion, int index, string importPath ) : base( "Animation Import" ) {
            Motion = motion;
            HavokIndex = index;
            ImportPath = importPath;

            var model = ModelRoot.Load( importPath );

            var boneNames = new List<string>();
            for( var i = 0; i < motion.Skeleton->Bones.Length; i++ ) {
                boneNames.Add( motion.Skeleton->Bones[i].Name.String );
            }

            foreach( var node in model.LogicalNodes ) {
                if( string.IsNullOrEmpty( node.Name ) || node.Name.ToLower().Contains( "mesh" ) || node.Name.ToLower().Contains( "armature" ) ) continue;
                if( !boneNames.Contains( node.Name ) || !node.IsTransformAnimated ) continue;
                NodeNames.Add( node.Name );
            }

            foreach( var animation in model.LogicalAnimations ) {
                AnimationNames.Add( animation.Name );
            }
        }

        protected override void DrawBody() {
            ImGui.Checkbox( "Compress Animation", ref Compress );

            var text = AnimationNames.Count == 0 ? "[无]" : AnimationNames[AnimationIndex];
            var combo = ImRaii.Combo( "Animation to Import", text );
            if( combo ) {
                for( var i = 0; i < AnimationNames.Count; i++ ) {
                    using var _ = ImRaii.PushId( i );
                    if( ImGui.Selectable( $"{AnimationNames[i]}##Name", i == AnimationIndex ) ) {
                        AnimationIndex = i;
                    }
                }
            }

            var nodes = ImRaii.TreeNode( "Nodes Being Imported" );
            if( nodes ) {
                using var child = ImRaii.Child( "子级", new( ImGui.GetContentRegionAvail().X, 300 ) );
                using var _ = ImRaii.PushIndent();
                foreach( var nodeName in NodeNames ) {
                    ImGui.Text( nodeName );
                }
            }
        }

        protected override void OnCancel() { }

        protected override void OnOk() {
            CommandManager.Pap.Add( new PapHavokCommand( Motion.File, () => {
                GltfAnimation.ImportAnimation(
                    Motion.File.MotionData.Skeleton,
                    Motion,
                    HavokIndex,
                    AnimationIndex,
                    Compress,
                    ImportPath );
            } ) );
            UiUtils.OkNotification( "已导入 Havok 数据" );
        }
    }
}
