using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class TabbedNodeData : UldNodeComponentData {
        public TabbedNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "文本 ID", size: 2 ),
                new ParsedUInt( "组 ID", size: 2 ),
            } );
        }
    }
}
