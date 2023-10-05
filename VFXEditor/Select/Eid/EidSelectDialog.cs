using VfxEditor.EidFormat;
using VfxEditor.Select.Shared.Skeleton;

namespace VfxEditor.Select.Eid {
    public class EidSelectDialog : SelectDialog {
        public EidSelectDialog( string id, EidManager manager, bool isSourceDialog ) : base( id, "eid", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new SkeletonNpcTab( this, "NPC", "eid", "eid"),
                new SkeletonCharacterTab( this, "角色", "eid", "eid", false ),
                new SkeletonMountTab( this, "坐骑", "eid", "eid"),
            } );
        }
    }
}
