namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLine : AvfxData {
        public readonly AvfxInt LineCount = new( "线数", "LnCT" );
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve LengthRandom = new( "随机长度", "LenR" );
        public readonly AvfxCurveColor ColorBegin = new( name: "起始颜色", "ColB" );
        public readonly AvfxCurveColor ColorEnd = new( name: "结束颜色", "ColE" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataLine() : base() {
            Parsed = new() {
                LineCount,
                Length,
                LengthRandom,
                ColorBegin,
                ColorEnd
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( LineCount );
            DisplayTabs.Add( Length );
            DisplayTabs.Add( LengthRandom );
            DisplayTabs.Add( ColorBegin );
            DisplayTabs.Add( ColorEnd );
        }
    }
}
