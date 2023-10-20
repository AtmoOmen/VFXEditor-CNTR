using System.IO;
using VfxEditor.Ui.Nodes;

namespace VfxEditor.AvfxFormat {
    public class UiModelView : UiNodeSplitView<AvfxModel> {
        public UiModelView( AvfxFile file, NodeGroup<AvfxModel> group ) : base( file, group, "模型", true, true, "default_model.vfxedit2" ) { }

        public override void OnSelect( AvfxModel item ) => item.OnSelect();

        public override AvfxModel Read( BinaryReader reader, int size ) {
            var item = new AvfxModel();
            item.Read( reader, size );
            return item;
        }
    }
}