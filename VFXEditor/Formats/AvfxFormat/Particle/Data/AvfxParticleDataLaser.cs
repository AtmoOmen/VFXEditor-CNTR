namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLaser : AvfxData {
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve Width = new( "宽度", "Wdt" );

        public AvfxParticleDataLaser() : base() {
            Parsed = new() {
                Length,
                Width
            };

            DisplayTabs.Add( Width );
            DisplayTabs.Add( Length );
        }
    }
}
