using VfxEditor.FileManager;
using VfxEditor.Select.Shpk;
using VfxEditor.Utils;

namespace VfxEditor.Formats.ShpkFormat {
    public unsafe class ShpkManager : FileManager<ShpkDocument, ShpkFile, WorkspaceMetaBasic> {
        public ShpkManager() : base( "着色器编辑器", "着色器" ) {
            SourceSelect = new ShpkSelectDialog( "着色器选择 [加载]", this, true );
            ReplaceSelect = new ShpkSelectDialog( "着色器选择 [替换]", this, false );
        }

        protected override ShpkDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override ShpkDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
