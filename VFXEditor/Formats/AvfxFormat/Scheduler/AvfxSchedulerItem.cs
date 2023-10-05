using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.AvfxFormat {
    public class AvfxSchedulerItem : GenericWorkspaceItem {
        public readonly AvfxScheduler Scheduler;
        public readonly string Name;

        public readonly AvfxBool Enabled = new( "启用", "bEna", value: true );
        public readonly AvfxInt StartTime = new( "开始时间", "StTm", value: 0 );
        public readonly AvfxInt TimelineIdx = new( "时间线索引", "TlNo", value: -1 );

        private readonly List<AvfxBase> Parsed;

        public AvfxNodeSelect<AvfxTimeline> TimelineSelect;

        private readonly List<IUiItem> Display;

        public AvfxSchedulerItem( AvfxScheduler scheduler, string name, bool initNodeSelects ) {
            Scheduler = scheduler;
            Name = name;

            Parsed = new() {
                Enabled,
                StartTime,
                TimelineIdx
            };

            if( initNodeSelects ) InitializeNodeSelects();

            Display = new() {
                Enabled,
                StartTime
            };
        }

        public AvfxSchedulerItem( AvfxScheduler scheduler, bool initNodeSelects, BinaryReader reader, string name ) : this( scheduler, name, initNodeSelects ) => AvfxBase.ReadNested( reader, Parsed, 36 );

        public void InitializeNodeSelects() {
            TimelineSelect = new AvfxNodeSelect<AvfxTimeline>( Scheduler, "时间线", Scheduler.NodeGroups.Timelines, TimelineIdx );
        }

        public void Write( BinaryWriter writer ) => AvfxBase.WriteNested( writer, Parsed );

        public override void Draw() {
            using var _ = ImRaii.PushId( Name );
            DrawRename();
            TimelineSelect.Draw();
            AvfxBase.DrawItems( Display );
        }

        public override string GetDefaultText() => $"{GetIdx()}: {TimelineSelect.GetText()}";

        public override string GetWorkspaceId() {
            var type = ( Name == "Item" ) ? "Item" : "触发器";
            return $"{Scheduler.GetWorkspaceId()}/{type}{GetIdx()}";
        }
    }
}
