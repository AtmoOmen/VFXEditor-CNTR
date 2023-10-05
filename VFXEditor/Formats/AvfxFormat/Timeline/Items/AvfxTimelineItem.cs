using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.AvfxFormat {
    public class AvfxTimelineItem : GenericWorkspaceItem {
        public readonly AvfxTimeline Timeline;

        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxInt StartTime = new( "开始时间", "StTm" );
        public readonly AvfxInt EndTime = new( "结束时间", "EdTm" );
        public readonly AvfxInt BinderIdx = new( "绑定器索引", "BdNo", value: -1 );
        public readonly AvfxInt EffectorIdx = new( "效果器索引", "EfNo", value: -1 );
        public readonly AvfxInt EmitterIdx = new( "发射器索引", "EmNo", value: -1 );
        public readonly AvfxInt Platform = new( "平台", "Plfm" );
        public readonly AvfxInt ClipIdx = new( "片段索引", "ClNo" );

        private readonly List<AvfxBase> Parsed;

        public AvfxNodeSelect<AvfxBinder> BinderSelect;
        public AvfxNodeSelect<AvfxEmitter> EmitterSelect;
        public AvfxNodeSelect<AvfxEffector> EffectorSelect;

        private readonly List<IUiItem> Display;

        public AvfxTimelineItem( AvfxTimeline timeline, bool initNodeSelects ) {
            Timeline = timeline;

            Parsed = new() {
                Enabled,
                StartTime,
                EndTime,
                BinderIdx,
                EffectorIdx,
                EmitterIdx,
                Platform,
                ClipIdx
            };
            AvfxBase.RecurseAssigned( Parsed, false );

            if( initNodeSelects ) InitializeNodeSelects();

            Display = new() {
                Enabled,
                StartTime,
                EndTime,
                Platform
            };
        }

        public AvfxTimelineItem( AvfxTimeline timeline, bool initNodeSelects, byte[] data ) : this( timeline, initNodeSelects ) {
            using var buffer = new MemoryStream( data );
            using var reader = new BinaryReader( buffer );
            AvfxBase.ReadNested( reader, Parsed, data.Length );
        }

        public void InitializeNodeSelects() {
            BinderSelect = new AvfxNodeSelect<AvfxBinder>( Timeline, "目标绑定器", Timeline.NodeGroups.Binders, BinderIdx );
            EmitterSelect = new AvfxNodeSelect<AvfxEmitter>( Timeline, "目标发射器", Timeline.NodeGroups.Emitters, EmitterIdx );
            EffectorSelect = new AvfxNodeSelect<AvfxEffector>( Timeline, "目标效果器", Timeline.NodeGroups.Effectors, EffectorIdx );
        }

        public void Write( BinaryWriter writer ) => AvfxBase.WriteNested( writer, Parsed );

        public override void Draw() {
            using var _ = ImRaii.PushId( "Item" );
            DrawRename();

            BinderSelect.Draw();
            EmitterSelect.Draw();
            EffectorSelect.Draw();
            AvfxBase.DrawItems( Display );

            var clipAssigned = ClipIdx.IsAssigned();
            if( ImGui.Checkbox( "启用片段", ref clipAssigned ) ) CommandManager.Avfx.Add( new AvfxAssignCommand( ClipIdx, clipAssigned ) );
            ClipIdx.Draw();
        }

        public override string GetDefaultText() {
            if( EmitterIdx.Value != -1 ) return EmitterSelect.GetText();

            if( ClipIdx.IsAssigned() && ClipIdx.Value != -1 ) {
                if( ClipIdx.Value < Timeline.Clips.Count ) return Timeline.Clips[ClipIdx.Value].GetText();
                return $"Clip {ClipIdx.Value}";
            }

            return "[无]";
        }

        public override string GetWorkspaceId() => $"{Timeline.GetWorkspaceId()}/Item{GetIdx()}";

        public AvfxEmitter Emitter => EmitterSelect.Selected;

        public bool HasValue => EmitterIdx.Value >= 0 || ( ClipIdx.IsAssigned() && ClipIdx.Value >= 0 );
    }
}
