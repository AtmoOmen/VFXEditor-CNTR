using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class SliderNodeData : UldNodeComponentData {
        public SliderNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedInt( "最小值" ),
                new ParsedInt( "最大值" ),
                new ParsedInt( "步长" ),
            } );
        }
    }
}
