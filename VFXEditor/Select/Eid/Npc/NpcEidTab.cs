using System.Collections.Generic;
using VfxEditor.Select.Shared.Npc;

namespace VfxEditor.Select.Eid.Npc {
    public class NpcEidTab : NpcTab {
        public NpcEidTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            DrawPath( "路径", Selected.GetEidPath(), Selected.Name );
        }

        protected override void GetLoadedFiles( NpcFilesStruct files, out List<string> loaded ) {
            loaded = new List<string>();
        }
    }
}
