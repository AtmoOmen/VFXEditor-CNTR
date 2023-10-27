using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class PreviewComponentData : UldGenericData {
        public PreviewComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
            } );
        }
    }
}
