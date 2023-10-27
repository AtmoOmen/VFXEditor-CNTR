using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class SliderComponentData : UldGenericData {
        public SliderComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedByteBool( "是否垂直" ),
                new ParsedUInt( "左部偏移", size: 1 ),
                new ParsedUInt( "右部偏移", size: 1),
                new ParsedInt( "内边距", size: 1)
            } );
        }
    }
}
