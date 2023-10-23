using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    public enum CollisionType : int {
        Hit = 0x0,
        Focus = 0x1,
        Move = 0x2,
    }

    public class CollisionNodeData : UldGenericData {
        public CollisionNodeData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedEnum<CollisionType>( "碰撞类型", size: 2 ),
                new ParsedUInt( "未知 1", size: 2 ),
                new ParsedInt( "X" ),
                new ParsedInt( "Y" ),
                new ParsedUInt( "半径" ),
            } );
        }
    }
}
