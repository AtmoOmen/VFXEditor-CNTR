using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class LayoutAmbientData : ScdLayoutData {
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedFloat ReverbFac = new( "混音强度系数" );
        public readonly ParsedFloat4 DirectVolume1 = new( "直接音量 1" );
        public readonly ParsedFloat4 DirectVolume2 = new( "直接音量 2" );
        public readonly ParsedReserve Reserved = new( 4 );

        public LayoutAmbientData() {
            Parsed = new() {
                Volume,
                Pitch,
                ReverbFac,
                DirectVolume1,
                DirectVolume2,
                Reserved
            };
        }
    }
}
