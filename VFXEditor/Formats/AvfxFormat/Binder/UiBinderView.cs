using System.IO;

namespace VfxEditor.AvfxFormat {
    public class UiBinderView : AvfxNodeDropdownView<AvfxBinder> {
        public UiBinderView( AvfxFile file, NodeGroup<AvfxBinder> group ) : base( file, group, "绑定器", true, true, "default_binder.vfxedit" ) { }

        public override void OnSelect( AvfxBinder item ) { }

        public override AvfxBinder Read( BinaryReader reader, int size ) {
            var item = new AvfxBinder(); // never has dependencies
            item.Read( reader, size );
            return item;
        }
    }
}