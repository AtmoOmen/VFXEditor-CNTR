using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutLineData : ScdLayoutData {
        public readonly ParsedFloat4 StartPosition = new( "开始位置" );
        public readonly ParsedFloat4 EndPosition = new( "结束位置" );
        public readonly ParsedFloat MaxRange = new( "范围最大值" );
        public readonly ParsedFloat MinRange = new( "范围最小值" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat RangeVolume = new( "范围音量" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat DopplerFac = new( "多普勒效应强度系数" );
        public readonly ParsedFloat InteriorFac = new( "内部效应强度系数" );
        public readonly ParsedFloat Direction = new( "方向" );
        public readonly ParsedReserve Reserved1 = new( 4 );

        public LayoutLineData() {
            Parsed = new() {
                StartPosition,
                EndPosition,
                MaxRange,
                MinRange,
                Height,
                RangeVolume,
                Volume,
                Pitch,
                ReverbFac,
                DopplerFac,
                InteriorFac,
                Direction,
                Reserved1
            };
        }
    }
}
