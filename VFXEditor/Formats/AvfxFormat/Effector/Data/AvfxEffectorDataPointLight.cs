using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataPointLight : AvfxData {
        public readonly AvfxCurveColor Color = new( "颜色" );
        public readonly AvfxCurve DistanceScale = new( "距离缩放", "DstS" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos" );
        public readonly AvfxEnum<PointLightAttenuation> PointLightAttenuationType = new( "点光源衰减", "Attn" );
        public readonly AvfxBool EnableShadow = new( "启用阴影", "bSdw" );
        public readonly AvfxBool EnableCharShadow = new( "启用角色阴影", "bChS" );
        public readonly AvfxBool EnableMapShadow = new( "启用地图阴影", "bMpS" );
        public readonly AvfxBool EnableMoveShadow = new( "启用移动阴影", "bMvS" );
        public readonly AvfxFloat ShadowCreateDistanceNear = new( "创建近距离效果", "SCDN" );
        public readonly AvfxFloat ShadowCreateDistanceFar = new( "创建远距离效果", "SCDF" );

        public readonly UiDisplayList Display;

        public AvfxEffectorDataPointLight() : base() {
            Parsed = new() {
                Color,
                DistanceScale,
                Rotation,
                Position,
                PointLightAttenuationType,
                EnableShadow,
                EnableCharShadow,
                EnableMapShadow,
                EnableMoveShadow,
                ShadowCreateDistanceNear,
                ShadowCreateDistanceFar
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( PointLightAttenuationType );
            Display.Add( EnableShadow );
            Display.Add( EnableCharShadow );
            Display.Add( EnableMapShadow );
            Display.Add( EnableMoveShadow );
            Display.Add( ShadowCreateDistanceNear );
            Display.Add( ShadowCreateDistanceFar );

            DisplayTabs.Add( Color );
            DisplayTabs.Add( DistanceScale );
            DisplayTabs.Add( Rotation );
            DisplayTabs.Add( Position );
        }
    }
}
