namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataCameraQuake : AvfxData {
        public readonly AvfxCurve Attenuation = new( "衰减", "Att" );
        public readonly AvfxCurve RadiusOut = new( "外半径", "RdO" );
        public readonly AvfxCurve RadiusIn = new( "内半径", "RdI" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos" );

        public AvfxEffectorDataCameraQuake() : base() {
            Parsed = new() {
                Attenuation,
                RadiusOut,
                RadiusIn,
                Rotation,
                Position
            };

            DisplayTabs.Add( Attenuation );
            DisplayTabs.Add( RadiusOut );
            DisplayTabs.Add( RadiusIn );
            DisplayTabs.Add( Rotation );
            DisplayTabs.Add( Position );
        }
    }
}
