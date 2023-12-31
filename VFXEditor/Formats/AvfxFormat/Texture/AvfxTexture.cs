﻿using ImGuiNET;
using OtterGui.Raii;
using System.IO;
using System.Numerics;
using System.Text;
using VfxEditor.AvfxFormat.Nodes;
using VfxEditor.Formats.TextureFormat.Textures;
using VfxEditor.Library.Texture;

namespace VfxEditor.AvfxFormat {
    public class AvfxTexture : AvfxNode {
        public const string NAME = "Tex";

        public readonly AvfxString Path = new( "路径", "Path" );
        public readonly UiNodeGraphView NodeView;

        public AvfxTexture() : base( NAME, AvfxNodeGroupSet.TextureColor ) {
            NodeView = new( this );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Path.SetValue( Encoding.ASCII.GetString( reader.ReadBytes( size ) ) );
        }

        protected override void RecurseChildrenAssigned( bool assigned ) { }

        protected override void WriteContents( BinaryWriter writer ) => writer.Write( Encoding.ASCII.GetBytes( Path.GetValue() ) );

        public override void Draw() {
            using var _ = ImRaii.PushId( "Texture" );
            NodeView.Draw();
            DrawRename();

            var preCombo = ImGui.GetCursorPosX();

            Plugin.LibraryManager.DrawTextureCombo( Path.GetValue(), ( TextureLeaf texture ) => {
                if( texture.DrawSelectable() ) Path.SetValue( texture.GetPath() );
            } );

            var imguiStyle = ImGui.GetStyle();
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( imguiStyle.ItemInnerSpacing.X, imguiStyle.ItemSpacing.Y ) ) ) {
                ImGui.SameLine();
                Path.Draw( ImGui.GetCursorPosX() - preCombo );
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            GetTexture()?.Draw();
        }

        public override void ShowTooltip() {
            ImGui.BeginTooltip();
            GetTexture()?.DrawImage();
            ImGui.EndTooltip();
        }

        public TextureDrawable GetTexture() => Plugin.TextureManager.GetTexture( Path.GetValue() );

        public override string GetDefaultText() => $"材质 {GetIdx()}";

        public override string GetWorkspaceId() => $"Tex{GetIdx()}";
    }
}
