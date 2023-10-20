using System.IO;
using VfxEditor.Ui.Nodes;

namespace VfxEditor.AvfxFormat {
    public class UiEmitterView : UiNodeDropdownView<AvfxEmitter> {
        public UiEmitterView( AvfxFile file, NodeGroup<AvfxEmitter> group ) : base( file, group, "发射器", true, true, "default_emitter.vfxedit" ) { }

        public override void OnSelect( AvfxEmitter item ) { }

        public override AvfxEmitter Read( BinaryReader reader, int size ) {
            var item = new AvfxEmitter( File.NodeGroupSet );
            item.Read( reader, size );
            return item;
        }
    }
}