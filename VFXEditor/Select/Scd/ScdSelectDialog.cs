using VfxEditor.ScdFormat;
using VfxEditor.Select.Scd.Action;
using VfxEditor.Select.Scd.Bgm;
using VfxEditor.Select.Scd.BgmQuest;
using VfxEditor.Select.Scd.Common;
using VfxEditor.Select.Scd.Instance;
using VfxEditor.Select.Scd.Mount;
using VfxEditor.Select.Scd.Orchestrion;
using VfxEditor.Select.Scd.Voice;
using VfxEditor.Select.Scd.Zone;

namespace VfxEditor.Select.Scd {
    public class ScdSelectDialog : SelectDialog {
        public ScdSelectDialog( string id, ScdManager manager, bool isSourceDialog ) : base( id, "scd", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new ActionTab( this, "Actions" ),
                new MountScdTab( this, "坐骑" ),
                new OrchestrionTab( this, "管弦乐琴音乐" ),
                new ZoneTab( this, "区域" ),
                new BgmTab( this, "BGM" ),
                new BgmQuestTab( this, "任务 BGM" ),
                new InstanceTab( this, "副本" ),
                new VoiceTab( this, "声音" ),
                new CommonTab( this, "通常" ),
            } );
        }
    }
}
