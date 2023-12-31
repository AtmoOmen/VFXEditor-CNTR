﻿using System.IO;
using VfxEditor.FileManager;
using VfxEditor.Select.Scd;
using VfxEditor.Utils;

namespace VfxEditor.ScdFormat {
    public class ScdManager : FileManager<ScdDocument, ScdFile, WorkspaceMetaBasic> {
        public static string ConvertWav => Path.Combine( Plugin.Configuration.WriteLocation, $"temp_out.wav" ).Replace( '\\', '/' );
        public static string ConvertOgg => Path.Combine( Plugin.Configuration.WriteLocation, $"temp_out.ogg" ).Replace( '\\', '/' );

        public ScdManager() : base( "Scd Editor", "Scd" ) {
            SourceSelect = new ScdSelectDialog( "选择音频文件 [加载]", this, true );
            ReplaceSelect = new ScdSelectDialog( "选择音频文件 [替换]", this, false );
        }

        protected override ScdDocument GetNewDocument() => new( this, NewWriteLocation );

        protected override ScdDocument GetWorkspaceDocument( WorkspaceMetaBasic data, string localPath ) => new( this, NewWriteLocation, localPath, data );

        public override void Dispose() {
            base.Dispose();
            CurrentFile?.Dispose();
            ScdUtils.Cleanup();
        }
    }
}
