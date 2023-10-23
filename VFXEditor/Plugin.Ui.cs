using Dalamud.Game.ClientState.Objects.Types;
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
        public static bool InGpose => PluginInterface.UiBuilder.GposeActive;
        public static GameObject GposeTarget => Objects.CreateObjectReference( new IntPtr( TargetSystem.Instance()->GPoseTarget ) );
        public static GameObject PlayerObject => InGpose ? GposeTarget : ClientState?.LocalPlayer;
        public static GameObject TargetObject => InGpose ? GposeTarget : TargetManager?.Target;

        public static readonly Dictionary<string, Modal> Modals = new();

        public static void Draw() {
            if( Loading ) return;

            CopyManager.ResetAll();
            CheckWorkspaceKeybinds();

            TexToolsDialog.Draw();
            PenumbraDialog.Draw();
            ToolsDialog.Draw();
            Tracker.Draw();
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
                ImGui.TextDisabled( "工作区" );
                ImGui.SameLine();
                UiUtils.HelpMarker( "工作区允许你一次保存多个视效的替换、导入的材质以及物品重命名(如: 粒子效果、触发器)" );

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
            DrawManagerMenu( AvfxManager, currentManager );
            DrawManagerMenu( TmbManager, currentManager );
            DrawManagerMenu( PapManager, currentManager );
            DrawManagerMenu( ScdManager, currentManager );
            DrawManagerMenu( UldManager, currentManager );
            DrawManagerMenu( PhybManager, currentManager );
            DrawManagerMenu( SklbManager, currentManager );

            if( ImGui.BeginMenu( "附加" ) ) {
                DrawManagerMenu( EidManager, currentManager );
                DrawManagerMenu( AtchManager, currentManager );
                ImGui.EndMenu();
            }
        }

        private static void DrawManagerMenu( IFileManager manager, IFileManager currentManager ) {
            using var disabled = ImRaii.Disabled( manager == currentManager );
            if( ImGui.MenuItem( manager.GetId() ) ) manager.Show();
        }

        public static void AddModal( Modal modal ) {
            Modals[modal.Title] = modal;
            ImGui.OpenPopup( modal.Title );
        }
    }
}




