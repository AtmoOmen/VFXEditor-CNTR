using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinder : AvfxNode {
        public const string NAME = "Bind";

        public readonly AvfxBool StartToGlobalDirection = new( "起始点至全局方向", "bStG" );
        public readonly AvfxBool VfxScaleEnabled = new( "视效缩放", "bVSc" );
        public readonly AvfxFloat VfxScaleBias = new( "视效缩放偏移", "bVSb" );
        public readonly AvfxBool VfxScaleDepthOffset = new( "视效缩放深度偏移", "bVSd" );
        public readonly AvfxBool VfxScaleInterpolation = new( "视效缩放插值", "bVSi" );
        public readonly AvfxBool TransformScale = new( "变换缩放", "bTSc" );
        public readonly AvfxBool TransformScaleDepthOffset = new( "变换缩放深度偏移", "bTSd" );
        public readonly AvfxBool TransformScaleInterpolation = new( "变换缩放插值", "bTSi" );
        public readonly AvfxBool FollowingTargetOrientation = new( "跟随目标面向", "bFTO" );
        public readonly AvfxBool DocumentScaleEnabled = new( "启用文档缩放", "bDSE" );
        public readonly AvfxBool AdjustToScreenEnabled = new( "适应屏幕", "bATS" );
        public readonly AvfxBool BET_Unknown = new( "BET (未知)", "bBET" );
        public readonly AvfxInt Life = new( "生命周期", "Life" );
        public readonly AvfxEnum<BinderRotation> BinderRotationType = new( "绑定器选择类型", "RoTp" );
        public readonly AvfxEnum<BinderType> BinderVariety = new( "类型", "BnVr" );
        public readonly AvfxBinderProperties PropStart = new( "起始属性", "PrpS" );
        public readonly AvfxBinderProperties Prop1 = new( "属性 1", "Prp1" );
        public readonly AvfxBinderProperties Prop2 = new( "属性 2", "Prp2" );
        public readonly AvfxBinderProperties PropGoal = new( "目标属性", "PrpG" );
        public AvfxData Data;

        private readonly List<AvfxBase> Parsed;

        private readonly AvfxDisplaySplitView<AvfxBinderProperties> PropSplitDisplay;
        private readonly UiDisplayList Parameters;

        public AvfxBinder() : base( NAME, AvfxNodeGroupSet.BinderColor ) {
            Parsed = new() {
                StartToGlobalDirection,
                VfxScaleEnabled,
                VfxScaleBias,
                VfxScaleDepthOffset,
                VfxScaleInterpolation,
                TransformScale,
                TransformScaleDepthOffset,
                TransformScaleInterpolation,
                FollowingTargetOrientation,
                DocumentScaleEnabled,
                AdjustToScreenEnabled,
                BET_Unknown,
                Life,
                BinderRotationType,
                BinderVariety,
                PropStart,
                Prop1,
                Prop2,
                PropGoal
            };

            BinderVariety.Command = () => {
                return new AvfxBinderDataCommand( this );
            };

            Parameters = new( "参数", new() {
                new UiNodeGraphView( this ),
                StartToGlobalDirection,
                VfxScaleEnabled,
                VfxScaleBias,
                VfxScaleDepthOffset,
                VfxScaleInterpolation,
                TransformScale,
                TransformScaleDepthOffset,
                TransformScaleInterpolation,
                FollowingTargetOrientation,
                DocumentScaleEnabled,
                AdjustToScreenEnabled,
                BET_Unknown,
                Life,
                BinderRotationType
            } );

            PropSplitDisplay = new AvfxDisplaySplitView<AvfxBinderProperties>( "Properties", new() {
                PropStart,
                Prop1,
                Prop2,
                PropGoal
            } );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );
            var binderType = BinderVariety.Value;

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Data" ) {
                    SetData( binderType );
                    Data?.Read( _reader, _size );
                }
            }, size );
        }

        protected override void RecurseChildrenAssigned( bool assigned ) {
            RecurseAssigned( Parsed, assigned );
            RecurseAssigned( Data, assigned );
        }

        public override void WriteContents( BinaryWriter writer ) {
            WriteNested( writer, Parsed );
            Data?.Write( writer );
        }

        public void SetData( BinderType type ) {
            Data = type switch {
                BinderType.Point => new AvfxBinderDataPoint(),
                BinderType.Linear => new AvfxBinderDataLinear(),
                BinderType.Spline => new AvfxBinderDataSpline(),
                BinderType.Camera => new AvfxBinderDataCamera(),
                _ => null,
            };
            Data?.SetAssigned( true );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Binder" );
            DrawRename();
            BinderVariety.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "参数" ) ) {
                if( tab ) Parameters.Draw();
            }

            DrawData();

            using( var tab = ImRaii.TabItem( "Properties" ) ) {
                if( tab ) PropSplitDisplay.Draw();
            }
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "Data" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"Binder {GetIdx()} ({BinderVariety.Value})";

        public override string GetWorkspaceId() => $"Bind{GetIdx()}";
    }
}
