using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutPolylineObstructionData : ScdLayoutData {
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat ObstacleFac = new( "障碍强度系数" );
        public readonly ParsedFloat HiCutFac = new( "高频截止强度系数" );
        public readonly ParsedFlag<ObstructionFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedByte VertexCount = new( "顶点数量" );
        public readonly ParsedReserve Reserved1 = new( 2 );
        public readonly ParsedFloat Width = new( "宽度" );
        public readonly ParsedFloat FadeRange = new( "淡化范围" );
        public readonly ParsedShort OpenTime = new( "开启时间" );
        public readonly ParsedShort CloseTime = new( "结束时间" );

        public LayoutPolylineObstructionData() {
            Parsed = new() {
                // Positions go here
                Height,
                ObstacleFac,
                HiCutFac,
                Flags,
                VertexCount,
                Reserved1,
                Width,
                FadeRange,
                OpenTime,
                CloseTime
            };

            for( var i = 0; i < 16; i++ ) Parsed.Insert( 0, new ParsedFloat4( $"Position {15 - i}" ) );
        }
    }
}
