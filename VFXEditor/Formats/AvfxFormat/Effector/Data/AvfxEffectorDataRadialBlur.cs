using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataRadialBlur : AvfxData {
        public readonly AvfxCurve Length = new( "长度", "Len" );
        public readonly AvfxCurve Strength = new( "强度", "Str" );
        public readonly AvfxCurve Gradation = new( "渐变", "Gra" );
        public readonly AvfxCurve InnerRadius = new( "内半径", "IRad" );
        public readonly AvfxCurve OuterRadius = new( "外半径", "ORad" );
        public readonly AvfxFloat FadeStartDistance = new( "开始淡出距离", "FSDc" );
        public readonly AvfxFloat FadeEndDistance = new( "结束淡出距离", "FEDc" );
        public readonly AvfxEnum<ClipBasePoint> FadeBasePointType = new( "Fade Base Point", "FaBP" );

        public readonly UiDisplayList Display;

        public AvfxEffectorDataRadialBlur() : base() {
            Parsed = new() {
                Length,
                Strength,
                Gradation,
                InnerRadius,
                OuterRadius,
                FadeStartDistance,
                FadeEndDistance,
                FadeBasePointType
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( FadeStartDistance );
            Display.Add( FadeEndDistance );
            Display.Add( FadeBasePointType );

            DisplayTabs.Add( Length );
            DisplayTabs.Add( Strength );
            DisplayTabs.Add( Gradation );
            DisplayTabs.Add( InnerRadius );
            DisplayTabs.Add( OuterRadius );
        }
    }
}
