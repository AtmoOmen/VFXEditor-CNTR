using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class LeveCardComponentData : UldGenericData {
        public LeveCardComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
            } );
        }
    }
}
