using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using VfxEditor.Data;
using VfxEditor.FileManager.Interfaces;
using VfxEditor.Ui.Components;
using VfxEditor.Ui.Export;
using VfxEditor.Utils;

namespace VfxEditor {
    public unsafe partial class Plugin {
        public static bool InGpose => Dalamud.PluginInterface.UiBuilder.GposeActive;
        public static GameObject GposeTarget => Dalamud.Objects.CreateObjectReference( new IntPtr( TargetSystem.Instance()->GPoseTarget ) );
        public static GameObject PlayerObject => InGpose ? GposeTarget : Dalamud.ClientState?.LocalPlayer;
        public static GameObject TargetObject => InGpose ? GposeTarget : Dalamud.TargetManager?.Target;

        public static readonly Dictionary<string, Modal> Modals = new();

        public static void Draw() {
            if( Loading ) return;

            CopyManager.ResetAll();
            CheckWorkspaceKeybinds();

            TexToolsDialog.Draw();
            PenumbraDialog.Draw();
            ToolsDialog.Draw();
            TrackerManager.Draw();
            Configuration.Draw();
            LibraryManager.Draw();

            Managers.ForEach( x => x?.Draw() );

            CopyManager.FinalizeAll();

            if( Configuration.AutosaveEnabled &&
                Configuration.AutosaveSeconds > 10 &&
                !string.IsNullOrEmpty( CurrentWorkspaceLocation ) &&
                ( DateTime.Now - LastAutoSave ).TotalSeconds > Configuration.AutosaveSeconds
            ) {
                LastAutoSave = DateTime.Now;
                SaveWorkspace();
            }

            foreach( var modal in Modals ) modal.Value.Draw();
        }

        private static void CheckWorkspaceKeybinds() {
            if( Configuration.OpenKeybind.KeyPressed() ) OpenWorkspace();
            if( Configuration.SaveKeybind.KeyPressed() ) SaveWorkspace();
            if( Configuration.SaveAsKeybind.KeyPressed() ) SaveAsWorkspace();
        }

        public static void DrawFileMenu() {
            using var _ = ImRaii.PushId( "Menu" );

            if( ImGui.BeginMenu( "文件" ) ) {
                if( ImGui.MenuItem( "新建" ) ) NewWorkspace();
                if( ImGui.MenuItem( "打开" ) ) OpenWorkspace();
                if( ImGui.MenuItem( "保存" ) ) SaveWorkspace();
                if( ImGui.MenuItem( "另存为" ) ) SaveAsWorkspace();

                ImGui.Separator();
                if( ImGui.MenuItem( "设置" ) ) Configuration.Show();
                if( ImGui.MenuItem( "工具" ) ) ToolsDialog.Show();
                if( ImGui.BeginMenu( "帮助" ) ) {
                    if( ImGui.MenuItem( "GitHub" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor" );
                    if( ImGui.MenuItem( "提交 Issue" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/issues" );
                    if( ImGui.MenuItem( "Wiki" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki" );
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            if( ImGui.BeginMenu( "导出" ) ) {
                if( ImGui.MenuItem( "Penumbra" ) ) PenumbraDialog.Show();
                if( ImGui.MenuItem( "TexTools" ) ) TexToolsDialog.Show();
                ImGui.EndMenu();
            }
        }

        public static void DrawManagersMenu( IFileManager currentManager ) {
            using var _ = ImRaii.PushId( "Menu" );

            if( ImGui.MenuItem( "材质" ) ) TextureManager.Show();
            ImGui.Separator();

            // Manually specify the order since it's different than the load order
            var managers = new IFileManager[] {
                AvfxManager,
                TmbManager,
                PapManager,
                ScdManager,
                UldManager,
                SklbManager,
                SkpManager,
                PhybManager,
                EidManager,
                AtchManager,
                ShpkManager,
            };

            var dropdown = false;

            for( var i = 0; i < managers.Length; i++ ) {
                var manager = managers[i];

                if( !dropdown && i < ( managers.Length - 1 ) ) { // no need for a dropdown for the last one
                    var width = ImGui.CalcTextSize( manager.GetId() ).X + ( 2 * ImGui.GetStyle().ItemSpacing.X ) + 10;

                    if( width > ImGui.GetContentRegionAvail().X ) {
                        dropdown = true;
                        using var font = ImRaii.PushFont( UiBuilder.IconFont );
                        if( !ImGui.BeginMenu( FontAwesomeIcon.CaretDown.ToIconString() ) ) return; // Menu hidden, just skip the rest
                    }
                }

                using var disabled = ImRaii.Disabled( manager == currentManager );
                if( ImGui.MenuItem( manager.GetId() ) ) manager.Show();
            }

            if( dropdown ) ImGui.EndMenu();
        }

        public static void AddModal( Modal modal ) {
            Modals[modal.Title] = modal;
            ImGui.OpenPopup( modal.Title );
        }
    }
}