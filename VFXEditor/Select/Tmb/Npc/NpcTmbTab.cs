using ImGuiNET;
using System.Collections.Generic;
using VfxEditor.Select.Shared.Npc;

namespace VfxEditor.Select.Tmb.Npc {
    public class NpcTmbTab : NpcTab {
        public NpcTmbTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            ImGui.Text( "分支: " + Selected.Variant );
            DrawPaths( "时间线", Loaded, Selected.Name );
        }

        protected override void GetLoadedFiles( NpcFilesStruct files, out List<string> loaded ) {
            loaded = files.tmb;
        }
    }
}
