using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class GaugeComponentData : UldGenericData {
        public GaugeComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedUInt( "未知节点 ID 5" ),
                new ParsedUInt( "未知节点 ID 6" ),
                new ParsedUInt( "Vertical Margin", size: 2 ),
                new ParsedUInt( "Horizontal Margin", size: 2 ),
                new ParsedByteBool( "是否垂直" ),
                new ParsedReserve( 3 ) // Padding
            } );
        }
    }
}
