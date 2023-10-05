using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutLineExtControllerData : ScdLayoutData {
        public readonly ParsedFloat4 StartPosition = new( "开始位置" );
        public readonly ParsedFloat4 EndPosition = new( "结束位置" );
        public readonly ParsedFloat MaxRange = new( "范围最大值" );
        public readonly ParsedFloat MinRange = new( "范围最小值" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat RangeVolume = new( "范围音量" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat LowerLimit = new( "下限" );
        public readonly ParsedInt FunctionNumber = new( "函数编号" );
        public readonly ParsedByte CalcType = new( "计算类型" );
        public readonly ParsedReserve Reserved1 = new( 3 + 4 * 4 );

        public LayoutLineExtControllerData() {
            Parsed = new() {
                StartPosition,
                EndPosition,
                MaxRange,
                MinRange,
                Height,
                RangeVolume,
                Volume,
                LowerLimit,
                FunctionNumber,
                CalcType,
                Reserved1
            };
        }
    }
}
