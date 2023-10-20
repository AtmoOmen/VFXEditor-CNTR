using VfxEditor.FileManager;
using VfxEditor.Interop.Havok;
using VfxEditor.Select.Sklb;
using VfxEditor.Utils;

namespace VfxEditor.SklbFormat {
    public class SklbManager : FileManager<SklbDocument, SklbFile, WorkspaceMetaBasic> {
        public SklbManager() : base( "骨骼编辑器", "Sklb" ) {
            SourceSelect = new SklbSelectDialog( "骨骼选择 [加载]", this, true );
            ReplaceSelect = new SklbSelectDialog( "骨骼选择 [替换]", this, false );
        }

        protected override SklbDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override SklbDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        public bool GetSimpleSklb( string path, out SimpleSklb skeleton, out bool replaced ) {
            replaced = false;
            skeleton = null;

            // Local
            if( System.IO.Path.IsPathRooted( path ) ) {
                if( System.IO.Path.Exists( path ) ) {
                    skeleton = SimpleSklb.LoadFromLocal( path );
                    return true;
                }
                return false;
            }

            // Game file
            foreach( var document in Documents ) {
                if( document.CurrentFile == null ) continue;
                if( document.ReplacePath.Equals( path ) ) {
                    replaced = true;
                    skeleton = SimpleSklb.LoadFromLocal( document.WriteLocation );
                    return true;
                }
            }

            if( Plugin.DataManager.FileExists( path ) ) {
                skeleton = Plugin.DataManager.GetFile<SimpleSklb>( path );
                return true;
            }

            return false;
        }
    }
}
