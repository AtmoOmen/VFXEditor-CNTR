using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutPointDirData : ScdLayoutData {
        public readonly ParsedFloat4 Position = new( "位置" );
        public readonly ParsedFloat4 Direction = new( "方向" );
        public readonly ParsedFloat RangeX = new( "X 轴范围" );
        public readonly ParsedFloat RangeY = new( "Y 轴范围" );
        public readonly ParsedFloat MaxRange = new( "范围最大值" );
        public readonly ParsedFloat MinRange = new( "范围最小值" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat RangeVolume = new( "范围音量" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat DopplerFac = new( "多普勒效应强度系数" );
        public readonly ParsedFloat InteriorFac = new( "内部效应强度系数" );
        public readonly ParsedFloat FixedDirection = new( "固定方向" );
        public readonly ParsedReserve Reserved1 = new( 3 * 4 );

        public LayoutPointDirData() {
            Parsed = new() {
                Position,
                Direction,
                RangeX,
                RangeY,
                MaxRange,
                MinRange,
                Height,
                RangeVolume,
                Volume,
                Pitch,
                ReverbFac,
                DopplerFac,
                InteriorFac,
                FixedDirection,
                Reserved1
            };
        }
    }
}
