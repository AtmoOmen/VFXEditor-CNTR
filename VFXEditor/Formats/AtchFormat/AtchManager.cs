using VfxEditor.FileManager;
using VfxEditor.Select.Atch;
using VfxEditor.Utils;

namespace VfxEditor.Formats.AtchFormat {
    public unsafe class AtchManager : FileManager<AtchDocument, AtchFile, WorkspaceMetaBasic> {
        public AtchManager() : base( "Atch Editor", "Atch" ) {
            SourceSelect = new AtchSelectDialog( "武器状态选择 [选择]", this, true );
            ReplaceSelect = new AtchSelectDialog( "武器状态选择 [替换]", this, false );
        }

        protected override AtchDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override AtchDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );
    }
}
