using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Data {
    public class TreeListComponentData : UldGenericData {
        public TreeListComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "未知节点 ID 1" ),
                new ParsedUInt( "未知节点 ID 2" ),
                new ParsedUInt( "未知节点 ID 3" ),
                new ParsedUInt( "未知节点 ID 4" ),
                new ParsedUInt( "未知节点 ID 5" ),
                new ParsedUInt( "环绕", size: 1),
                new ParsedUInt( "取向", size: 1 ),
                new ParsedUInt( "内边距", size: 2 )
            } );
        }
    }
}
