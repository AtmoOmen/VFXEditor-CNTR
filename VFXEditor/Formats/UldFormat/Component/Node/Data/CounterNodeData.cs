using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public class CounterNodeData : UldGenericData {
        public CounterNodeData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "分部列表 ID", size: 2 ),
                new ParsedUInt( "未知 1", size: 2 ),
                new ParsedInt( "分部 Id", size: 1 ),
                new ParsedInt( "数字宽度", size: 1 ),
                new ParsedInt( "逗号宽度", size: 1 ),
                new ParsedInt( "空格宽度", size: 1 ),
                new ParsedUInt( "对齐", size: 2 ),
                new ParsedUInt( "未知 2", size: 2 ),
            } );
        }
    }
}
