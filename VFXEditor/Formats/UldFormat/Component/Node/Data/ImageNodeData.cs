using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public class ImageNodeData : UldGenericData {
        public ImageNodeData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "分部列表 ID", size: 2 ),
                new ParsedUInt( "未知 1", size: 2 ),
                new ParsedUInt( "分部 Id" ),
                new ParsedByteBool( "水平翻转" ),
                new ParsedByteBool( "垂直翻转" ),
                new ParsedInt( "环绕", size: 1 ),
                new ParsedInt( "未知 2", size: 1 )
            } );
        }
    }
}
