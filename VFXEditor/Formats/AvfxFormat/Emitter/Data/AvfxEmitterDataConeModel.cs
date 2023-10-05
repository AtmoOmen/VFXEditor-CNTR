using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataConeModel : AvfxData {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxInt DivideX = new( "X 轴分割", "DivX" );
        public readonly AvfxInt DivideY = new( "Y 轴分割", "DivY" );
        public readonly AvfxCurve AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve AXR = new( "随机 X 轴角度", "AnXR", CurveType.Angle );
        public readonly AvfxCurve AYR = new( "随机 Y 轴角度", "AnYR", CurveType.Angle );
        public readonly AvfxCurve Radius = new( "半径", "Rad" );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );
        public readonly AvfxCurve InjectionAngle = new( "注入角度", "IjA", CurveType.Angle );
        public readonly AvfxCurve InjectionAngleRandom = new( "随机注入角度", "IjAR", CurveType.Angle );

        public readonly UiDisplayList Display;

        public AvfxEmitterDataConeModel() : base() {
            Parsed = new() {
                RotationOrderType,
                GenerateMethodType,
                DivideX,
                DivideY,
                AX,
                AY,
                AXR,
                AYR,
                Radius,
                InjectionSpeed,
                InjectionSpeedRandom,
                InjectionAngle,
                InjectionAngleRandom
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( RotationOrderType );
            Display.Add( GenerateMethodType );
            Display.Add( DivideX );
            Display.Add( DivideY );
            DisplayTabs.Add( AX );
            DisplayTabs.Add( AY );
            DisplayTabs.Add( AXR );
            DisplayTabs.Add( AYR );
            DisplayTabs.Add( Radius );
            DisplayTabs.Add( InjectionSpeed );
            DisplayTabs.Add( InjectionSpeedRandom );
            DisplayTabs.Add( InjectionAngle );
            DisplayTabs.Add( InjectionAngleRandom );
        }
    }
}
