using VfxEditor.FileManager;
using VfxEditor.Select.Uld;
using VfxEditor.Utils;

namespace VfxEditor.UldFormat {
    public unsafe class UldManager : FileManager<UldDocument, UldFile, WorkspaceMetaRenamed> {
        public UldManager() : base( "Uld Editor", "Uld" ) {
            SourceSelect = new UldSelectDialog( "选择界面 [加载]", this, true );
            ReplaceSelect = new UldSelectDialog( "选择界面 [替换]", this, false );
        }

        protected override UldDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override UldDocument GetWorkspaceDocument( WorkspaceMetaRenamed data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
