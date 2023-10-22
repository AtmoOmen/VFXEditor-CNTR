using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public enum GridPartsType : int {
        Divide = 0x0,
        Compose = 0x1,
    }

    public enum GridRenderType : int {
        Scale = 0x0,
        Tile = 0x1,
    }

    public class NineGridNodeData : UldGenericData {
        public NineGridNodeData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "分部列表 ID", size: 2 ),
                new ParsedUInt( "未知 1", size: 2 ),
                new ParsedUInt( "分部 Id" ),
                new ParsedEnum<GridPartsType>( "网格部分类型", size: 1 ),
                new ParsedEnum<GridRenderType>( "网格渲染类型", size: 1 ),
                new ParsedShort( "顶部偏移" ),
                new ParsedShort( "底部偏移" ),
                new ParsedShort( "左部偏移" ),
                new ParsedShort( "右部偏移" ),
                new ParsedInt( "未知 2", size: 1 ),
                new ParsedInt( "未知 3", size: 1 ),
            } );
        }
    }
}
