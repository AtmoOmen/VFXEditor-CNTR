using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class Unknown25ComponentData : UldGenericData {
        public Unknown25ComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
            } );
        }
    }
}
