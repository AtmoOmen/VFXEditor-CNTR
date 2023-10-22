using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class ButtonComponentData : UldGenericData {
        public ButtonComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "文本节点 ID" ),
                new ParsedUInt( "背景节点 ID" ),
            } );
        }
    }
}
