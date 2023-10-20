using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleSimple : AvfxParticleAttribute {
        public readonly AvfxInt InjectionPositionType = new( "注入位置类型", "SIPT" );
        public readonly AvfxInt InjectionDirectionType = new( "注入方向类型", "SIDT" );
        public readonly AvfxInt BaseDirectionType = new( "基准方向类型", "SBDT" );
        public readonly AvfxInt CreateCount = new( "创建数", "CCnt" );
        public readonly AvfxFloat CreateAreaX = new( "创建 X 轴区域", "CrAX" );
        public readonly AvfxFloat CreateAreaY = new( "创建 Y 轴区域", "CrAY" );
        public readonly AvfxFloat CreateAreaZ = new( "创建 Z 轴区域", "CrAZ" );
        public readonly AvfxFloat CoordAccuracyX = new( "X 轴坐标精度", "CAX" );
        public readonly AvfxFloat CoordAccuracyY = new( "Y 轴坐标精度", "CAY" );
        public readonly AvfxFloat CoordAccuracyZ = new( "Z 轴坐标精度", "CAZ" );
        public readonly AvfxFloat CoordGraX = new( "X 轴坐标梯度", "CGX" );
        public readonly AvfxFloat CoordGraY = new( "Y 轴坐标梯度", "CGY" );
        public readonly AvfxFloat CoordGraZ = new( "Z 轴坐标梯度", "CGZ" );
        public readonly AvfxFloat ScaleXStart = new( "缩放起始 X 轴分量", "SBX" );
        public readonly AvfxFloat ScaleYStart = new( "缩放起始 Y 轴分量", "SBY" );
        public readonly AvfxFloat ScaleXEnd = new( "缩放结束 X 轴分量", "SEX" );
        public readonly AvfxFloat ScaleYEnd = new( "缩放结束 Y 轴分量", "SEY" );
        public readonly AvfxFloat ScaleCurve = new( "缩放曲线", "SC" );
        public readonly AvfxFloat ScaleRandX0 = new( "X 轴随机缩放 - 0", "SRX0" );
        public readonly AvfxFloat ScaleRandX1 = new( "X 轴随机缩放 - 1", "SRX1" );
        public readonly AvfxFloat ScaleRandY0 = new( "Scale Random Y 0 ", "SRY0" );
        public readonly AvfxFloat ScaleRandY1 = new( "Y 轴随机缩放 - 1", "SRY1" );
        public readonly AvfxFloat RotXStart = new( "旋转起始 X 轴分量", "RIX" );
        public readonly AvfxFloat RotYStart = new( "旋转起始 Y 轴分量", "RIY" );
        public readonly AvfxFloat RotZStart = new( "旋转起始 Z 轴分量", "RIZ" );
        public readonly AvfxFloat RotXAdd = new( "X 轴旋转增量", "RAX" );
        public readonly AvfxFloat RotYAdd = new( "Y 轴旋转增量", "RAY" );
        public readonly AvfxFloat RotZAdd = new( "Z 轴旋转增量", "RAZ" );
        public readonly AvfxFloat RotXBase = new( "X 轴旋转基值", "RBX" );
        public readonly AvfxFloat RotYBase = new( "Y 轴旋转基值", "RBY" );
        public readonly AvfxFloat RotZBase = new( "Z 轴旋转基值", "RBZ" );
        public readonly AvfxFloat RotXVel = new( "X 轴旋转速度", "RVX" );
        public readonly AvfxFloat RotYVel = new( "Y 轴旋转速度", "RVY" );
        public readonly AvfxFloat RotZVel = new( "Z 轴旋转速度", "RVZ" );
        public readonly AvfxFloat VelMin = new( "速度最小值", "VMin" );
        public readonly AvfxFloat VelMax = new( "速度最大值", "VMax" );
        public readonly AvfxFloat VelFlatteryRate = new( "速度平缓率", "FltR" );
        public readonly AvfxFloat VelFlatterySpeed = new( "速度平缓速", "FltS" );
        public readonly AvfxInt UvCellU = new( "平面坐标系水平分量", "UvCU" );
        public readonly AvfxInt UvCellV = new( "平面坐标系垂直分量", "UvCV" );
        public readonly AvfxInt UvInterval = new( "平面坐标区间", "UvIv" );
        public readonly AvfxInt UvNoRandom = new( "随机平面坐标", "UvNR" );
        public readonly AvfxInt UvNoLoopCount = new( "平面坐标循环数", "UvLC" );
        public readonly AvfxInt InjectionModelIdx = new( "注入模型索引", "IJMN", defaultValue: -1 );
        public readonly AvfxInt InjectionVertexBindModelIdx = new( "注入模型绑定索引", "VBMN", defaultValue: -1 );
        public readonly AvfxFloat InjectionRadialDir0 = new( "辐射方向注入 - 0", "IRD0" );
        public readonly AvfxFloat InjectionRadialDir1 = new( "辐射方向注入 - 1", "IRD1" );
        public readonly AvfxFloat PivotX = new( "中心点 X", "PvtX" );
        public readonly AvfxFloat PivotY = new( "中心点 Y", "PvtY" );
        public readonly AvfxInt BlockNum = new( "区块编号", "BlkN" );
        public readonly AvfxFloat LineLengthMin = new( "最短线长", "LLin" );
        public readonly AvfxFloat LineLengthMax = new( "最长线长", "LLax" );
        public readonly AvfxInt CreateIntervalVal = new( "创建间隔", "CrI" );
        public readonly AvfxInt CreateIntervalRandom = new( "随机创建间隔", "CrIR" );
        public readonly AvfxInt CreateIntervalCount = new( "创建间隔数", "CrIC" );
        public readonly AvfxInt CreateIntervalLife = new( "创建间隔生命周期", "CrIL" );
        public readonly AvfxInt CreateNewAfterDelete = new( "死亡后创建新对象", "bCrN", size: 1 );
        public readonly AvfxInt UvReverse = new( "平面坐标反转", "bRUV", size: 1 );
        public readonly AvfxInt ScaleRandomLink = new( "随机缩放关联", "bSRL", size: 1 );
        public readonly AvfxInt BindParent = new( "绑定父级", "bBnP", size: 1 );
        public readonly AvfxInt ScaleByParent = new( "根据父级缩放", "bSnP", size: 1 );
        public readonly AvfxInt PolyLineTag = new( "折线标签", "PolT" );

        public readonly AvfxSimpleColors Colors = new();
        public readonly AvfxSimpleFrames Frames = new();

        private readonly List<AvfxBase> Parsed;

        private readonly UiDisplayList AnimationDisplay;
        private readonly UiDisplayList TextureDisplay;
        private readonly UiDisplayList ColorDisplay;

        public AvfxParticleSimple( AvfxParticle particle ) : base( "Smpl", particle ) {
            InitNodeSelects();

            Parsed = new() {
                InjectionPositionType,
                InjectionDirectionType,
                BaseDirectionType,
                CreateCount,
                CreateAreaX,
                CreateAreaY,
                CreateAreaZ,
                CoordAccuracyX,
                CoordAccuracyY,
                CoordAccuracyZ,
                CoordGraX,
                CoordGraY,
                CoordGraZ,
                ScaleXStart,
                ScaleYStart,
                ScaleXEnd,
                ScaleYEnd,
                ScaleCurve,
                ScaleRandX0,
                ScaleRandX1,
                ScaleRandY0,
                ScaleRandY1,
                RotXStart,
                RotYStart,
                RotZStart,
                RotXAdd,
                RotYAdd,
                RotZAdd,
                RotXBase,
                RotYBase,
                RotZBase,
                RotXVel,
                RotYVel,
                RotZVel,
                VelMin,
                VelMax,
                VelFlatteryRate,
                VelFlatterySpeed,
                UvCellU,
                UvCellV,
                UvInterval,
                UvNoRandom,
                UvNoLoopCount,
                InjectionModelIdx,
                InjectionVertexBindModelIdx,
                InjectionRadialDir0,
                InjectionRadialDir1,
                PivotX,
                PivotY,
                BlockNum,
                LineLengthMin,
                LineLengthMax,
                CreateIntervalVal,
                CreateIntervalRandom,
                CreateIntervalCount,
                CreateIntervalLife,
                CreateNewAfterDelete,
                UvReverse,
                ScaleRandomLink,
                BindParent,
                ScaleByParent,
                PolyLineTag,
                Colors,
                Frames
            };

            Display.Add( InjectionPositionType );
            Display.Add( InjectionDirectionType );
            Display.Add( BaseDirectionType );
            Display.Add( CreateCount );
            Display.Add( new UiFloat3( "创建区域", CreateAreaX, CreateAreaY, CreateAreaZ ) );
            Display.Add( new UiFloat3( "坐标精确度", CoordAccuracyX, CoordAccuracyY, CoordAccuracyZ ) );
            Display.Add( new UiFloat3( "坐标梯度", CoordGraX, CoordGraY, CoordGraZ ) );
            Display.Add( InjectionRadialDir0 );
            Display.Add( InjectionRadialDir1 );
            Display.Add( BlockNum );
            Display.Add( LineLengthMin );
            Display.Add( LineLengthMax );
            Display.Add( CreateIntervalVal );
            Display.Add( CreateIntervalRandom );
            Display.Add( CreateIntervalCount );
            Display.Add( CreateIntervalLife );
            Display.Add( CreateNewAfterDelete );

            DisplayTabs.Add( AnimationDisplay = new UiDisplayList( "动画" ) );
            AnimationDisplay.Add( new UiFloat2( "起始缩放", ScaleXStart, ScaleYStart ) );
            AnimationDisplay.Add( new UiFloat2( "结束缩放", ScaleXEnd, ScaleYEnd ) );
            AnimationDisplay.Add( ScaleCurve );
            AnimationDisplay.Add( new UiFloat2( "随机 X 轴缩放", ScaleRandX0, ScaleRandX1 ) );
            AnimationDisplay.Add( new UiFloat2( "随机 Y 轴缩放", ScaleRandY0, ScaleRandY1 ) );
            AnimationDisplay.Add( new UiFloat3( "旋转增量", RotXAdd, RotYAdd, RotZAdd ) );
            AnimationDisplay.Add( new UiFloat3( "旋转基数", RotXBase, RotYBase, RotZBase ) );
            AnimationDisplay.Add( new UiFloat3( "旋转速度", RotXVel, RotYVel, RotZVel ) );
            AnimationDisplay.Add( VelMin );
            AnimationDisplay.Add( VelMax );
            AnimationDisplay.Add( VelFlatteryRate );
            AnimationDisplay.Add( VelFlatterySpeed );
            AnimationDisplay.Add( ScaleRandomLink );
            AnimationDisplay.Add( BindParent );
            AnimationDisplay.Add( ScaleByParent );
            AnimationDisplay.Add( PolyLineTag );

            DisplayTabs.Add( TextureDisplay = new UiDisplayList( "材质" ) );
            TextureDisplay.Add( UvCellU );
            TextureDisplay.Add( UvCellV );
            TextureDisplay.Add( UvInterval );
            TextureDisplay.Add( UvNoRandom );
            TextureDisplay.Add( UvNoLoopCount );
            TextureDisplay.Add( UvReverse );

            DisplayTabs.Add( ColorDisplay = new UiDisplayList( "颜色" ) );
            ColorDisplay.Add( new UiSimpleColor( Frames.Frame1, Colors.Color1 ) );
            ColorDisplay.Add( new UiSimpleColor( Frames.Frame2, Colors.Color2 ) );
            ColorDisplay.Add( new UiSimpleColor( Frames.Frame3, Colors.Color3 ) );
            ColorDisplay.Add( new UiSimpleColor( Frames.Frame4, Colors.Color4 ) );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            ReadNested( reader, Parsed, size );
            EnableAllSelectors();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        protected override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( "Simple" );

            AssignedCopyPaste( this, GetDefaultText() );
            if( ImGui.SmallButton( "+ 简易动画" ) ) Assign();
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( "Simple" );

            AssignedCopyPaste( this, GetDefaultText() );
            if( UiUtils.RemoveButton( "删除", small: true ) ) {
                Unassign();
                return;
            }
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => "简易动画";

        public override List<UiNodeSelect> GetNodeSelects() => new() {
            new UiNodeSelect<AvfxModel>( Particle, "注入模型", Particle.NodeGroups.Models, InjectionModelIdx ),
            new UiNodeSelect<AvfxModel>( Particle, "注入顶点绑定模型", Particle.NodeGroups.Models, InjectionVertexBindModelIdx )
        };
    }
}
