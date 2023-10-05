namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataLinear : AvfxData {
        public readonly AvfxCurve CarryOverFactor = new( "传递因子", "COF" );
        public readonly AvfxCurve CarryOverFactorRandom = new( "随机传递因子", "COFR" );

        public AvfxBinderDataLinear() : base() {
            Parsed = new() {
                CarryOverFactor,
                CarryOverFactorRandom
            };

            DisplayTabs.Add( CarryOverFactor );
            DisplayTabs.Add( CarryOverFactorRandom );
        }
    }
}
