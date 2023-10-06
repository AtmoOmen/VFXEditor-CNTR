using VfxEditor.FileManager;
using VfxEditor.Select.Pap;
using VfxEditor.Utils;

namespace VfxEditor.PapFormat {
    public class PapManager : FileManager<PapDocument, PapFile, WorkspaceMetaBasic> {
        public PapManager() : base( "动画编辑器", "Pap" ) {
            SourceSelect = new PapSelectDialog( "选择动画 [加载]", this, true );
            ReplaceSelect = new PapSelectDialog( "选择动画 [替换]", this, false );
        }

        protected override PapDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override PapDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
