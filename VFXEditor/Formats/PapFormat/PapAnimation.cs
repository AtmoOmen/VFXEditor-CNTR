using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System.IO;
using System.Numerics;
using VfxEditor.Data;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;
using VfxEditor.TmbFormat;
using VfxEditor.Utils;

namespace VfxEditor.PapFormat {
    public class PapAnimation {
        public readonly PapFile File;

        public short HavokIndex = 0;
        public readonly string HkxTempLocation;

        private readonly ParsedPaddedString Name = new( "名称", "cbbm_replace_this", 32, 0x00 );
        private readonly ParsedShort Type = new( "类型" );
        private readonly ParsedBool Face = new( "面部动画" );
        public TmbFile Tmb;

        public PapAnimation( PapFile file, string hkxPath ) {
            File = file;
            HkxTempLocation = hkxPath;
        }

        public PapAnimation( PapFile file, BinaryReader reader, string hkxPath ) {
            File = file;
            HkxTempLocation = hkxPath;

            Name.Read( reader );
            Type.Read( reader );
            HavokIndex = reader.ReadInt16();
            Face.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Name.Write( writer );
            Type.Write( writer );
            writer.Write( HavokIndex );
            Face.Write( writer );
        }

        public void ReadTmb( BinaryReader reader, CommandManager manager ) {
            Tmb = new TmbFile( reader, manager, false );
        }

        public void ReadTmb( string path, CommandManager manager ) {
            Tmb = TmbFile.FromPapEmbedded( path, manager );
        }

        public byte[] GetTmbBytes() => Tmb.ToBytes();

        public string GetName() => Name.Value;

        public void Draw() {
            SheetData.InitMotionTimelines();
            if( !string.IsNullOrEmpty( Name.Value ) && SheetData.MotionTimelines.TryGetValue( Name.Value, out var motionData ) ) {
                ImGui.TextDisabled( $"循环: [{motionData.Loop}] 嘴部: [{motionData.Lip}] 眼部: [{motionData.Blink}]" );

                ImGui.SameLine();
                UiUtils.HelpMarker( "这些值硬编码于游戏的 MotionTimeline 表格中, 并且基于特定的动画名称" );
            }

            Name.Draw( CommandManager.Pap );
            Type.Draw( CommandManager.Pap );
            Face.Draw( CommandManager.Pap );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            ImGui.TextDisabled( $"此动画的 Havok 索引为: {HavokIndex}" );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            using var tabBar = ImRaii.TabBar( "动画栏" );
            if( !tabBar ) return;

            DrawTmb();
            DrawHavok();
            DrawMotion();
        }

        private void DrawTmb() {
            using var tabItem = ImRaii.TabItem( "TMB" );
            if( !tabItem ) return;

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 4, 4 ) ) ) {
                if( ImGui.Button( "导出" ) ) UiUtils.WriteBytesDialog( ".tmb", Tmb.ToBytes(), "tmb" );

                ImGui.SameLine();
                if( ImGui.Button( "替换" ) ) {
                    FileDialogManager.OpenFileDialog( "选择文件", ".tmb,.*", ( bool ok, string res ) => {
                        if( ok ) {
                            CommandManager.Pap.Add( new PapReplaceTmbCommand( this, TmbFile.FromPapEmbedded( res, CommandManager.Pap ) ) );
                            UiUtils.OkNotification( "已导入时间线数据" );
                        }
                    } );
                }
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var _ = ImRaii.PushId( "Tmb" );
            Tmb.Draw();
        }

        private void DrawMotion() {
            using var tabItem = ImRaii.TabItem( "动作" );
            if( !tabItem ) return;

            File.MotionData.Draw( HavokIndex );
        }

        private void DrawHavok() {
            using var tabItem = ImRaii.TabItem( "Havok" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Havok" );

            File.MotionData.DrawHavok( HavokIndex );
        }
    }
}
