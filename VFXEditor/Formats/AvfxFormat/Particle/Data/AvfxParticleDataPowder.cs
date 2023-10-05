using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataPowder : AvfxData {
        public readonly AvfxBool IsLightning = new( "是否点亮", "bLgt" );
        public readonly AvfxEnum<DirectionalLightType> DirectionalLightType = new( "定向光源类型", "LgtT" );
        public readonly AvfxFloat CenterOffset = new( "中心偏移", "CnOf" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataPowder() : base() {
            Parsed = new() {
                IsLightning,
                DirectionalLightType,
                CenterOffset
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( DirectionalLightType );
            Display.Add( IsLightning );
            Display.Add( CenterOffset );
        }
    }
}
