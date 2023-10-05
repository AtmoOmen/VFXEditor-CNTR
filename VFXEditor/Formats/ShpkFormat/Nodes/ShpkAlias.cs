using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.Formats.ShpkFormat.Nodes {
    public class ShpkAlias : IUiItem {
        public readonly ParsedUIntHex Selector = new( "选择器" );
        public readonly ParsedUInt NodeIdx = new( "节点索引" );

        public ShpkAlias() { }

        public ShpkAlias( BinaryReader reader ) {
            Selector.Read( reader );
            NodeIdx.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Selector.Write( writer );
            NodeIdx.Write( writer );
        }

        public void Draw() {
            Selector.Draw( CommandManager.Shpk );
            NodeIdx.Draw( CommandManager.Shpk );
        }
    }
}
