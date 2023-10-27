using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class DropDownComponentData : UldGenericData {
        public DropDownComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
            } );
        }
    }
}
