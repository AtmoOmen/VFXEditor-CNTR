using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class GaugeNodeData : UldNodeComponentData {
        public GaugeNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedInt( "Indicator" ),
                new ParsedInt( "最小值" ),
                new ParsedInt( "最大值" ),
                new ParsedInt( "值" ),
            } );
        }
    }
}
