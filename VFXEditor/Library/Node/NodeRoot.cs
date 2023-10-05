using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;

namespace VfxEditor.Library.Node {
    public class NodeRoot : LibraryFolder {
        public NodeRoot( List<LibraryProps> items ) : base( null, "", "", items ) { }

        public override bool Draw( LibraryManager library, string searchInput ) {
            using var child = ImRaii.Child( "子级", ImGui.GetContentRegionAvail(), false );

            if( Children.Count == 0 ) ImGui.TextDisabled( "未保存任何节点..." );
            return base.Draw( library, searchInput );
        }
    }
}
