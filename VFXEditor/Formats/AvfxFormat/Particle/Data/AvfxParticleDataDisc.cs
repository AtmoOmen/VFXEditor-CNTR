namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataDisc : AvfxData {
        public readonly AvfxInt PartsCount = new( "组件数量", "PrtC" );
        public readonly AvfxInt PartsCountU = new( "水平组件数量", "PCnU" );
        public readonly AvfxInt PartsCountV = new( "垂直组件数量", "PCnV" );
        public readonly AvfxFloat PointIntervalFactoryV = new( "垂直点间隔因子", "PIFU" );
        public readonly AvfxCurve Angle = new( "角度", "Ang", CurveType.Angle );
        public readonly AvfxCurve HeightBeginInner = new( "内部起始高度", "HBI" );
        public readonly AvfxCurve HeightEndInner = new( "内部结束高度", "HEI" );
        public readonly AvfxCurve HeightBeginOuter = new( "外部开始高度", "HBO" );
        public readonly AvfxCurve HeightEndOuter = new( "外部结束高度", "HEO" );
        public readonly AvfxCurve WidthBegin = new( "起始宽度", "WB" );
        public readonly AvfxCurve WidthEnd = new( "结束宽度", "WE" );
        public readonly AvfxCurve RadiusBegin = new( "半径起始点", "RB" );
        public readonly AvfxCurve RadiusEnd = new( "半径结束点", "RE" );
        public readonly AvfxCurveColor ColorEdgeInner = new( name: "内部边缘颜色", "CEI" );
        public readonly AvfxCurveColor ColorEdgeOuter = new( name: "外部边缘颜色", "CEO" );

        public readonly UiDisplayList Display;

        public AvfxParticleDataDisc() : base() {
            Parsed = new() {
                PartsCount,
                PartsCountU,
                PartsCountV,
                PointIntervalFactoryV,
                Angle,
                HeightBeginInner,
                HeightEndInner,
                HeightBeginOuter,
                HeightEndOuter,
                WidthBegin,
                WidthEnd,
                RadiusBegin,
                RadiusEnd,
                ColorEdgeInner,
                ColorEdgeOuter
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( PartsCount );
            Display.Add( PartsCountU );
            Display.Add( PartsCountV );
            Display.Add( PointIntervalFactoryV );
            DisplayTabs.Add( Angle );
            DisplayTabs.Add( HeightBeginInner );
            DisplayTabs.Add( HeightEndInner );
            DisplayTabs.Add( HeightBeginOuter );
            DisplayTabs.Add( HeightEndOuter );
            DisplayTabs.Add( WidthBegin );
            DisplayTabs.Add( WidthEnd );
            DisplayTabs.Add( RadiusBegin );
            DisplayTabs.Add( RadiusEnd );
            DisplayTabs.Add( ColorEdgeInner );
            DisplayTabs.Add( ColorEdgeOuter );
        }
    }
}
