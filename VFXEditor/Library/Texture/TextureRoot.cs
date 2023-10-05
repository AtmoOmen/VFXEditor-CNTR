using Dalamud.Interface;
using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.Numerics;
using VfxEditor.Library.Texture;
using VfxEditor.Utils;

namespace VfxEditor.Library.Node {
    public class TextureRoot : LibraryFolder {
        private string AddPath = "";

        public TextureRoot( List<LibraryProps> items ) : base( null, "", "", items ) { }

        public override unsafe bool Draw( LibraryManager library, string searchInput ) {
            ImGui.InputTextWithHint( "##Add", "vfx/action/some_texture.atex", ref AddPath, 256 );
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 4, 4 ) ) )
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                ImGui.SameLine();
                if( ImGui.Button( FontAwesomeIcon.Plus.ToIconString() ) ) {
                    AddTexture( AddPath, AddPath );
                    library.Save();
                    AddPath = "";
                }

                ImGui.SameLine();
                if( ImGui.Button( FontAwesomeIcon.Save.ToIconString() ) ) {
                    FileDialogManager.SaveFileDialog( "选择保存位置", ".txt", "材质", "txt", ( bool ok, string res ) => {
                        if( !ok ) return;
                        library.ExportTextures( res );
                    } );
                }

                ImGui.SameLine();
                if( ImGui.Button( FontAwesomeIcon.Upload.ToIconString() ) ) {
                    FileDialogManager.OpenFileDialog( "选择文件", ".txt,.*", ( bool ok, string res ) => {
                        if( !ok ) return;
                        library.ImportTextures( res );
                    } );
                }
            }

            ImGui.Separator();

            using var child = ImRaii.Child( "子级", ImGui.GetContentRegionAvail(), false );

            if( Children.Count == 0 ) ImGui.TextDisabled( "无已保存的材质" );
            return base.Draw( library, searchInput );
        }

        public unsafe void AddTexture( string name, string path ) {
            if( string.IsNullOrEmpty( path ) || !path.Contains( '/' ) ) return;
            Children.Add( new TextureLeaf( this, name, UiUtils.RandomString( 12 ), path, *ImGui.GetStyleColorVec4( ImGuiCol.Header ) ) );
        }
    }
}
