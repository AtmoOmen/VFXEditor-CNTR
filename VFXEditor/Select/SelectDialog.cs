using Dalamud.Interface;
using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using VfxEditor.FileManager;
using VfxEditor.FileManager.Interfaces;
using VfxEditor.Select.Lists;
using VfxEditor.Ui;
using VfxEditor.Utils;

namespace VfxEditor.Select {
    public enum SelectResultType {
        Local,
        GamePath,
        GameItem,
        GameStatus,
        GameAction,
        GameZone,
        GameEmote,
        GameGimmick,
        GameCutscene,
        GameNpc,
        GameMusic,
        GameQuest,
        GameCharacter,
        GameJob,
        GameMisc,
        GameMount,
        GameHousing,
        GameUi
    }

    [Serializable]
    public class SelectResult {
        public SelectResultType Type;
        public string DisplayString;
        public string Path;

        public SelectResult( SelectResultType type, string displayString, string path ) {
            Type = type;
            DisplayString = displayString;
            Path = path;
        }

        public bool CompareTo( SelectResult other ) => Type == other.Type && DisplayString == other.DisplayString && Path == other.Path;
    }

    public abstract class SelectDialog : GenericDialog {
        public static readonly uint FavoriteColor = ImGui.GetColorU32( new Vector4( 1.0f, 0.878f, 0.1058f, 1 ) );
        public static readonly List<string> LoggedFiles = new();

        public readonly IFileManagerSelect Manager;
        public readonly string Extension;
        public readonly bool ShowLocal;
        private readonly Action<SelectResult> Action;

        protected readonly List<SelectTab> GameTabs = new();
        protected readonly List<SelectResult> Favorites;
        protected readonly SelectListTab RecentTab;
        protected readonly SelectFavoriteTab FavoritesTab;
        protected readonly SelectPenumbraTab PenumbraTab;

        private string GamePathInput = "";
        private string LocalPathInput = "";
        private string LoggedFilesSearch = "";

        public SelectDialog( string name, string extension, FileManagerBase manager, bool showLocal ) : this( name, extension, manager, showLocal,
                showLocal ? ( ( SelectResult result ) => manager.SetSource( result ) ) : ( ( SelectResult result ) => manager.SetReplace( result ) ) ) { }

        public SelectDialog( string name, string extension, IFileManagerSelect manager, bool showLocal, Action<SelectResult> action ) : base( name, false, 800, 500 ) {
            Manager = manager;
            Extension = extension;
            Favorites = manager.GetConfig().Favorites;
            ShowLocal = showLocal;
            Action = action;

            RecentTab = new( this, "最近", manager.GetConfig().RecentItems );
            FavoritesTab = new( this, "收藏", manager.GetConfig().Favorites );
            if( showLocal ) PenumbraTab = new( this );
        }

        public void Invoke( SelectResult result ) => Action?.Invoke( result );

        public virtual void Play( string path ) { }

        public override void DrawBody() {
            using var _ = ImRaii.PushId( $"{Manager.GetId()}/{Name}" );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawGameTabs();
            DrawPaths();
            if( ShowLocal ) PenumbraTab.Draw();
            RecentTab.Draw();
            FavoritesTab.Draw();
        }

        // ============= GAME =================

        private void DrawGameTabs() {
            if( GameTabs.Count == 0 ) return;
            using var _ = ImRaii.PushId( "Game" );

            using var tabItem = ImRaii.TabItem( "游戏物品" );
            if( !tabItem ) return;

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            foreach( var tab in GameTabs ) tab.Draw();
        }

        // ======== PATHS =========

        private void DrawPaths() {
            using var _ = ImRaii.PushId( "Paths" );

            using var tabItem = ImRaii.TabItem( "Paths" );
            if( !tabItem ) return;

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            ImGui.Text( "游戏路径" );
            using( var __ = ImRaii.PushId( "Game" ) )
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                ImGui.InputTextWithHint( "##Path", $"vfx/common/eff/wp_astro1h.{Extension}", ref GamePathInput, 255 );

                ImGui.SameLine();
                if( ImGui.Button( "选择" ) ) SelectGamePath( GamePathInput );
            }

            if( ShowLocal ) {
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

                ImGui.Text( "Local Path" );
                using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );
                using var __ = ImRaii.PushId( "Local" );

                ImGui.SetNextItemWidth( UiUtils.GetOffsetInputSize( UiUtils.GetPaddedIconSize( FontAwesomeIcon.FolderOpen ) ) );
                ImGui.InputTextWithHint( "##Path", $"C:\\Users\\me\\Desktop\\custom.{Extension}", ref LocalPathInput, 255 );

                ImGui.SameLine();
                using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    if( ImGui.Button( FontAwesomeIcon.FolderOpen.ToIconString() ) ) {
                        FileDialogManager.OpenFileDialog( "选择文件", $".{Extension},.*", ( bool ok, string res ) => {
                            if( !ok ) return;
                            Invoke( new SelectResult( SelectResultType.Local, "[LOCAL] " + res, res ) );
                        } );
                    }
                }

                ImGui.SameLine();
                if( ImGui.Button( "选择" ) && Path.IsPathRooted( LocalPathInput ) && File.Exists( LocalPathInput ) ) {
                    Invoke( new SelectResult( SelectResultType.Local, "[LOCAL] " + LocalPathInput, LocalPathInput ) );
                    LocalPathInput = "";
                }
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 20 );
            ImGui.Separator();

            if( ImGui.Checkbox( "记录所有文件", ref Plugin.Configuration.LogAllFiles ) ) Plugin.Configuration.Save();

            using var disabled = ImRaii.Disabled( LoggedFiles.Count == 0 && !Plugin.Configuration.LogAllFiles );

            ImGui.SameLine();
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                ImGui.InputTextWithHint( "##Search", "搜索", ref LoggedFilesSearch, 255 );

                ImGui.SameLine();
                using var font = ImRaii.PushFont( UiBuilder.IconFont );
                if( UiUtils.RemoveButton( FontAwesomeIcon.Trash.ToIconString() ) ) LoggedFiles.Clear();
            }


            using var windowPadding = ImRaii.PushStyle( ImGuiStyleVar.WindowPadding, new Vector2( 0, 0 ) );
            using var child = ImRaii.Child( "子级", new Vector2( -1, -1 ), true );

            var searched = LoggedFiles
                .Where( x => x.EndsWith( Extension ) && ( string.IsNullOrEmpty( LoggedFilesSearch ) || x.ToLower().Contains( LoggedFilesSearch.ToLower() ) ) )
                .ToList();

            SelectUiUtils.DisplayVisible( searched.Count, out var preItems, out var showItems, out var postItems, out var itemHeight );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + preItems * itemHeight );

            if( ImGui.BeginTable( "Table", 1, ImGuiTableFlags.RowBg ) ) {
                ImGui.TableSetupColumn( "##Column", ImGuiTableColumnFlags.WidthStretch );

                var idx = 0;
                foreach( var file in searched ) {
                    if( idx < preItems || idx > ( preItems + showItems ) ) { idx++; continue; }

                    ImGui.TableNextColumn();
                    ImGui.SetCursorPosX( ImGui.GetCursorPosX() + 4 );
                    ImGui.Selectable( $"{file}##{idx}" );
                    if( ImGui.IsMouseDoubleClicked( ImGuiMouseButton.Left ) && ImGui.IsItemHovered() ) SelectGamePath( file );

                    idx++;
                }

                ImGui.EndTable();
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + postItems * itemHeight );
        }

        private void SelectGamePath( string path ) {
            var cleanedPath = path.Trim().Replace( "\\", "/" );
            if( Dalamud.DataManager.FileExists( cleanedPath ) ) {
                Invoke( new SelectResult( SelectResultType.GamePath, "[GAME] " + cleanedPath, cleanedPath ) );
                GamePathInput = "";
            }
        }

        // ======== FAVORITES ======

        public bool DrawFavorite( SelectResult selectResult ) {
            var res = false;
            var isFavorite = IsFavorite( selectResult );

            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) )
            using( var color = ImRaii.PushColor( ImGuiCol.Text, FavoriteColor, isFavorite ) ) {
                ImGui.Text( FontAwesomeIcon.Star.ToIconString() );

                if( ImGui.IsItemClicked() ) {
                    if( isFavorite ) RemoveFavorite( selectResult );
                    else AddFavorite( selectResult );
                    res = true;
                }
            }

            ImGui.SameLine();
            ImGui.SetCursorPosX( ImGui.GetCursorPosX() - 2 );
            return res;
        }

        private bool IsFavorite( SelectResult result ) => Favorites.Any( result.CompareTo );

        private void AddFavorite( SelectResult result ) {
            Favorites.Add( result );
            Plugin.Configuration.Save();
        }

        private void RemoveFavorite( SelectResult result ) {
            Favorites.RemoveAll( result.CompareTo );
            Plugin.Configuration.Save();
        }
    }
}