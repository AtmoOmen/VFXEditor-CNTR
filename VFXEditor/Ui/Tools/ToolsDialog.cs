using ImGuiNET;
using OtterGui.Raii;

namespace VfxEditor.Ui.Tools {
    public partial class ToolsDialog : GenericDialog {
        private readonly ResourceTab ResourceTab;
        private readonly UtilitiesTab UtilitiesTab;
        private readonly LoadedTab LoadedTab;
        private readonly LuaTab LuaTab;

        public ToolsDialog() : base( "工具", false, 300, 400 ) {
            ResourceTab = new ResourceTab();
            UtilitiesTab = new UtilitiesTab();
            LoadedTab = new LoadedTab();
            LuaTab = new LuaTab();
        }

        public override void DrawBody() {
            using var _ = ImRaii.PushId( "ToolsTab" );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "资源" ) ) {
                ResourceTab.Draw();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "工具" ) ) {
                UtilitiesTab.Draw();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Loaded Files" ) ) {
                LoadedTab.Draw();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Lua Variables" ) ) {
                LuaTab.Draw();
                ImGui.EndTabItem();
            }
        }
    }
}