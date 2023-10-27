using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class ListItemNodeData : UldNodeComponentData {
        public ListItemNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedByteBool( "切换" ),
                new ParsedReserve( 3 ),
            } );
        }
    }
}
