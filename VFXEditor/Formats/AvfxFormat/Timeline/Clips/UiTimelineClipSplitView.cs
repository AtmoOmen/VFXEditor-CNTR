using System.Collections.Generic;

namespace VfxEditor.AvfxFormat {
    public class UiTimelineClipSplitView : AvfxItemSplitView<AvfxTimelineClip> {
        public readonly AvfxTimeline Timeline;

        public UiTimelineClipSplitView( List<AvfxTimelineClip> items, AvfxTimeline timeline ) : base( "片段", items ) {
            Timeline = timeline;
        }

        public override void Disable( AvfxTimelineClip item ) { }

        public override void Enable( AvfxTimelineClip item ) { }

        public override AvfxTimelineClip CreateNewAvfx() => new( Timeline );
    }
}
