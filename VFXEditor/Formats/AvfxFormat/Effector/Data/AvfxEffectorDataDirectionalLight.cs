namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataDirectionalLight : AvfxData {
        public readonly AvfxCurveColor Ambient = new( "环境", "Amb" );
        public readonly AvfxCurveColor Color = new( "颜色" );
        public readonly AvfxCurve Power = new( "强度", "Pow" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );

        public AvfxEffectorDataDirectionalLight() : base() {
            Parsed = new() {
                Ambient,
                Color,
                Power,
                Rotation
            };

            DisplayTabs.Add( Ambient );
            DisplayTabs.Add( Color );
            DisplayTabs.Add( Power );
            DisplayTabs.Add( Rotation );
        }
    }
}
