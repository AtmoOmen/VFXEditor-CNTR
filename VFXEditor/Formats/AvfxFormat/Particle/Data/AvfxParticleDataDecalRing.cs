namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDecalRing : AvfxData {
        public readonly AvfxCurve Width = new( "宽度", "WID" );
        public readonly AvfxFloat ScalingScale = new( "缩放比例", "SS" );
        public readonly AvfxFloat RingFan = new( "环形扇叶排布", "RF" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataDecalRing() : base() {
            Parsed = new() {
                Width,
                ScalingScale,
                RingFan
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( ScalingScale );
            Display.Add( RingFan );
            DisplayTabs.Add( Width );
        }
    }
}
