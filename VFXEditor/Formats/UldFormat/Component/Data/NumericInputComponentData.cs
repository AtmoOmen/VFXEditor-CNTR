using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class NumericInputComponentData : UldGenericData {
        public NumericInputComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedUInt( "未知节点 ID 5" ),
                new ParsedUInt( "颜色" ),
            } );
        }
    }
}
