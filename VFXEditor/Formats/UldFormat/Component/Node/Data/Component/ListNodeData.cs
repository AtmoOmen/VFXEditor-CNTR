using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class ListNodeData : UldNodeComponentData {
        public ListNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "行", size: 2 ),
                new ParsedUInt( "列", size: 2 ),
            } );
        }
    }
}
