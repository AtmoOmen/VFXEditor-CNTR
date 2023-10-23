using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataSphereModel : AvfxData {
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxInt DivideX = new( "X 轴分割", "DivX" );
        public readonly AvfxInt DivideY = new( "Y 轴分割", "DivY" );
        public readonly AvfxCurve Radius = new( "半径", "Rads" );
        public readonly AvfxCurve AZ = new( "Z 角度", "AnZ", CurveType.Angle );
        public readonly AvfxCurve AYR = new( "随机 Y 轴角度", "AnYR", CurveType.Angle );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );

        public readonly UiDisplayList Display;

        public AvfxEmitterDataSphereModel() : base() {
            Parsed = new() {
                RotationOrderType,
                GenerateMethodType,
                DivideX,
                DivideY,
                Radius,
                AZ,
                AYR,
                InjectionSpeed,
                InjectionSpeedRandom
            };
            DivideX.SetValue( 1 );
            DivideY.SetValue( 1 );

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( RotationOrderType );
            Display.Add( GenerateMethodType );
            Display.Add( DivideX );
            Display.Add( DivideY );
            DisplayTabs.Add( Radius );
            DisplayTabs.Add( AZ );
            DisplayTabs.Add( AYR );
            DisplayTabs.Add( InjectionSpeed );
            DisplayTabs.Add( InjectionSpeedRandom );
        }
    }
}
