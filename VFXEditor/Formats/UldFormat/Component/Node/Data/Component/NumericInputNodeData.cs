using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class NumericInputNodeData : UldNodeComponentData {
        public NumericInputNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedInt( "值" ),
                new ParsedInt( "Max" ),
                new ParsedInt( "Min" ),
                new ParsedInt( "Add" ),
                new ParsedUInt( "未知 1" ),
                new ParsedByteBool( "Comma" ),
                new ParsedReserve( 3 ),
            } );
        }
    }
}
