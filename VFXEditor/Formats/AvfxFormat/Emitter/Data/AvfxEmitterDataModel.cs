using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterDataModel : AvfxData {
        public readonly AvfxInt ModelIdx = new( "模型索引", "MdNo", defaultValue: -1 );
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转顺序", "ROT" );
        public readonly AvfxEnum<GenerateMethod> GenerateMethodType = new( "生成方法", "GeMT" );
        public readonly AvfxCurve AX = new( "X 轴角度", "AnX", CurveType.Angle );
        public readonly AvfxCurve AY = new( "Y 轴角度", "AnY", CurveType.Angle );
        public readonly AvfxCurve AZ = new( "Z 角度", "AnZ", CurveType.Angle );
        public readonly AvfxCurve InjectionSpeed = new( "注入速度", "IjS" );
        public readonly AvfxCurve InjectionSpeedRandom = new( "随机注入速度", "IjSR" );

        public readonly UiNodeSelect<AvfxModel> ModelSelect;
        public readonly UiDisplayList Display;

        public AvfxEmitterDataModel( AvfxEmitter emitter ) : base() {
            Parsed = new() {
                ModelIdx,
                RotationOrderType,
                GenerateMethodType,
                AX,
                AY,
                AZ,
                InjectionSpeed,
                InjectionSpeedRandom
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( ModelSelect = new UiNodeSelect<AvfxModel>( emitter, "模型", emitter.NodeGroups.Models, ModelIdx ) );
            Display.Add( RotationOrderType );
            Display.Add( GenerateMethodType );
            DisplayTabs.Add( AX );
            DisplayTabs.Add( AY );
            DisplayTabs.Add( AZ );
            DisplayTabs.Add( InjectionSpeed );
            DisplayTabs.Add( InjectionSpeedRandom );
        }

        public override void Enable() {
            ModelSelect.Enable();
        }

        public override void Disable() {
            ModelSelect.Disable();
        }
    }
}
