using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataWindmill : AvfxData {
        public readonly AvfxEnum<WindmillUVType> WindmillUVType = new( "风车 UV 映射类型", "WUvT" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataWindmill() : base() {
            Parsed = new() {
                WindmillUVType
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( WindmillUVType );
        }
    }
}
