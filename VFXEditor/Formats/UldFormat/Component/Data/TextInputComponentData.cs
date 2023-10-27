using VfxEditor.Parsing;
using VfxEditor.Parsing.Color;

namespace VfxEditor.UldFormat.Component.Data {
    public class TextInputComponentData : UldGenericData {
        public TextInputComponentData() {
            AddUnknown( 16, "未知节点 ID" );

            Parsed.AddRange( new ParsedBase[] {
                new ParsedSheetColor( "颜色" ),
                new ParsedSheetColor( "输入法颜色" ),
            } );
        }
    }
}
