using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutPolylineData : ScdLayoutData {
        public readonly ParsedFloat MaxRange = new( "范围最大值" );
        public readonly ParsedFloat MinRange = new( "范围最小值" );
        public readonly ParsedFloat2 Height = new( "高度" );
        public readonly ParsedFloat RangeVolume = new( "范围音量" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat DopplerFac = new( "多普勒效应强度系数" );
        public readonly ParsedByte VertexCount = new( "顶点数量" );
        public readonly ParsedReserve Reserved1 = new( 3 );
        public readonly ParsedFloat InteriorFac = new( "内部效应强度系数" );
        public readonly ParsedFloat Direction = new( "方向" );

        public LayoutPolylineData() {
            Parsed = new() {
                // Positions go here
                MaxRange,
                MinRange,
                Height,
                RangeVolume,
                Volume,
                Pitch,
                ReverbFac,
                DopplerFac,
                VertexCount,
                Reserved1,
                InteriorFac,
                Direction
            };

            for( var i = 0; i < 16; i++ ) Parsed.Insert( 0, new ParsedFloat4( $"Position {15 - i}" ) );
        }
    }
}
