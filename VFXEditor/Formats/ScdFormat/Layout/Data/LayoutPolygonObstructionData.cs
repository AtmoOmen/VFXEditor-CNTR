using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutPolygonObstructionData : ScdLayoutData {
        public readonly ParsedFloat ObstacleFac = new( "障碍强度系数" );
        public readonly ParsedFloat HiCutFac = new( "高频截止强度系数" );
        public readonly ParsedFlag<ObstructionFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedByte VertexCount = new( "顶点数量" );
        public readonly ParsedReserve Reserved1 = new( 2 );
        public readonly ParsedShort OpenTime = new( "开启时间" );
        public readonly ParsedShort CloseTime = new( "结束时间" );

        public LayoutPolygonObstructionData() {
            Parsed = new() {
                ObstacleFac,
                HiCutFac,
                Flags,
                VertexCount,
                Reserved1,
                OpenTime,
                CloseTime
            };

            for( var i = 0; i < 32; i++ ) Parsed.Insert( 0, new ParsedFloat4( $"Position {15 - i}" ) );
        }
    }
}
