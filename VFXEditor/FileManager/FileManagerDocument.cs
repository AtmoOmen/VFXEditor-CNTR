﻿using Dalamud.Interface;
using Dalamud.Logging;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VfxEditor.FileManager.Interfaces;
using VfxEditor.Select;
using VfxEditor.Ui.Export;
using VfxEditor.Utils;

namespace VfxEditor.FileManager {
    public abstract class FileManagerDocument<R, S> : IFileDocument where R : FileManagerFile {
        public R CurrentFile { get; protected set; }
        protected VerifiedStatus Verified => CurrentFile == null ? VerifiedStatus.UNKNOWN : CurrentFile.Verified;

        public string DisplayName => string.IsNullOrEmpty( Name ) ? ReplaceDisplay : Name;
        protected string Name = "";

        protected SelectResult Source;
        public string SourceDisplay => Source == null ? "[无]" : Source.DisplayString;
        public string SourcePath => Source == null ? "" : Source.Path;

        protected SelectResult Replace;
        public string ReplaceDisplay => Replace == null ? "[无]" : Replace.DisplayString;
        public string ReplacePath => ( Disabled || Replace == null ) ? "" : Replace.Path;
        protected bool Disabled = false;

        public string WriteLocation { get; protected set; }

        public abstract string Id { get; }
        public abstract string Extension { get; }

        protected readonly FileManagerBase Manager;

        public bool Unsaved = false;
        protected DateTime LastUpdate = DateTime.Now;

        public FileManagerDocument( FileManagerBase manager, string writeLocation ) {
            Manager = manager;
            WriteLocation = writeLocation;
        }

        public bool GetReplacePath( string path, out string replacePath ) {
            replacePath = null;
            if( CurrentFile == null ) return false;

            replacePath = ReplacePath.ToLower().Equals( path.ToLower() ) ? WriteLocation : null;
            return !string.IsNullOrEmpty( replacePath );
        }

        protected abstract R FileFromReader( BinaryReader reader );

        protected void LoadLocal( string localPath ) {
            if( !File.Exists( localPath ) ) {
                PluginLog.Error( $"本地文件: [{localPath}] 不存在" );
                return;
            }

            if( !localPath.EndsWith( $".{Extension}" ) ) {
                PluginLog.Error( $"{localPath} 文件类型错误" );
                return;
            }

            try {
                using var reader = new BinaryReader( File.Open( localPath, FileMode.Open ) );
                CurrentFile?.Dispose();
                CurrentFile = FileFromReader( reader );
                UiUtils.OkNotification( $"{Id} 文件已加载" );
            }
            catch( Exception e ) {
                PluginLog.Error( e, "读取文件时发生错误", e );
                UiUtils.ErrorNotification( "读取文件时发生错误" );
            }
        }

        protected void LoadGame( string gamePath ) {
            if( !Plugin.DataManager.FileExists( gamePath ) ) {
                PluginLog.Error( $"游戏文件: [{gamePath}] 不存在" );
                return;
            }

            if( !gamePath.EndsWith( $".{Extension}" ) ) {
                PluginLog.Error( $"{gamePath} 文件类型错误" );
                return;
            }

            try {
                var file = Plugin.DataManager.GetFile( gamePath );
                using var ms = new MemoryStream( file.Data );
                using var reader = new BinaryReader( ms );
                CurrentFile?.Dispose();
                CurrentFile = FileFromReader( reader );
                UiUtils.OkNotification( $"{Id} 文件已加载" );
            }
            catch( Exception e ) {
                PluginLog.Error( e, "读取文件时发生错误" );
                UiUtils.ErrorNotification( "读取文件时发生错误" );
            }
        }

        // =================

        public void SetSource( SelectResult result ) {
            if( result == null ) return;
            Source = result;

            if( result.Type == SelectResultType.Local ) LoadLocal( result.Path );
            else LoadGame( result.Path );

            if( CurrentFile != null ) {
                WriteFile( WriteLocation );
            }
        }

        protected void RemoveSource() {
            CurrentFile?.Dispose();
            CurrentFile = null;
            Source = null;
            Unsaved = false;
        }

        public void SetReplace( SelectResult result ) { Replace = result; }

        protected void RemoveReplace() { Replace = null; }

        // =====================

        protected void WriteFile( string path ) {
            if( CurrentFile == null ) return;
            if( Plugin.Configuration?.LogDebug == true ) PluginLog.Log( "已将 {1} 个文件写入 {0}", path, Id );
            File.WriteAllBytes( path, CurrentFile.ToBytes() );
        }

        protected void ExportRaw() => UiUtils.WriteBytesDialog( "." + Extension, CurrentFile.ToBytes(), Extension );

        protected void Reload( List<string> papIds = null ) {
            if( CurrentFile == null || ReplacePath.Contains( ".sklb" ) ) return;
            Plugin.ResourceLoader.ReloadPath( ReplacePath, WriteLocation, papIds );
        }

        public void Update() {
            if( ( DateTime.Now - LastUpdate ).TotalSeconds <= 0.2 ) return;
            LastUpdate = DateTime.Now;
            Unsaved = false;

            if( Plugin.Configuration.UpdateWriteLocation ) {
                var newWriteLocation = Manager.NewWriteLocation;
                CurrentFile?.Update();
                WriteFile( newWriteLocation );
                WriteLocation = newWriteLocation;
                Reload( GetPapIds() );
                Plugin.ResourceLoader.ReRender();
            }
            else {
                WriteFile( WriteLocation );
                Reload( GetPapIds() );
                Plugin.ResourceLoader.ReRender();
            }
        }

        protected virtual List<string> GetPapIds() => null;

        // =======================

        protected void LoadWorkspace( string localPath, string relativeLocation, string name, SelectResult source, SelectResult replace, bool disabled ) {
            Name = name ?? "";
            Source = source;
            Replace = replace;
            Disabled = disabled;
            LoadLocal( WorkspaceUtils.ResolveWorkspacePath( relativeLocation, localPath ) );
            if( CurrentFile != null ) CurrentFile.Verified = VerifiedStatus.WORKSPACE;
            WriteFile( WriteLocation );
        }

        public string GetExportSource() => SourceDisplay;

        public string GetExportReplace() => DisplayName;

        public bool CanExport() => CurrentFile != null && !string.IsNullOrEmpty( ReplacePath );

        public void PenumbraExport( string modFolder, Dictionary<string, string> filesOut ) {
            var path = ReplacePath;
            if( string.IsNullOrEmpty( path ) || CurrentFile == null ) return;
            var data = CurrentFile.ToBytes();

            PenumbraUtils.WriteBytes( data, modFolder, path, filesOut );
        }

        public void TextoolsExport( BinaryWriter writer, List<TTMPL_Simple> simplePartsOut, ref int modOffset ) {
            var path = ReplacePath;
            if( string.IsNullOrEmpty( path ) || CurrentFile == null ) return;
            var modData = TexToolsUtils.CreateType2Data( CurrentFile.ToBytes() );
            simplePartsOut.Add( TexToolsUtils.CreateModResource( path, modOffset, modData.Length ) );
            writer.Write( modData );
            modOffset += modData.Length;
        }

        public abstract S GetWorkspaceMeta( string newPath );

        public void WorkspaceExport( List<S> tmbMeta, string rootPath, string newPath ) {
            if( CurrentFile != null ) {
                var newFullPath = Path.Combine( rootPath, newPath );
                File.WriteAllBytes( newFullPath, CurrentFile.ToBytes() );
                tmbMeta.Add( GetWorkspaceMeta( newPath ) );
            }
        }

        // ====== DRAWING ==========

        public virtual void CheckKeybinds() {
            if( Plugin.Configuration.CopyKeybind.KeyPressed() ) Manager.GetCopyManager()?.Copy();
            if( Plugin.Configuration.PasteKeybind.KeyPressed() ) Manager.GetCopyManager()?.Paste();
            if( Plugin.Configuration.UndoKeybind.KeyPressed() ) Manager.GetCommandManager()?.Undo();
            if( Plugin.Configuration.RedoKeybind.KeyPressed() ) Manager.GetCommandManager()?.Redo();
        }

        public void Draw() {
            if( Plugin.Configuration.WriteLocationError ) {
                ImGui.TextWrapped( $"VFXEditor 没有 {Plugin.Configuration.WriteLocation} 的访问权限. 请到 [文件 > 设置] 更改路径并重启游戏" );
                return;
            }

            var searchWidth = ImGui.GetContentRegionAvail().X - 160 - 125;

            using( var style = ImRaii.PushStyle( ImGuiStyleVar.WindowPadding, new Vector2( 0 ) ) )
            using( var _ = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 0 ) ) ) {
                ImGui.Columns( 3, "列", false );
                ImGui.SetColumnWidth( 0, 160 );
            }
            DrawInputTextColumn();

            using( var _ = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 0 ) ) ) {
                ImGui.NextColumn();
                ImGui.SetColumnWidth( 1, searchWidth );
            }
            DrawSearchBarsColumn();

            using( var _ = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 0 ) ) ) {
                ImGui.NextColumn();
                ImGui.SetColumnWidth( 2, 126 );
            }
            DrawExtraColumn();

            ImGui.Columns( 1 );

            DrawBody();
        }

        protected virtual void DrawInputTextColumn() {
            var pos = ImGui.GetCursorScreenPos() + new Vector2( 5, 0 );
            var height = ImGui.GetFrameHeight();
            var spacing = ImGui.GetStyle().ItemSpacing.Y;

            var radius = 5f;
            var width = 15f;
            var segmentResolution = 10;
            var thickness = 2;

            var arrowHeight = 8;
            var arrowWidth = 8;

            var drawList = ImGui.GetWindowDrawList();
            var topLeft = pos + new Vector2( 0, height * 0.5f );
            var topRight = topLeft + new Vector2( width, 0 );
            var bottomRight = pos + new Vector2( width, height * 1.5f + spacing - 1 );
            var bottomLeft = new Vector2( topLeft.X, bottomRight.Y );

            var mousePos = ImGui.GetMousePos();
            var hovered = ImGui.IsWindowFocused( ImGuiFocusedFlags.RootWindow ) && UiUtils.Contains( topLeft - new Vector2( 5, 5 ), bottomRight + new Vector2( 5, 5 ), mousePos );

            var color = hovered ?
                ImGui.ColorConvertFloat4ToU32( UiUtils.YELLOW_COLOR ) :
                ( Disabled ?
                    ImGui.ColorConvertFloat4ToU32( UiUtils.RED_COLOR ) :
                    ImGui.GetColorU32( ImGuiCol.TextDisabled )
               );

            if( hovered && ImGui.IsMouseClicked( ImGuiMouseButton.Left ) ) Disabled = !Disabled;

            var topLeftCurveCenter = new Vector2( topLeft.X + radius, topLeft.Y + radius );
            var bottomLeftCurveCenter = new Vector2( bottomLeft.X + radius, bottomLeft.Y - radius );

            drawList.PathArcTo( topLeftCurveCenter, radius, DegreesToRadians( 180 ), DegreesToRadians( 270 ), segmentResolution );
            drawList.PathStroke( color, ImDrawFlags.None, thickness );

            drawList.PathArcTo( bottomLeftCurveCenter, radius, DegreesToRadians( 90 ), DegreesToRadians( 180 ), segmentResolution );
            drawList.PathStroke( color, ImDrawFlags.None, thickness );

            drawList.AddLine( topLeft + new Vector2( -0.5f, radius - 0.5f ), bottomLeft + new Vector2( -0.5f, -radius + 0.5f ), color, thickness );
            drawList.AddLine( topLeft + new Vector2( radius - 0.5f, -0.5f ), topRight + new Vector2( 0, -0.5f ), color, thickness );
            drawList.AddLine( bottomLeft + new Vector2( radius - 0.5f, -0.5f ), bottomRight + new Vector2( -4, -0.5f ), color, thickness );

            if( Disabled ) {
                var crossCenter = bottomRight + new Vector2( -4, 0 );
                var crossHeight = arrowHeight / 2;

                drawList.AddLine( crossCenter + new Vector2( crossHeight, crossHeight ), crossCenter + new Vector2( -crossHeight, -crossHeight ), color, thickness );
                drawList.AddLine( crossCenter + new Vector2( -crossHeight, crossHeight ), crossCenter + new Vector2( crossHeight, -crossHeight ), color, thickness );
            }
            else {
                drawList.AddTriangleFilled( bottomRight, bottomRight + new Vector2( -arrowWidth, arrowHeight / 2 ), bottomRight + new Vector2( -arrowWidth, -arrowHeight / 2 ), color );
            }

            if( hovered ) UiUtils.Tooltip( "切换替换模式", true );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            ImGui.SetCursorPosX( ImGui.GetCursorPosX() + 25 );
            ImGui.Text( $"已载入的 {Id}" );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            ImGui.SetCursorPosX( ImGui.GetCursorPosX() + 25 );
            ImGui.Text( $"被替换的 {Id}" );
        }

        private static float DegreesToRadians( float degrees ) => MathF.PI / 180 * degrees;

        protected void DrawSearchBarsColumn() {
            var timesWidth = UiUtils.GetPaddedIconSize( FontAwesomeIcon.Times );
            var searchWidth = UiUtils.GetPaddedIconSize( FontAwesomeIcon.Search );
            // 3 * 2 for spacing, 25 for some more padding
            var inputWidth = ImGui.GetColumnWidth() - timesWidth - searchWidth - ( 3 * 2 ) - 20;

            DisplaySourceBar( inputWidth );
            DisplayReplaceBar( inputWidth );
        }

        protected virtual void DrawExtraColumn() { }

        protected void DisplaySourceBar( float inputSize ) {
            using var _ = ImRaii.PushId( "Source" );
            using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 3, 4 ) );
            var sourceString = Source == null ? "" : Source.DisplayString;

            // Remove
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( UiUtils.TransparentButton( FontAwesomeIcon.Times.ToIconString(), UiUtils.RED_COLOR ) ) RemoveSource();
            }

            // Input
            ImGui.SameLine();
            ImGui.SetNextItemWidth( inputSize );
            ImGui.InputTextWithHint( "", "[无]", ref sourceString, 255, ImGuiInputTextFlags.ReadOnly );

            // Search
            ImGui.SameLine();

            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Search.ToIconString() ) ) Manager.ShowSource();
            }
        }

        protected void DisplayReplaceBar( float inputSize ) {
            using var _ = ImRaii.PushId( "Replace" );
            using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 3, 4 ) );
            var previewString = Replace == null ? "" : Replace.DisplayString;

            // Remove
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( UiUtils.TransparentButton( FontAwesomeIcon.Times.ToIconString(), UiUtils.RED_COLOR ) ) RemoveReplace();
            }

            // Input
            ImGui.SameLine();
            ImGui.SetNextItemWidth( inputSize );
            ImGui.InputTextWithHint( "", "[无]", ref previewString, 255, ImGuiInputTextFlags.ReadOnly );
            if( Replace != null && ImGui.IsItemClicked( ImGuiMouseButton.Right ) ) ImGui.OpenPopup( "CopyPopup" );

            if( Replace != null && ImGui.BeginPopup( "CopyPopup" ) ) {
                ImGui.Text( Replace.Path );
                ImGui.SameLine();
                ImGui.SetCursorPosX( ImGui.GetCursorPosX() + 2 );
                if( ImGui.SmallButton( "复制" ) ) ImGui.SetClipboardText( Replace.Path );
                ImGui.EndPopup();
            }

            // Search
            ImGui.SameLine();

            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Search.ToIconString() ) ) Manager.ShowReplace();
            }
        }

        protected void DisplayFileControls() {
            if( UiUtils.OkButton( "刷新" ) ) Update();

            ImGui.SameLine();
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Download.ToIconString() ) ) ExportRaw();
            }
            UiUtils.Tooltip( "导出为原始文件。\n要导出为 Textools/Penumbra 模组，请使用\"模组导出\" 菜单项" );

            ImGui.SameLine();
            UiUtils.ShowVerifiedStatus( Verified );

            var warnings = GetWarningText();
            if( !string.IsNullOrEmpty( warnings ) ) {
                ImGui.SameLine();
                using var _ = ImRaii.PushColor( ImGuiCol.Text, UiUtils.RED_COLOR );
                using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    ImGui.SameLine();
                    ImGui.Text( FontAwesomeIcon.InfoCircle.ToIconString() );
                }
                ImGui.SameLine();
                ImGui.Text( warnings );
            }
        }

        protected virtual string GetWarningText() => "";

        protected virtual void DrawBody() {
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            ImGui.Separator();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            if( CurrentFile == null ) DisplayBeginHelpText();
            else {
                DisplayFileControls();
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
                using var _ = ImRaii.PushId( "Body" );
                CurrentFile.Draw();
            }
        }

        public void DrawRename() {
            Name ??= "";
            using var _ = ImRaii.PushId( "Rename" );
            ImGui.InputTextWithHint( "", ReplaceDisplay, ref Name, 64, ImGuiInputTextFlags.AutoSelectAll );
        }

        public virtual void Dispose() {
            Plugin.CleanupExport( this );
            CurrentFile?.Dispose();
            CurrentFile = null;
            File.Delete( WriteLocation );
        }

        // ========================

        protected static void DisplayBeginHelpText() {
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 15 );

            var availWidth = ImGui.GetContentRegionMax().X;
            var width = availWidth > 500 ? 500 : availWidth; // cap out at 300
            ImGui.SetCursorPosX( ImGui.GetCursorPosX() + ( availWidth - width ) / 2 );
            using var child = ImRaii.Child( "HelpTextChild", new Vector2( width, -1 ) );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 30 );

            var buttonWidth = ImGui.GetContentRegionMax().X - ImGui.GetStyle().FramePadding.X * 2;

            ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.41764705882f, 0.41764705882f, 0.41764705882f, 1 ) );
            if( ImGui.Button( "Wiki + 指南(英文)", new Vector2( buttonWidth, 0 ) ) ) {
                UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki" );
            }
            ImGui.PopStyleColor();

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.21764705882f, 0.21764705882f, 0.21764705882f, 1 ) );
            if( ImGui.Button( "GitHub", new Vector2( buttonWidth, 0 ) ) ) {
                UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor" );
            }
            ImGui.PopStyleColor();

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.21764705882f, 0.21764705882f, 0.21764705882f, 1 ) );
            if( ImGui.Button( "提交 Issue", new Vector2( buttonWidth, 0 ) ) ) {
                UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/issues" );
            }
            ImGui.PopStyleColor();

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.33725490196f, 0.38431372549f, 0.96470588235f, 1 ) );
            if( ImGui.Button( "XIVLauncher Discord", new Vector2( buttonWidth, 0 ) ) ) {
                UiUtils.OpenUrl( "https://discord.gg/3NMcUV5" );
            }
            ImGui.PopStyleColor();
        }

        private static readonly string Text = "请 不要 修改移动类技能 (冲刺、后跳等)。尝试修改 .tmb 或 .pap 文件前请先阅读指南";

        protected static void DisplayAnimationWarning() {
            using var color = ImRaii.PushColor( ImGuiCol.Border, new Vector4( 1, 0, 0, 0.3f ) );
            color.Push( ImGuiCol.ChildBg, new Vector4( 1, 0, 0, 0.1f ) );

            var style = ImGui.GetStyle();
            var textSize = ImGui.CalcTextSize( Text, ImGui.GetContentRegionMax().X - style.WindowPadding.X * 2 - 8 );

            using var child = ImRaii.Child( "AnimationWarningChild", new Vector2( -1,
                textSize.Y +
                style.WindowPadding.Y * 2 +
                style.ItemSpacing.Y +
                ImGui.GetTextLineHeightWithSpacing()
            ), true, ImGuiWindowFlags.NoScrollbar );

            ImGui.TextWrapped( Text );
            if( ImGui.SmallButton( "指南##Pap" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki" );
        }
    }
}
