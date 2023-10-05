using VfxEditor.FileManager;
using VfxEditor.Select.Skp;
using VfxEditor.Utils;

namespace VfxEditor.Formats.SkpFormat {
    public unsafe class SkpManager : FileManager<SkpDocument, SkpFile, WorkspaceMetaBasic> {
        public SkpManager() : base( "Skp Editor", "建模" ) {
            SourceSelect = new SkpSelectDialog( "建模选择 [加载]", this, true );
            ReplaceSelect = new SkpSelectDialog( "建模选择 [替换]", this, false );
        }

        protected override SkpDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override SkpDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
