﻿using Dalamud.Interface;
using ImGuiNET;
using OtterGui.Raii;
using System.IO;
using System.Linq;
using System.Numerics;
using VfxEditor.FileManager;
using VfxEditor.Select;
using VfxEditor.Spawn;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat {
    public class AvfxDocument : FileManagerDocument<AvfxFile, WorkspaceMetaRenamed> {
        public override string Id => "Vfx";
        public override string Extension => "avfx";

        private string SpawnPath => ReplacePath;
        private bool SpawnDisabled => string.IsNullOrEmpty( SpawnPath );

        public AvfxDocument( AvfxManager manager, string writeLocation ) : base( manager, writeLocation ) { }

        public AvfxDocument( AvfxManager manager, string writeLocation, string localPath, WorkspaceMetaRenamed data ) : this( manager, writeLocation ) {
            LoadWorkspace( localPath, data.RelativeLocation, data.Name, data.Source, data.Replace, data.Disabled );
            CurrentFile?.ReadRenamingMap( data.Renaming );
        }

        public override void CheckKeybinds() {
            base.CheckKeybinds();

            if( Plugin.Configuration.SpawnOnSelfKeybind.KeyPressed() ) {
                VfxSpawn.Remove();
                if( !SpawnDisabled ) VfxSpawn.OnSelf( SpawnPath, true );
            }

            if( Plugin.Configuration.SpawnOnGroundKeybind.KeyPressed() ) {
                VfxSpawn.Remove();
                if( !SpawnDisabled ) VfxSpawn.OnGround( SpawnPath, true );

            }

            if( Plugin.Configuration.SpawnOnTargetKeybind.KeyPressed() ) {
                VfxSpawn.Remove();
                if( !SpawnDisabled ) VfxSpawn.OnTarget( SpawnPath, true );
            }
        }

        protected override AvfxFile FileFromReader( BinaryReader reader ) => new( reader );

        public void Import( string path ) {
            if( CurrentFile != null && File.Exists( path ) ) CurrentFile.Import( path );
        }

        public void ShowExportDialog( AvfxNode node ) => CurrentFile.ShowExportDialog( node );

        public void OpenTemplate( string path ) =>
            SetSource( new SelectResult( SelectResultType.Local, "[模板]", Path.Combine( Plugin.RootLocation, "Files", path ) ) );

        public override WorkspaceMetaRenamed GetWorkspaceMeta( string newPath ) => new() {
            Name = Name,
            RelativeLocation = newPath,
            Replace = Replace,
            Source = Source,
            Renaming = CurrentFile.GetRenamingMap(),
            Disabled = Disabled
        };

        // ========= DRAWING =============

        protected override void DrawExtraColumn() {
            using var framePadding = ImRaii.PushStyle( ImGuiStyleVar.FramePadding, new Vector2( 4, 3 ) );
            using( var group = ImRaii.Group() ) {
                if( VfxSpawn.Exists ) {
                    if( ImGui.Button( "移除", new Vector2( 60, ImGui.GetFrameHeight() ) ) ) VfxSpawn.Remove();
                }
                else {
                    using var style = ImRaii.PushStyle( ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f, SpawnDisabled );
                    if( ImGui.Button( "生成", new Vector2( 60, ImGui.GetFrameHeight() ) ) && !SpawnDisabled ) ImGui.OpenPopup( "SpawnPopup" );
                }

                using( var _ = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 2, 4 ) ) ) {
                    ImGui.SameLine();
                }

                using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    if( ImGui.Button( FontAwesomeIcon.Cog.ToIconString(), new Vector2( 28, ImGui.GetFrameHeight() ) ) ) ImGui.OpenPopup( "SettingsPopup" );
                }

                if( ImGui.Button( "库", new Vector2( 90, ImGui.GetFrameHeight() ) ) ) Plugin.LibraryManager.Show();
            }

            var height = ImGui.GetItemRectSize().Y;

            using( var _ = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( ImGui.GetStyle().ItemSpacing.Y, 4 ) ) ) {
                ImGui.SameLine();
            }
            Plugin.Tracker.Vfx.DrawEye( new Vector2( 28, height ) );

            framePadding.Pop(); // so it doesn't mess with the popups

            if( ImGui.BeginPopup( "SpawnPopup" ) ) {
                if( ImGui.Selectable( "在地面" ) ) VfxSpawn.OnGround( SpawnPath, true );
                if( ImGui.Selectable( "在自身" ) ) VfxSpawn.OnSelf( SpawnPath, true );
                if( ImGui.Selectable( "在目标" ) ) VfxSpawn.OnTarget( SpawnPath, true );
                ImGui.EndPopup();
            }

            if( ImGui.BeginPopup( "SettingsPopup" ) ) {
                if( ImGui.Checkbox( "循环", ref Plugin.Configuration.VfxSpawnLoop ) ) Plugin.Configuration.Save();
                ImGui.SetNextItemWidth( 150 );
                if( ImGui.InputFloat( "延迟", ref Plugin.Configuration.VfxSpawnDelay ) ) Plugin.Configuration.Save();
                ImGui.EndPopup();
            }
        }

        protected override string GetWarningText() {
            var invalidTimeline = CurrentFile.TimelineView.Group.Items.Where( timeline => timeline.Items.Any( item => !item.HasValue ) ).FirstOrDefault();
            if( invalidTimeline == null ) return "";
            return $"时间线 [{invalidTimeline.GetText()}] 缺少数值";
        }
    }
}
