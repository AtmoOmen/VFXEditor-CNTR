using VfxEditor.PhybFormat;
using VfxEditor.Select.Shared.Skeleton;

namespace VfxEditor.Select.Phyb {
    public class PhybSelectDialog : SelectDialog {
        public PhybSelectDialog( string id, PhybManager manager, bool isSourceDialog ) : base( id, "phyb", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new SkeletonArmorTab( this, "Armor", "phy", "phyb" ),
                new SkeletonNpcTab( this, "NPC" , "phy", "phyb"),
                new SkeletonCharacterTab( this, "角色", "phy", "phyb" ),
                new SkeletonMountTab( this, "坐骑", "phy", "phyb")
            } );
        }
    }
}
