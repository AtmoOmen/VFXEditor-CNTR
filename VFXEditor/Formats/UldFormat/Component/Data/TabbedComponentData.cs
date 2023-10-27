using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class TabbedComponentData : UldGenericData {
        public TabbedComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
            } );
        }
    }
}
