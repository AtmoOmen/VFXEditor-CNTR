using VfxEditor.FileManager;
using VfxEditor.Select.Phyb;
using VfxEditor.Utils;

namespace VfxEditor.PhybFormat {
    public class PhybManager : FileManager<PhybDocument, PhybFile, WorkspaceMetaBasic> {
        public PhybManager() : base( "碰撞体编辑器", "碰撞" ) {
            SourceSelect = new PhybSelectDialog( "碰撞体选择 [加载]", this, true );
            ReplaceSelect = new PhybSelectDialog( "碰撞体选择 [替换]", this, false );
        }

        protected override PhybDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override PhybDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}