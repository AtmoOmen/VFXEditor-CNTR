using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutDirectionData : ScdLayoutData {
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat Direction = new( "方向" );
        public readonly ParsedFloat RotSpeed = new( "旋转速度" );
        public readonly ParsedReserve Reserved = new( 3 * 4 );

        public LayoutDirectionData() {
            Parsed = new() {
                Volume,
                Pitch,
                ReverbFac,
                Direction,
                RotSpeed,
                Reserved
            };
        }
    }
}
