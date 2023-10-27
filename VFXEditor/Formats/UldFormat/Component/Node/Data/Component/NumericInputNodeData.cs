using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class NumericInputNodeData : UldNodeComponentData {
        public NumericInputNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedInt( "值" ),
                new ParsedInt( "最大值" ),
                new ParsedInt( "最小值" ),
                new ParsedInt( "新增" ),
                new ParsedUInt( "未知 1" ),
                new ParsedByteBool( "逗号" ),
                new ParsedReserve( 3 ),
            } );
        }
    }
}
