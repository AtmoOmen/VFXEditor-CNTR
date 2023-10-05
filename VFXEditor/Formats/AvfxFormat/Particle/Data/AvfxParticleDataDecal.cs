namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDecal : AvfxData {
        public readonly AvfxFloat ScalingScale = new( "缩放比例", "SS" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataDecal() : base() {
            Parsed = new() {
                ScalingScale
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( ScalingScale );
        }
    }
}
