using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutBoxObstructionData : ScdLayoutData {
        public readonly ParsedFloat4 Position1 = new( "位置 1" );
        public readonly ParsedFloat4 Position2 = new( "位置 2" );
        public readonly ParsedFloat4 Position3 = new( "位置 3" );
        public readonly ParsedFloat4 Position4 = new( "位置 4" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat ObstacleFac = new( "障碍强度系数" );
        public readonly ParsedFloat HiCutFac = new( "高频截止强度系数" );
        public readonly ParsedFlag<ObstructionFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedReserve Reserved1 = new( 3 );
        public readonly ParsedFloat FadeRange = new( "淡化范围" );
        public readonly ParsedShort OpenTime = new( "开启时间" );
        public readonly ParsedShort CloseTime = new( "结束时间" );
        public readonly ParsedReserve Reserved2 = new( 4 );

        public LayoutBoxObstructionData() {
            Parsed = new() {
                Position1,
                Position2,
                Position3,
                Position4,
                Height,
                ObstacleFac,
                HiCutFac,
                Flags,
                Reserved1,
                FadeRange,
                OpenTime,
                CloseTime,
                Reserved2
            };
        }
    }
}
