using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataModel : AvfxData {
        public readonly AvfxInt ModelNumberRandomValue = new( "随机模型编号", "MNRv" );
        public readonly AvfxEnum<RandomType> ModelNumberRandomType = new( "随机模型编号类型", "MNRt" );
        public readonly AvfxInt ModelNumberRandomInterval = new( "随机模型编号间隔", "MNRi" );
        public readonly AvfxEnum<FresnelType> FresnelType = new( "菲涅尔类型", "FrsT" );
        public readonly AvfxEnum<DirectionalLightType> DirectionalLightType = new( "定向光源类型", "DLT" );
        public readonly AvfxEnum<PointLightType> PointLightType = new( "点光源类型", "PLT" );
        public readonly AvfxBool IsLightning = new( "是否点亮", "bLgt" );
        public readonly AvfxBool IsMorph = new( "是否变形", "bShp" );
        public AvfxIntList ModelIdx = new( "模型索引", "MdNo", defaultValue: -1 );
        public readonly AvfxCurve AnimationNumber = new( "动画编号", "NoAn" );
        public readonly AvfxCurve Morph = new( "变形", "Moph" );
        public readonly AvfxCurve FresnelCurve = new( "菲涅尔曲线", "FrC" );
        public readonly AvfxCurve3Axis FresnelRotation = new( "菲涅尔旋转", "FrRt", CurveType.Angle );
        public readonly AvfxCurveColor ColorBegin = new( name: "起始颜色", "ColB" );
        public readonly AvfxCurveColor ColorEnd = new( name: "结束颜色", "ColE" );

        public readonly UiNodeSelectList<AvfxModel> ModelSelect;
        public readonly UiDisplayList Display;

        public AvfxParticleDataModel( AvfxParticle particle ) : base() {
            Parsed = new() {
                ModelNumberRandomValue,
                ModelNumberRandomType,
                ModelNumberRandomInterval,
                FresnelType,
                DirectionalLightType,
                PointLightType,
                IsLightning,
                IsMorph,
                ModelIdx,
                AnimationNumber,
                Morph,
                FresnelCurve,
                FresnelRotation,
                ColorBegin,
                ColorEnd
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( ModelSelect = new UiNodeSelectList<AvfxModel>( particle, "模型", particle.NodeGroups.Models, ModelIdx ) );
            Display.Add( ModelNumberRandomValue );
            Display.Add( ModelNumberRandomType );
            Display.Add( ModelNumberRandomInterval );
            Display.Add( FresnelType );
            Display.Add( DirectionalLightType );
            Display.Add( PointLightType );
            Display.Add( IsLightning );
            Display.Add( IsMorph );

            DisplayTabs.Add( Morph );
            DisplayTabs.Add( FresnelCurve );
            DisplayTabs.Add( FresnelRotation );
            DisplayTabs.Add( ColorBegin );
            DisplayTabs.Add( ColorEnd );
            DisplayTabs.Add( AnimationNumber );
        }

        public override void Enable() => ModelSelect.Enable();

        public override void Disable() => ModelSelect.Disable();
    }
}
