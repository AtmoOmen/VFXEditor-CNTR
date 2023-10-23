using VfxEditor.Select.Shared.Skeleton;
using VfxEditor.SklbFormat;

namespace VfxEditor.Select.Sklb {
    public class SklbSelectDialog : SelectDialog {
        public SklbSelectDialog( string id, SklbManager manager, bool isSourceDialog ) : base( id, "sklb", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new SkeletonArmorTab( this, "装备", "skl", "sklb" ),
                new SkeletonNpcTab( this, "NPC" , "skl", "sklb"),
                new SkeletonCharacterTab( this, "角色", "skl", "sklb" ),
                new SkeletonMountTab( this, "坐骑", "skl", "sklb")
            } );
        }
    }
}
