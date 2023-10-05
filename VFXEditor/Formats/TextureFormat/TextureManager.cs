using Dalamud.Logging;
using ImGuiFileDialog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.Data;
using VfxEditor.FileManager.Interfaces;
using VfxEditor.Formats.TextureFormat.Textures;
using VfxEditor.Formats.TextureFormat.Ui;
using VfxEditor.Select;
using VfxEditor.Ui;
using VfxEditor.Utils;

namespace VfxEditor.Formats.TextureFormat {
    public class TextureManager : GenericDialog, IFileManager {
        private int TEX_ID = 0;
        public string NewWriteLocation => Path.Combine( Plugin.Configuration.WriteLocation, $"TexTemp{TEX_ID++}.atex" ).Replace( '\\', '/' );

        private readonly List<TextureReplace> Textures = new();
        private readonly Dictionary<string, TexturePreview> Previews = new();
        private readonly TextureView View;
        private readonly ManagerConfiguration Configuration;

        public TextureManager() : base( "材质", false, 800, 500 ) {
            LoadLibrary();
            Configuration = Plugin.Configuration.GetManagerConfig( "Tex" );
            View = new( this, Textures );
        }

        public CopyManager GetCopyManager() => null;
        public CommandManager GetCommandManager() => null;
        public ManagerConfiguration GetConfig() => Configuration;
        public IEnumerable<IFileDocument> GetDocuments() => Textures;
        public string GetId() => "材质";

        public void ReplaceTexture( string importPath, string gamePath ) {
            var newReplace = new TextureReplace( gamePath, NewWriteLocation );
            newReplace.ImportFile( importPath );
            Textures.Add( newReplace );
        }

        public void RemoveReplace( TextureReplace replace ) {
            replace.Dispose();
            Textures.Remove( replace );
            View.ClearSelected();
        }

        public void Import( SelectResult result ) {
            FileDialogManager.OpenFileDialog( "选择文件", "Image files{.png,.tex,.atex,.dds},.*", ( bool ok, string res ) => {
                if( !ok ) return;
                try {
                    AddRecent( result );
                    ReplaceTexture( res, result.Path );
                }
                catch( Exception e ) {
                    PluginLog.Error( e, "无法导入数据" );
                }
            } );
        }

        public void AddRecent( SelectResult result ) => Plugin.Configuration.AddRecent( Configuration.RecentItems, result );

        public override void DrawBody() => View.Draw();

        // ====================

        public TextureDrawable GetTexture( string gamePath ) {
            gamePath = gamePath.Trim( '\0' );
            if( string.IsNullOrEmpty( gamePath ) || !gamePath.Contains( '/' ) ) return null;

            foreach( var texture in Textures ) {
                if( texture.GetReplacePath( gamePath, out var _ ) ) return texture;
            }

            if( Previews.TryGetValue( gamePath, out var preview ) ) return preview;

            if( !Dalamud.DataManager.FileExists( gamePath ) ) return null;

            try {
                var data = Dalamud.DataManager.GetFile<TextureDataFile>( gamePath );
                if( !data.ValidFormat ) {
                    PluginLog.Error( $"Invalid format: {data.Header.Format} {gamePath}" );
                    return null;
                }
                var newPreview = new TexturePreview( data, gamePath );
                Previews[gamePath] = newPreview;
                return newPreview;
            }
            catch( Exception e ) {
                PluginLog.Error( e, $"Could not find tex: {gamePath}" );
                return null;
            }
        }

        public bool GetReplacePath( string path, out string replacePath ) => IFileManager.GetReplacePath( this, path, out replacePath );

        public bool DoDebug( string path ) => path.Contains( ".atex" ) || path.Contains( ".tex" );

        // ===================

        public void WorkspaceImport( JObject meta, string loadLocation ) {
            var items = WorkspaceUtils.ReadFromMeta<WorkspaceMetaTex>( meta, "Tex" );
            if( items == null ) return;
            foreach( var item in items ) {
                var fullPath = WorkspaceUtils.ResolveWorkspacePath( item.RelativeLocation, Path.Combine( loadLocation, "Tex" ) );
                var newReplace = new TextureReplace( Plugin.TextureManager.NewWriteLocation, item );
                newReplace.ImportFile( fullPath );
                Textures.Add( newReplace );
            }
        }

        public void WorkspaceExport( Dictionary<string, string> meta, string saveLocation ) {
            var texRootPath = Path.Combine( saveLocation, "Tex" );
            Directory.CreateDirectory( texRootPath );

            var idx = 0;
            var texMeta = new List<WorkspaceMetaTex>();
            foreach( var texture in Textures ) {
                texMeta.Add( texture.WorkspaceExport( texRootPath, idx ) );
                idx++;
            }
            WorkspaceUtils.WriteToMeta( meta, texMeta.ToArray(), "Tex" );
        }

        // ================

        public void ToDefault() => Dispose();

        public void Dispose() {
            FreeLibrary();
            Textures.ForEach( x => x.Dispose() );
            Previews.Values.ToList().ForEach( x => x.Dispose() );
            Textures.Clear();
            Previews.Clear();
            TEX_ID = 0;
        }

        // =======================

        private static void LoadLibrary() {
            // Set paths manually since TexImpNet can be dumb sometimes
            // Using the 32-bit version in all cases because net6, I guess
            var runtimeRoot = Path.Combine( Plugin.RootLocation, "runtimes" );

            var freeImgLib = TeximpNet.Unmanaged.FreeImageLibrary.Instance;
            var _32bitPath = Path.Combine( runtimeRoot, "win-x64", "native", "FreeImage.dll" );
            var _64bitPath = Path.Combine( runtimeRoot, "win-x86", "native", "FreeImage.dll" );
            freeImgLib.Resolver.SetOverrideLibraryName32( _32bitPath );
            freeImgLib.Resolver.SetOverrideLibraryName64( _32bitPath );
            PluginLog.Log( $"FreeImage TeximpNet paths: {_32bitPath} / {_64bitPath}" );
            PluginLog.Log( $"FreeImage Default name: {freeImgLib.DefaultLibraryName} Library loaded: {freeImgLib.IsLibraryLoaded}" );
            freeImgLib.LoadLibrary();
            PluginLog.Log( $"FreeImage Library path: {freeImgLib.LibraryPath} Library loaded: {freeImgLib.IsLibraryLoaded}" );

            var nvtLib = TeximpNet.Unmanaged.NvTextureToolsLibrary.Instance;
            var nv_32bitPath = Path.Combine( runtimeRoot, "win-x64", "native", "nvtt.dll" );
            var nv_64bitPath = Path.Combine( runtimeRoot, "win-x86", "native", "nvtt.dll" );
            nvtLib.Resolver.SetOverrideLibraryName32( nv_32bitPath );
            nvtLib.Resolver.SetOverrideLibraryName64( nv_32bitPath );
            PluginLog.Log( $"NVT TeximpNet 路径: {nv_32bitPath} / {nv_64bitPath}" );
            PluginLog.Log( $"NVT 默认名称: {nvtLib.DefaultLibraryName} 已加载库: {nvtLib.IsLibraryLoaded}" );
            nvtLib.LoadLibrary();
            PluginLog.Log( $"NVT 库路径: {nvtLib.LibraryPath} 已加载库: {nvtLib.IsLibraryLoaded}" );
        }

        private static void FreeLibrary() {
            TeximpNet.Unmanaged.FreeImageLibrary.Instance.FreeLibrary();
            TeximpNet.Unmanaged.NvTextureToolsLibrary.Instance.FreeLibrary();
            PluginLog.Log( $"已加载 FreeImage 库: {TeximpNet.Unmanaged.FreeImageLibrary.Instance.IsLibraryLoaded}" );
            PluginLog.Log( $"已加载 NVTT 库: {TeximpNet.Unmanaged.NvTextureToolsLibrary.Instance.IsLibraryLoaded}" );
        }
    }
}
