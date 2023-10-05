using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataCone : AvfxData {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxCurve AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve InnerSize = new( "内尺寸", "InS" );
        public readonly AvfxCurve OuterSize = new( "外尺寸", "OuS" );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );
        public readonly AvfxCurve InjectionAngle = new( "注入角度", "IjA", CurveType.Angle );

        public readonly UiDisplayList Display;

        public AvfxEmitterDataCone() : base() {
            Parsed = new() {
                RotationOrderType,
                AX,
                AY,
                InnerSize,
                OuterSize,
                InjectionSpeed,
                InjectionSpeedRandom,
                InjectionAngle
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( RotationOrderType );
            DisplayTabs.Add( AX );
            DisplayTabs.Add( AY );
            DisplayTabs.Add( InnerSize );
            DisplayTabs.Add( OuterSize );
            DisplayTabs.Add( InjectionSpeed );
            DisplayTabs.Add( InjectionSpeedRandom );
            DisplayTabs.Add( InjectionAngle );
        }
    }
}
