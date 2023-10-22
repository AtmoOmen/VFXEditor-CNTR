using ImGuiNET;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.UldFormat.Texture {
    public class UldTexture : UldWorkspaceItem {
        public readonly ParsedPaddedString Path = new( "路径", 44, 0x00 );
        public readonly ParsedUInt IconId = new( "图标 ID" );
        private readonly ParsedUInt Unk1 = new( "未知 1" );

        private bool ShowHd = false;

        public UldTexture() { }

        public UldTexture( BinaryReader reader, char minorVersion ) {
            Id.Read( reader );
            Path.Read( reader );
            IconId.Read( reader );
            if( minorVersion == '1' ) Unk1.Read( reader );
            else Unk1.Value = 0;
        }

        public void Write( BinaryWriter writer, char minorVersion ) {
            Id.Write( writer );
            Path.Write( writer );
            IconId.Write( writer );
            if( minorVersion == '1' ) Unk1.Write( writer );
        }

        public override void Draw() {
            DrawRename();
            Id.Draw( CommandManager.Uld );

            Path.Draw( CommandManager.Uld );

            if( !string.IsNullOrEmpty( Path.Value ) ) {
                ImGui.Checkbox( "显示 HD 素材", ref ShowHd );
                if( ShowHd ) ImGui.TextDisabled( TexturePath );
                Plugin.TextureManager.GetTexture( TexturePath )?.Draw();
            }

            IconId.Draw( CommandManager.Uld );
            if( IconId.Value > 0 ) {
                ImGui.Checkbox( "显示 HD 素材", ref ShowHd );
                ImGui.TextDisabled( IconPath );
                Plugin.TextureManager.GetTexture( IconPath )?.Draw();
            }

            Unk1.Draw( CommandManager.Uld );
        }

        public override string GetDefaultText() => $"材质 {GetIdx()}";

        public override string GetWorkspaceId() => $"Texture{GetIdx()}";

        private string TexturePath => GetTexturePath( ShowHd );

        public string GetTexturePath( bool hd ) => hd ? Path.Value.Replace( ".tex", "_hr1.tex" ) : Path.Value;

        private string IconPath => GetIconPath( ShowHd );

        public string GetIconPath( bool hd ) => string.Format( "ui/icon/{0:D3}000/{1:D6}{2}.tex", IconId.Value / 1000, IconId.Value, hd ? "_hr1" : "" );
    }
}
