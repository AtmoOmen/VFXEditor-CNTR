using System;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    [Flags]
    public enum SurfaceFlags {
        IsReverbObject = 0x80
    }

    public class LayoutSurfaceData : ScdLayoutData {
        public readonly ParsedFloat4 Position1 = new( "位置 1" );
        public readonly ParsedFloat4 Position2 = new( "位置 2" );
        public readonly ParsedFloat4 Position3 = new( "位置 3" );
        public readonly ParsedFloat4 Position4 = new( "位置 4" );
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
        public readonly ParsedByte SubSoundType = new( "子声音类型" );
        public readonly ParsedFlag<SurfaceFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedReserve Reserved1 = new( 2 );
        public readonly ParsedFloat RotSpeed = new( "旋转速度" );
        public readonly ParsedReserve Reserved2 = new( 3 * 4 );

        public LayoutSurfaceData() {
            Parsed = new() {
                Position1,
                Position2,
                Position3,
                Position4,
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
                SubSoundType,
                Flags,
                Reserved1,
                RotSpeed,
                Reserved2
            };
        }
    }
}
