using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Nodes;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.AvfxFormat {
    public class AvfxTimeline : AvfxNode {
        public const string NAME = "TmLn";

        public readonly AvfxInt LoopStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopEnd = new( "循环结束", "LpEd" );
        public readonly AvfxInt BinderIdx = new( "绑定器索引", "BnNo" );
        public readonly AvfxInt TimelineCount = new( "物体数", "TICn" );
        public readonly AvfxInt ClipCount = new( "片段数", "CpCn" );

        private readonly List<AvfxBase> Parsed;

        public readonly List<AvfxTimelineClip> Clips = new();
        public readonly List<AvfxTimelineItem> Items = new();

        public readonly AvfxNodeGroupSet NodeGroups;

        public readonly UiNodeSelect<AvfxBinder> BinderSelect;

        public readonly UiTimelineClipSplitView ClipSplit;
        public readonly UiTimelineItemSequencer ItemSplit;
        private readonly List<IUiItem> Display;

        public AvfxTimeline( AvfxNodeGroupSet groupSet ) : base( NAME, AvfxNodeGroupSet.TimelineColor ) {
            NodeGroups = groupSet;

            Parsed = new List<AvfxBase> {
                LoopStart,
                LoopEnd,
                BinderIdx,
                TimelineCount,
                ClipCount
            };

            Display = new() {
                LoopStart,
                LoopEnd
            };

            BinderSelect = new( this, "选择绑定器", groupSet.Binders, BinderIdx );

            ClipSplit = new( Clips, this );
            ItemSplit = new( Items, this );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );

            AvfxTimelineItemContainer lastItem = null;

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Item" ) {
                    lastItem = new AvfxTimelineItemContainer( this );
                    lastItem.Read( _reader, _size );
                }
                else if( _name == "Clip" ) {
                    var clip = new AvfxTimelineClip( this );
                    clip.Read( _reader, _size );
                    Clips.Add( clip );
                }
            }, size );

            if( lastItem != null ) {
                Items.AddRange( lastItem.Items );
                Items.ForEach( x => x.InitializeNodeSelects() );
            }

            ClipSplit.UpdateIdx();
            ItemSplit.UpdateIdx();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        protected override void WriteContents( BinaryWriter writer ) {
            TimelineCount.SetValue( Items.Count );
            ClipCount.SetValue( Clips.Count );
            WriteNested( writer, Parsed );

            // Item
            for( var i = 0; i < Items.Count; i++ ) {
                var item = new AvfxTimelineItemContainer( this );
                item.Items.AddRange( Items.GetRange( 0, i + 1 ) );
                item.Write( writer );
            }

            foreach( var clip in Clips ) clip.Write( writer );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Timeline" );
            DrawRename();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawParameters();

            using( var tab = ImRaii.TabItem( "物体" ) ) {
                if( tab ) ItemSplit.Draw();
            }

            using( var tab = ImRaii.TabItem( "片段" ) ) {
                if( tab ) ClipSplit.Draw();
            }
        }

        private void DrawParameters() {
            using var tabItem = ImRaii.TabItem( "参数" );
            if( !tabItem ) return;

            using var child = ImRaii.Child( "子级" );
            BinderSelect.Draw();
            DrawItems( Display );
        }

        public override void GetChildrenRename( Dictionary<string, string> renameDict ) {
            Items.ForEach( item => IWorkspaceUiItem.GetRenamingMap( item, renameDict ) );
            Clips.ForEach( item => IWorkspaceUiItem.GetRenamingMap( item, renameDict ) );
        }

        public override void SetChildrenRename( Dictionary<string, string> renameDict ) {
            Items.ForEach( item => IWorkspaceUiItem.ReadRenamingMap( item, renameDict ) );
            Clips.ForEach( item => IWorkspaceUiItem.ReadRenamingMap( item, renameDict ) );
        }

        public override string GetDefaultText() => $"时间线 {GetIdx()}";

        public override string GetWorkspaceId() => $"Tmln{GetIdx()}";
    }
}
