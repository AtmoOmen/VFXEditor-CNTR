using Dalamud.Configuration;
using Dalamud.Interface;
using Dalamud.Logging;
using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VfxEditor.Formats.TextureFormat;
using VfxEditor.Library;
using VfxEditor.Select;
using VfxEditor.SklbFormat.Bones;
using VfxEditor.Ui;

namespace VfxEditor {
    [Serializable]
    public unsafe class ManagerConfiguration {
        public List<SelectResult> RecentItems = new();
        public List<SelectResult> Favorites = new();
        public bool UseCustomWindowColor = false;
        public Vector4 TitleBg = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBg );
        public Vector4 TitleBgActive = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBgActive );
        public Vector4 TitleBgCollapsed = *ImGui.GetStyleColorVec4( ImGuiCol.TitleBgCollapsed );
    }

    [Serializable]
    public class Configuration : GenericDialog, IPluginConfiguration {
        public int Version { get; set; } = 0;
        public bool IsEnabled { get; set; } = true;

        public bool LogAllFiles = false;
        public bool LogDebug = false;
        public bool LogVfxDebug = false;
        public bool LogVfxTriggers = false;

        public bool HideWithUI = true;
        public bool UpdateWriteLocation = true;
        public bool AutosaveEnabled = false;
        public int AutosaveSeconds = 300;
        public int SaveRecentLimit = 10;
        public int MaxUndoSize = 10;
        public bool OverlayLimit = true;
        public float OverlayRemoveDelay = 1;
        public bool FilepickerImagePreview = true;
        public bool BlockGameInputsWhenFocused = false;
        public string WriteLocation = Path.Combine( new[] {
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "XIVLauncher",
            "pluginConfigs",
            "VFXEditor",
        } );

        public bool VfxSpawnLoop = false;
        public float VfxSpawnDelay = 0.1f;

        // ===== [ OBSOLETE ] =======
        public List<SelectResult> RecentSelects = new();
        public List<SelectResult> RecentSelectsTMB = new();
        public List<SelectResult> RecentSelectsPAP = new();
        public List<SelectResult> RecentSelectsScd = new();
        public List<SelectResult> FavoriteVfx = new();
        public List<SelectResult> FavoriteTmb = new();
        public List<SelectResult> FavoritePap = new();
        public List<SelectResult> FavoriteScd = new();
        // ===========================

        public Dictionary<string, ManagerConfiguration> ManagerConfigs = new();

        public KeybindConfiguration SaveKeybind = new();
        public KeybindConfiguration SaveAsKeybind = new();
        public KeybindConfiguration OpenKeybind = new();
        public KeybindConfiguration UpdateKeybind = new();
        public KeybindConfiguration CopyKeybind = new();
        public KeybindConfiguration PasteKeybind = new();
        public KeybindConfiguration UndoKeybind = new();
        public KeybindConfiguration RedoKeybind = new();
        public KeybindConfiguration SpawnOnSelfKeybind = new();
        public KeybindConfiguration SpawnOnGroundKeybind = new();
        public KeybindConfiguration SpawnOnTargetKeybind = new();

        public List<LibraryProps> VFXNodeLibraryItems = new();
        public List<LibraryProps> VfxTextureLibraryItems = new();
        public bool VfxTextureDefaultLoaded = false;

        public bool LoopMusic = true;
        public bool LoopSoundEffects = false;
        public float ScdVolume = 1f;
        public bool SimulateScdLoop = false;

        public bool UseDegreesForAngles = false;

        public Vector4 CurveEditorLineColor = new( 0, 0.1f, 1, 1 );
        public Vector4 CurveEditorPointColor = new( 1 );
        public Vector4 CurveEditorSelectedColor = new( 1.000f, 0.884f, 0.561f, 1f );
        public Vector4 CurveEditorPrimarySelectedColor = new( 0.984375f, 0.7265625f, 0.01176470f, 1.0f );
        public List<Vector4> CurveEditorPalette = new();
        public int CurveEditorLineWidth = 2;
        public int CurveEditorColorRingSize = 3;
        public int CurveEditorGrabbingDistance = 25;
        public int CurveEditorPointSize = 7;
        public int CurveEditorSelectedSize = 10;
        public int CurveEditorPrimarySelectedSize = 12;
        public Vector4 TimelineSelectedColor = new( 1f, 0.532f, 0f, 1f );
        public Vector4 TimelineBarColor = new( 0.44f, 0.457f, 0.492f, 1f );

        public bool ShowTabBar = true;
        public bool DocumentPopoutShowSource = false;

        public bool PhybSkeletonSplit = true;
        public bool EidSkeletonSplit = true;
        public bool ShowBoneNames = true;

        public BoneDisplay SklbBoneDisplay = BoneDisplay.Blender_Style_Perpendicular;

        public bool ModelWireframe = false;
        public bool ModelShowEdges = true;
        public bool ModelShowEmitters = true;

        public Vector4 LuaParensColor = new( 0.5f, 0.5f, 0.5f, 1f );
        public Vector4 LuaFunctionColor = new( 0f, 0.439f, 1f, 1f );
        public Vector4 LuaLiteralColor = new( 0.639f, 0.207f, 0.933f, 1f );
        public Vector4 LuaVariableColor = new( 0.125f, 0.67058f, 0.45098f, 1f );

        public int PngMips = 9;
        public TextureFormat PngFormat = TextureFormat.DXT5;

        [NonSerialized]
        public bool WriteLocationError = false;

        public Configuration() : base( "设置", false, 300, 200 ) { }

        public void Setup() {
            Dalamud.PluginInterface.UiBuilder.DisableUserUiHide = !HideWithUI;
            FileDialogManager.ImagePreview = FilepickerImagePreview;

            // Move old configurations over to new
            ProcessOldManagerConfigs( RecentSelects, FavoriteVfx, "Vfx" );
            ProcessOldManagerConfigs( RecentSelectsTMB, FavoriteTmb, "Tmb" );
            ProcessOldManagerConfigs( RecentSelectsPAP, FavoritePap, "Pap" );
            ProcessOldManagerConfigs( RecentSelectsScd, FavoriteScd, "Scd" );

            try { Directory.CreateDirectory( WriteLocation ); }
            catch( Exception ) { WriteLocationError = true; }

            PluginLog.Log( "写入路径: " + WriteLocation );

            if( CurveEditorPalette.Count == 0 ) {
                CurveEditorPalette.AddRange( ImGuiHelpers.DefaultColorPalette( 56 ) );
            }
        }

        public ManagerConfiguration GetManagerConfig( string id ) {
            if( ManagerConfigs.TryGetValue( id, out var config ) ) return config;
            var newConfig = new ManagerConfiguration();
            ManagerConfigs.Add( id, newConfig );
            return newConfig;
        }

        private void ProcessOldManagerConfigs( List<SelectResult> recent, List<SelectResult> favorites, string key ) {
            if( recent.Count == 0 && favorites.Count == 0 ) return;

            if( !ManagerConfigs.ContainsKey( key ) ) ManagerConfigs[key] = new();
            ManagerConfigs[key].RecentItems.AddRange( recent );
            ManagerConfigs[key].Favorites.AddRange( favorites );

            recent.Clear();
            favorites.Clear();
        }

        public void AddRecent( List<SelectResult> recentList, SelectResult result ) {
            if( result == null ) return;
            recentList.RemoveAll( result.CompareTo );

            recentList.Add( result );
            if( recentList.Count > SaveRecentLimit ) recentList.RemoveRange( 0, recentList.Count - SaveRecentLimit );
            Save();
        }

        public void Save() {
            Dalamud.PluginInterface.SavePluginConfig( this );
            Dalamud.PluginInterface.UiBuilder.DisableUserUiHide = !HideWithUI;
        }

        // =================

        public override void DrawBody() {
            using var _ = ImRaii.PushId( "##Settings" );

            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "设置" ) ) {
                DrawConfiguration();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "按键绑定" ) ) {
                DrawKeybinds();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Vfx" ) ) {
                DrawVfx();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "Tmb" ) ) {
                DrawTmb();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "文件编辑器" ) ) {
                DrawEditorSpecific();
                ImGui.EndTabItem();
            }
        }

        private void DrawConfiguration() {
            using var child = ImRaii.Child( "Config" );

            ImGui.TextDisabled( "对缓存文件位置的修改可能需要通过重启游戏来生效" );
            if( ImGui.InputText( "缓存文件位置", ref WriteLocation, 255 ) ) Save();
            if( ImGui.Checkbox( "每次刷新时更新写入地址", ref UpdateWriteLocation ) ) Save();

            if( ImGui.Checkbox( "记录所有文件", ref LogAllFiles ) ) Save();
            if( ImGui.Checkbox( "记录 DEBUG 信息", ref LogDebug ) ) Save();
            if( ImGui.Checkbox( "记录视效 DEBUG 信息", ref LogVfxDebug ) ) Save();
            if( ImGui.Checkbox( "Log Vfx triggers", ref LogVfxTriggers ) ) Save();

            if( ImGui.Checkbox( "自动保存工作区", ref AutosaveEnabled ) ) Save();
            using( var autosaveDim = ImRaii.PushStyle( ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f, !AutosaveEnabled ) )
            using( var indent = ImRaii.PushIndent() ) {
                ImGui.SetNextItemWidth( 120 );
                if( ImGui.InputInt( "自动保存时间间隔 (秒)", ref AutosaveSeconds ) ) Save();
            }

            if( ImGui.Checkbox( "跟随 UI 隐藏", ref HideWithUI ) ) Save();

            if( ImGui.Checkbox( "选择文件时预览图像", ref FilepickerImagePreview ) ) {
                FileDialogManager.ImagePreview = FilepickerImagePreview;
                Save();
            }

            ImGui.SetNextItemWidth( 135 );
            if( ImGui.InputInt( "近期文件限制数", ref SaveRecentLimit ) ) {
                SaveRecentLimit = Math.Max( SaveRecentLimit, 0 );
                Save();
            }

            ImGui.SetNextItemWidth( 135 );
            if( ImGui.InputFloat( "实时叠加层移除延迟时间", ref OverlayRemoveDelay ) ) Save();
            if( ImGui.Checkbox( "实时叠加层距离限制", ref OverlayLimit ) ) Save();

            ImGui.SetNextItemWidth( 135 );
            if( ImGui.InputInt( "可回退的历史操作步数", ref MaxUndoSize ) ) Save();

            if( ImGui.Checkbox( "Show tab bar", ref ShowTabBar ) ) Save();
        }

        private void DrawKeybinds() {
            if( ImGui.Checkbox( "当 VFXEditor 窗口处于活动状态时屏蔽游戏内输入##Settings", ref BlockGameInputsWhenFocused ) ) Save();

            using var child = ImRaii.Child( "按键绑定", new Vector2( -1 ), false );

            if( SaveKeybind.Draw( "保存" ) ) Save();
            if( SaveAsKeybind.Draw( "另存为" ) ) Save();
            if( OpenKeybind.Draw( "打开" ) ) Save();
            if( CopyKeybind.Draw( "复制" ) ) Save();
            if( PasteKeybind.Draw( "粘贴" ) ) Save();
            if( UndoKeybind.Draw( "撤销" ) ) Save();
            if( RedoKeybind.Draw( "重做" ) ) Save();
            if( UpdateKeybind.Draw( "刷新" ) ) Save();
            if( SpawnOnSelfKeybind.Draw( "在自身生成(仅限视效)" ) ) Save();
            if( SpawnOnGroundKeybind.Draw( "在地面生成(仅限视效)" ) ) Save();
            if( SpawnOnTargetKeybind.Draw( "在目标生成(仅限视效)" ) ) Save();
        }

        private void DrawVfx() {
            using var child = ImRaii.Child( "Vfx" );

            if( ImGui.CollapsingHeader( "曲线编辑器", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "线颜色", ref CurveEditorLineColor ) ) Save();
                if( ImGui.ColorEdit4( "点颜色", ref CurveEditorPointColor ) ) Save();
                if( ImGui.ColorEdit4( "基础颜色", ref CurveEditorPrimarySelectedColor ) ) Save();
                if( ImGui.ColorEdit4( "已选颜色", ref CurveEditorSelectedColor ) ) Save();

                if( ImGui.InputInt( "线宽", ref CurveEditorLineWidth ) ) Save();
                if( ImGui.InputInt( "颜色环宽度", ref CurveEditorColorRingSize ) ) Save();
                if( ImGui.InputInt( "点大小", ref CurveEditorPointSize ) ) Save();
                if( ImGui.InputInt( "基础大小", ref CurveEditorPrimarySelectedSize ) ) Save();
                if( ImGui.InputInt( "已选大小", ref CurveEditorSelectedSize ) ) Save();
                if( ImGui.InputInt( "选取距离", ref CurveEditorGrabbingDistance ) ) Save();
            }

            if( ImGui.CollapsingHeader( "时间线编辑器", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "已选颜色", ref TimelineSelectedColor ) ) Save();
                if( ImGui.ColorEdit4( "顶栏颜色", ref TimelineBarColor ) ) Save();
            }
        }

        private void DrawTmb() {
            using var child = ImRaii.Child( "Tmb" );

            if( ImGui.CollapsingHeader( "Lua", ImGuiTreeNodeFlags.DefaultOpen ) ) {
                using var indent = ImRaii.PushIndent( 10f );

                if( ImGui.ColorEdit4( "括号颜色", ref LuaParensColor ) ) Save();
                if( ImGui.ColorEdit4( "函数颜色", ref LuaFunctionColor ) ) Save();
                if( ImGui.ColorEdit4( "字面值颜色", ref LuaLiteralColor ) ) Save();
                if( ImGui.ColorEdit4( "变量颜色", ref LuaVariableColor ) ) Save();
            }
        }

        private void DrawEditorSpecific() {
            using var child = ImRaii.Child( "EditorSpecific" );

            foreach( var config in ManagerConfigs ) {
                using var _ = ImRaii.PushId( config.Key );

                if( ImGui.CollapsingHeader( config.Key ) ) {
                    using var indent = ImRaii.PushIndent( 5f );

                    ImGui.Checkbox( "使用自定义窗口颜色", ref config.Value.UseCustomWindowColor );
                    if( config.Value.UseCustomWindowColor ) {
                        if( ImGui.ColorEdit4( "背景色", ref config.Value.TitleBg ) ) Save();
                        if( ImGui.ColorEdit4( "激活时的颜色", ref config.Value.TitleBgActive ) ) Save();
                        if( ImGui.ColorEdit4( "折叠时的颜色", ref config.Value.TitleBgCollapsed ) ) Save();
                    }
                }
            }
        }
    }
}