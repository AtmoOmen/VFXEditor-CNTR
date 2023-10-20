using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Numerics;
using VfxEditor.Utils;

namespace VfxEditor.Select {
    public class SelectUiUtils {
        public static SelectResult GetSelectResult( string path, SelectResultType resultType, string resultName ) {
            var resultPrefix = resultType.ToString().ToUpper().Replace( "GAME", "" );
            return new SelectResult( resultType, $"[{resultPrefix}] {resultName}", path );
        }

        public static void DisplayNoVfx() {
            using( var style = ImRaii.PushColor( ImGuiCol.Text, UiUtils.YELLOW_COLOR ) ) {
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
                ImGui.TextWrapped( $"这一对象没有预置视觉效果，你可以点击下面的链接查阅如何为其添加一个视效(英文)" );
            }
            UiUtils.WikiButton( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki/Adding-a-VFX-to-an-Item-Without-One" );
        }

        public static bool Matches( string item, string query ) => item.ToLower().Contains( query.ToLower() );

        public static void DisplayPath( string path ) {
            using var style = ImRaii.PushColor( ImGuiCol.Text, new Vector4( 0.8f, 0.8f, 0.8f, 1 ) );
            ImGui.TextWrapped( path );
        }

        public static void DisplayPathWarning( string path, string warning ) {
            using( var style = ImRaii.PushColor( ImGuiCol.Text, UiUtils.YELLOW_COLOR ) ) {
                ImGui.TextWrapped( $"{path} (!)" );
            }
            UiUtils.Tooltip( warning );
        }

        public static void Copy( string path ) {
            using var style = ImRaii.PushColor( ImGuiCol.Button, new Vector4( 0.15f, 0.15f, 0.15f, 1 ) );
            if( ImGui.Button( "复制" ) ) ImGui.SetClipboardText( path );
        }

        public static void DisplayVisible( int count, out int preItems, out int showItems, out int postItems, out float itemHeight ) {
            var childHeight = ImGui.GetWindowSize().Y - ImGui.GetCursorPosY();
            var scrollY = ImGui.GetScrollY();
            var style = ImGui.GetStyle();
            itemHeight = ImGui.GetTextLineHeight() + style.ItemSpacing.Y;
            preItems = ( int )Math.Floor( scrollY / itemHeight );
            showItems = ( int )Math.Ceiling( childHeight / itemHeight );
            postItems = count - showItems - preItems;
        }

        public static void DrawIcon( ImGuiScene.TextureWrap icon ) {
            if( icon != null && icon.ImGuiHandle != IntPtr.Zero ) {
                ImGui.Image( icon.ImGuiHandle, new Vector2( icon.Width, icon.Height ) );
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );
            }
        }

        public static void NpcThankYou() {
            ImGui.TextDisabled( "请查阅由 ResLogger 提供的 NPC 文件列表" );
            ImGui.SameLine();
            if( ImGui.SmallButton( "GitHub##ResLogger" ) ) UiUtils.OpenUrl( "https://github.com/lmcintyre/ResLogger2" );
        }

        public static byte[] BgraToRgba( byte[] data ) {
            var ret = new byte[data.Length];
            for( var i = 0; i < data.Length / 4; i++ ) {
                var idx = i * 4;
                ret[idx + 0] = data[idx + 2];
                ret[idx + 1] = data[idx + 1];
                ret[idx + 2] = data[idx + 0];
                ret[idx + 3] = data[idx + 3];
            }
            return ret;
        }
    }
}
