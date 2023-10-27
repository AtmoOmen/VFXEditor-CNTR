using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class NineGridTextComponentData : UldGenericData {
        public NineGridTextComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
            } );
        }
    }
}
