using System;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    [Flags]
    public enum PolygonFlags {
        IsReverbObject = 0x80
    }

    public class LayoutPolygonData : ScdLayoutData {
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
        public readonly ParsedFlag<PolygonFlags> Flag = new( "标识", size: 1 );
        public readonly ParsedByte VertexCount = new( "顶点数量" );
        public readonly ParsedReserve Reserved1 = new( 1 );
        public readonly ParsedFloat RotSpeed = new( "旋转速度" );
        public readonly ParsedReserve Reserved2 = new( 3 * 4 );

        public LayoutPolygonData() {
            Parsed = new() {
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
                Flag,
                VertexCount,
                Reserved1,
                RotSpeed,
                Reserved2,
                // Positions go here
            };

            for( var i = 0; i < 32; i++ ) Parsed.Add( new ParsedFloat4( $"Position {i}" ) );
        }
    }
}
