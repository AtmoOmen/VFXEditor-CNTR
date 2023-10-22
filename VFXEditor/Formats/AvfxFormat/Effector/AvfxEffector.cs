using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Nodes;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffector : AvfxNode {
        public const string NAME = "Efct";

        public readonly AvfxEnum<EffectorType> EffectorVariety = new( "Type", "EfVT" );
        public readonly AvfxEnum<RotationOrder> RotationOrder = new( "旋转顺序", "RoOT" );
        public readonly AvfxEnum<CoordComputeOrder> CoordComputeOrder = new( "坐标计算顺序", "CCOT" );
        public readonly AvfxBool AffectOtherVfx = new( "影响其他视效", "bAOV" );
        public readonly AvfxBool AffectGame = new( "影响游戏", "bAGm" );
        public readonly AvfxInt LoopPointStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopPointEnd = new( "循环结束", "LpEd" );
        public AvfxData Data;

        private readonly List<AvfxBase> Parsed;

        private readonly UiDisplayList Parameters;

        public AvfxEffector() : base( NAME, AvfxNodeGroupSet.EffectorColor ) {
            Parsed = new() {
                EffectorVariety,
                RotationOrder,
                CoordComputeOrder,
                AffectOtherVfx,
                AffectGame,
                LoopPointStart,
                LoopPointEnd
            };

            EffectorVariety.Parsed.ExtraCommandGenerator = () => {
                return new AvfxEffectorDataCommand( this );
            };

            Parameters = new( "参数", new() {
                new UiNodeGraphView( this ),
                RotationOrder,
                CoordComputeOrder,
                AffectOtherVfx,
                AffectGame,
                LoopPointStart,
                LoopPointEnd
            } );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );
            var effectorType = EffectorVariety.GetValue();

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Data" ) {
                    SetData( effectorType );
                    Data?.Read( _reader, _size );
                }
            }, size );
        }

        protected override void RecurseChildrenAssigned( bool assigned ) {
            RecurseAssigned( Parsed, assigned );
            RecurseAssigned( Data, assigned );
        }

        protected override void WriteContents( BinaryWriter writer ) {
            WriteNested( writer, Parsed );
            Data?.Write( writer );
        }

        public void SetData( EffectorType type ) {
            Data = type switch {
                EffectorType.PointLight => new AvfxEffectorDataPointLight(),
                EffectorType.DirectionalLight => new AvfxEffectorDataDirectionalLight(),
                EffectorType.RadialBlur => new AvfxEffectorDataRadialBlur(),
                EffectorType.BlackHole => null,
                EffectorType.CameraQuake2_Unknown or EffectorType.CameraQuake => new AvfxEffectorDataCameraQuake(),
                _ => null
            };
            Data?.SetAssigned( true );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Effector" );
            DrawRename();
            EffectorVariety.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "参数" ) ) {
                if( tab ) Parameters.Draw();
            }

            DrawData();
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "Data" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"效果器 {GetIdx()} ({EffectorVariety.GetValue()})";

        public override string GetWorkspaceId() => $"Effct{GetIdx()}";
    }
}
