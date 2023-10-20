using VfxEditor.EidFormat;
using VfxEditor.Select.Eid.Character;
using VfxEditor.Select.Eid.Mount;
using VfxEditor.Select.Eid.Npc;

namespace VfxEditor.Select.Eid {
    public class EidSelectDialog : SelectDialog {
        public EidSelectDialog( string id, EidManager manager, bool isSourceDialog ) : base( id, "eid", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new NpcEidTab( this, "NPC" ),
                new CharacterEidTab( this, "角色" ),
                new MountEidTab( this, "坐骑" ),
            } );
        }
    }
}
