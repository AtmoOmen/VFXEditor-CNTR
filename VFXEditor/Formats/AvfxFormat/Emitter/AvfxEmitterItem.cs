using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitterItem : GenericWorkspaceItem {
        public readonly bool IsParticle;
        public readonly AvfxEmitter Emitter;

        public readonly AvfxBool Enabled = new( "启用", "bEnb", defaultValue: false );
        public readonly AvfxInt TargetIdx = new( "目标索引", "TgtB", defaultValue: -1 );
        public readonly AvfxInt LocalDirection = new( "本地方向", "LoDr", defaultValue: 0 );
        public readonly AvfxInt CreateTime = new( "创建时间", "CrTm", defaultValue: 1 );
        public readonly AvfxInt CreateCount = new( "创建数", "CrCn", defaultValue: 1 );
        public readonly AvfxInt CreateProbability = new( "创建概率", "CrPr", defaultValue: 100 );
        public readonly AvfxEnum<ParentInfluenceCoordOptions> ParentInfluenceCoord = new( "对子级的影响", "PICd", defaultValue: ParentInfluenceCoordOptions.InitialPosition_WithOptions );
        public readonly AvfxEnum<ParentInfluenceColorOptions> ParentInfluenceColor = new( "对子级颜色的影响", "PICo", defaultValue: ParentInfluenceColorOptions.Initial );
        public readonly AvfxBool InfluenceCoordScale = new( "对缩放的影响", "ICbS", defaultValue: false );
        public readonly AvfxBool InfluenceCoordRot = new( "对旋转的影响", "ICbR", defaultValue: false );
        public readonly AvfxBool InfluenceCoordPos = new( "对位置的影响", "ICbP", defaultValue: true );
        public readonly AvfxBool InfluenceCoordBinderPosition = new( "对绑定点位置的影响", "ICbB", defaultValue: false );
        public readonly AvfxInt InfluenceCoordUnstickiness = new( "对坐标不粘性的影响", "ICSK", defaultValue: 0 );
        public readonly AvfxBool InheritParentVelocity = new( "继承父级速度", "IPbV", defaultValue: false );
        public readonly AvfxBool InheritParentLife = new( "继承父级生命周期", "IPbL", defaultValue: false );
        public readonly AvfxBool OverrideLife = new( "覆盖生命周期", "bOvr", defaultValue: false );
        public readonly AvfxInt OverrideLifeValue = new( "覆盖生命周期值", "OvrV", defaultValue: 60 );
        public readonly AvfxInt OverrideLifeRandom = new( "随机覆盖生命周期", "OvrR", defaultValue: 0 );
        public readonly AvfxInt ParameterLink = new( "参数链接", "PrLk", defaultValue: -1 );
        public readonly AvfxInt StartFrame = new( "起始帧", "StFr", defaultValue: 0 );
        public readonly AvfxBool StartFrameNullUpdate = new( "起始帧空更新", "bStN", defaultValue: false );
        public readonly AvfxFloat ByInjectionAngleX = new( "通过 X 轴注射角度", "BIAX", defaultValue: 0 );
        public readonly AvfxFloat ByInjectionAngleY = new( "通过 Y 轴注射角度", "BIAY", defaultValue: 0 );
        public readonly AvfxFloat ByInjectionAngleZ = new( "通过 Z 轴注射角度", "BIAZ", defaultValue: 0 );
        public readonly AvfxInt GenerateDelay = new( "生成延迟", "GenD", 0 );
        public readonly AvfxBool GenerateDelayByOne = new( "生成 1 单元延迟", "bGD", false );

        private readonly List<AvfxBase> Parsed;

        public UiNodeSelect<AvfxParticle> ParticleSelect;
        public UiNodeSelect<AvfxEmitter> EmitterSelect;

        private readonly List<IUiItem> Display;
        private readonly List<IUiItem> CoordOptionsDisplay;
        private readonly List<IUiItem> Display2;

        public AvfxEmitterItem( bool isParticle, AvfxEmitter emitter, bool initNodeSelects ) {
            IsParticle = isParticle;
            Emitter = emitter;

            Parsed = new() {
                Enabled,
                TargetIdx,
                LocalDirection,
                CreateTime,
                CreateCount,
                CreateProbability,
                ParentInfluenceCoord,
                ParentInfluenceColor,
                InfluenceCoordScale,
                InfluenceCoordRot,
                InfluenceCoordPos,
                InfluenceCoordBinderPosition,
                InfluenceCoordUnstickiness,
                InheritParentVelocity,
                InheritParentLife,
                OverrideLife,
                OverrideLifeValue,
                OverrideLifeRandom,
                ParameterLink,
                StartFrame,
                StartFrameNullUpdate,
                ByInjectionAngleX,
                ByInjectionAngleY,
                ByInjectionAngleZ,
                GenerateDelay,
                GenerateDelayByOne
            };

            if( initNodeSelects ) InitializeNodeSelects();

            Display = new() {
                Enabled,
                LocalDirection,
                CreateTime,
                CreateCount,
                CreateProbability,
                ParentInfluenceColor
            };

            CoordOptionsDisplay = new() {
                InfluenceCoordScale,
                InfluenceCoordRot,
                InfluenceCoordPos
            };

            Display2 = new() {
                InfluenceCoordBinderPosition,
                InfluenceCoordUnstickiness,
                InheritParentVelocity,
                InheritParentLife,
                OverrideLife,
                OverrideLifeValue,
                OverrideLifeRandom,
                ParameterLink,
                StartFrame,
                StartFrameNullUpdate,
                new UiFloat3( "通过注射角度", ByInjectionAngleX, ByInjectionAngleY, ByInjectionAngleZ ),
                GenerateDelay,
                GenerateDelayByOne
            };
        }

        public AvfxEmitterItem( bool isParticle, AvfxEmitter emitter, bool initNodeSelects, BinaryReader reader ) : this( isParticle, emitter, initNodeSelects ) => AvfxBase.ReadNested( reader, Parsed, 312 );

        public void InitializeNodeSelects() {
            if( IsParticle ) ParticleSelect = new UiNodeSelect<AvfxParticle>( Emitter, "目标粒子", Emitter.NodeGroups.Particles, TargetIdx );
            else EmitterSelect = new UiNodeSelect<AvfxEmitter>( Emitter, "目标发射器", Emitter.NodeGroups.Emitters, TargetIdx );
        }

        public void Write( BinaryWriter writer ) => AvfxBase.WriteNested( writer, Parsed );

        public override void Draw() {
            using var _ = ImRaii.PushId( "Item" );
            DrawRename();

            if( IsParticle ) ParticleSelect.Draw();
            else EmitterSelect.Draw();

            AvfxBase.DrawItems( Display );

            ParentInfluenceCoord.Draw();
            var influenceType = ParentInfluenceCoord.GetValue();
            var allowOptions = influenceType == ParentInfluenceCoordOptions.InitialPosition_WithOptions || influenceType == ParentInfluenceCoordOptions.WithOptions_NoPosition;
            if( !allowOptions ) ImGui.PushStyleVar( ImGuiStyleVar.Alpha, 0.5f );
            AvfxBase.DrawItems( CoordOptionsDisplay );
            if( !allowOptions ) ImGui.PopStyleVar();

            AvfxBase.DrawItems( Display2 );
        }

        public override string GetDefaultText() => IsParticle ? ParticleSelect.GetText() : EmitterSelect.GetText();

        public override string GetWorkspaceId() {
            var type = IsParticle ? "Ptcl" : "Emit";
            return $"{Emitter.GetWorkspaceId()}/{type}{GetIdx()}";
        }
    }
}
