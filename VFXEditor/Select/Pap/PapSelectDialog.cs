using VfxEditor.PapFormat;
using VfxEditor.Select.Pap.Action;
using VfxEditor.Select.Pap.Emote;
using VfxEditor.Select.Pap.IdlePose;
using VfxEditor.Select.Pap.Job;
using VfxEditor.Select.Pap.Mount;
using VfxEditor.Select.Pap.Npc;
using VfxEditor.Select.Pap.Weapon;

namespace VfxEditor.Select.Pap {
    public class PapSelectDialog : SelectDialog {
        public PapSelectDialog( string id, PapManager manager, bool isSourceDialog ) : base( id, "pap", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new WeaponTab( this, "武器" ),
                new ActionTab( this, "技能" ),
                new NonPlayerActionTab( this, "非玩家对象动作" ),
                new EmoteTab( this, "表情" ),
                new NpcPapTab( this, "NPC" ),
                new MountTab( this, "坐骑" ),
                new CharacterPapTab( this, "角色" ),
                new JobTab( this, "职业" ),
            } );
        }
    }
}
