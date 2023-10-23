using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxBinderProperties : AvfxOptional {
        public readonly string Name;

        public readonly AvfxEnum<BindPoint> BindPointType = new( "绑定点类型", "BPT" );
        public readonly AvfxEnum<BindTargetPoint> BindTargetPointType = new( "绑定目标点类型", "BPTP", defaultValue: BindTargetPoint.ByName );
        public readonly AvfxString BinderName = new( "名称", "Name", showRemoveButton: true );
        public readonly AvfxInt BindPointId = new( "绑定点 ID", "BPID", defaultValue: 3 );
        public readonly AvfxInt GenerateDelay = new( "生成延迟", "GenD" );
        public readonly AvfxInt CoordUpdateFrame = new( "坐标更新帧", "CoUF", defaultValue: -1 );
        public readonly AvfxBool RingEnable = new( "启用环", "bRng" );
        public readonly AvfxInt RingProgressTime = new( "环进度时间", "RnPT", defaultValue: 1 );
        public readonly AvfxFloat RingPositionX = new( "X 轴环位置", "RnPX" );
        public readonly AvfxFloat RingPositionY = new( "Y 轴环位置", "RnPY" );
        public readonly AvfxFloat RingPositionZ = new( "Z 轴环位置", "RnPZ" );
        public readonly AvfxFloat RingRadius = new( "环半径", "RnRd" );
        public readonly AvfxInt BCT = new( "BCT", "BCT" );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos", locked: true );

        private readonly List<AvfxBase> Parsed;

        private readonly UiDisplayList Parameters;
        private readonly List<INamedUiItem> DisplayTabs;

        private static readonly Dictionary<int, string> BinderIds = new() {
            { 0, "未工作" },
            { 1, "头部" },
            { 3, "左手武器" },
            { 4, "右手武器" },
            { 6, "右肩" },
            { 7, "左肩" },
            { 8, "右前臂" },
            { 9, "左前臂" },
            { 10, "右小腿" },
            { 11, "左小腿" },
            { 16, "角色前方" },
            { 25, "头部" },
            { 26, "头部" },
            { 27, "头部" },
            { 28, "颈部" },
            { 29, "角色中心" },
            { 30, "角色中心" },
            { 31, "角色中心" },
            { 32, "右手" },
            { 33, "左手" },
            { 34, "右脚" },
            { 35, "左脚" },
            { 42, "角色上方?" },
            { 43, "头部 (偏右眼)" },
            { 44, "头部 (偏左眼)" },
            { 77, "怪物武器" },
        };

        public AvfxBinderProperties( string name, string avfxName ) : base( avfxName ) {
            Name = name;

            Parsed = new() {
                BindPointType,
                BindTargetPointType,
                BinderName,
                BindPointId,
                GenerateDelay,
                CoordUpdateFrame,
                RingEnable,
                RingProgressTime,
                RingPositionX,
                RingPositionY,
                RingPositionZ,
                RingRadius,
                BCT,
                Position
            };
            BinderName.SetAssigned( false );
            Position.SetAssigned( true );

            DisplayTabs = new() {
                ( Parameters = new UiDisplayList( "参数" ) ),
                Position
            };

            Parameters.AddRange( new() {
                BindPointType,
                BindTargetPointType,
                BinderName,
                new UiIntCombo( "绑定点 ID", BindPointId, BinderIds ),
                GenerateDelay,
                CoordUpdateFrame,
                RingEnable,
                RingProgressTime,
                new UiFloat3( "环位置", RingPositionX, RingPositionY, RingPositionZ ),
                RingRadius,
                BCT
            } );
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        protected override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( Name );

            AssignedCopyPaste( this, Name );
            DrawAddButtonRecurse( this, Name );
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( Name );

            AssignedCopyPaste( this, Name );
            DrawRemoveButton( this, Name );

            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => Name;
    }
}
