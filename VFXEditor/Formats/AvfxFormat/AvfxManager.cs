using ImGuiNET;
using VfxEditor.FileManager;
using VfxEditor.Select.Vfx;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat {
    public class AvfxManager : FileManager<AvfxDocument, AvfxFile, WorkspaceMetaRenamed> {
        public AvfxManager() : base( "VFXEditor", "Vfx", "avfx", "Docs", "VFX" ) {
            SourceSelect = new VfxSelectDialog( "选择文件 [加载]", this, true );
            ReplaceSelect = new VfxSelectDialog( "选择文件 [替换]", this, false );
        }

        protected override AvfxDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override AvfxDocument GetWorkspaceDocument( WorkspaceMetaRenamed data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        protected override void DrawEditMenuExtra() {
            if( ImGui.BeginMenu( "模板" ) ) {
                if( ImGui.MenuItem( "空白" ) ) ActiveDocument?.OpenTemplate( "default_vfx.avfx" );
                if( ImGui.MenuItem( "武器" ) ) ActiveDocument?.OpenTemplate( "default_weapon.avfx" );
                ImGui.EndMenu();
            }

            if( CurrentFile == null ) return;
            if( ImGui.MenuItem( "清除" ) ) CurrentFile.Cleanup();
        }

        public void Import( string path ) => ActiveDocument.Import( path );

        public void ShowExportDialog( AvfxNode node ) => ActiveDocument.ShowExportDialog( node );
    }
}
