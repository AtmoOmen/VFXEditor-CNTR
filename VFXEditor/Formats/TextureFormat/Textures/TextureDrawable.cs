using Dalamud.Logging;
using ImGuiFileDialog;
using ImGuiNET;
using System;

namespace VfxEditor.Formats.TextureFormat.Textures {
    public abstract class TextureDrawable {
        protected string GamePath = "";
        protected string GameExtension => GamePath.Split( '.' )[^1].Trim( '\0' );

        public TextureDrawable( string gamePath ) {
            GamePath = gamePath.Trim( '\0' );
        }

        protected abstract TextureDataFile GetRawData();

        protected abstract void OnReplace( string importPath );

        public abstract void DrawImage();
        public abstract void DrawImage( uint u, uint v, uint w, uint h );

        protected abstract void DrawControls();

        public void Draw() {
            DrawImage();
            DrawControls();
        }

        public void Draw( uint u, uint v, uint w, uint h ) {
            DrawImage( u, v, w, h );
            DrawControls();
        }

        protected void DrawExportReplaceButtons() {
            if( ImGui.Button( "导出" ) ) ImGui.OpenPopup( "TexExport" );

            ImGui.SameLine();
            if( ImGui.Button( "替换" ) ) ImportDialog();

            if( ImGui.BeginPopup( "TexExport" ) ) {
                if( ImGui.Selectable( ".png" ) ) GetRawData()?.SavePngDialog();
                if( ImGui.Selectable( ".dds" ) ) GetRawData()?.SaveDdsDialog();
                if( ImGui.Selectable( $".{GameExtension}" ) ) GetRawData()?.SaveTexDialog( GameExtension );
                ImGui.EndPopup();
            }
        }

        protected void ImportDialog() {
            FileDialogManager.OpenFileDialog( "选择文件", "图片文件{.png,." + GameExtension + ",.dds},.*", ( bool ok, string res ) => {
                if( !ok ) return;
                try {
                    OnReplace( res );
                }
                catch( Exception e ) {
                    PluginLog.Error( e, "无法导入数据" );
                }
            } );
        }
    }
}
