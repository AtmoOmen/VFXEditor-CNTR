namespace VfxEditor.AvfxFormat {
    public class AvfxBinderDataCamera : AvfxData {
        public readonly AvfxCurve Distance = new( "距离", "Dst" );
        public readonly AvfxCurve DistanceRandom = new( "随机距离", "DstR" );

        public AvfxBinderDataCamera() : base() {
            Parsed = new() {
                Distance,
                DistanceRandom
            };

            DisplayTabs.Add( Distance );
            DisplayTabs.Add( DistanceRandom );
        }
    }
}
