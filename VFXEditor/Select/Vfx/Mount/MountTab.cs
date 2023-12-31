﻿using Dalamud.Logging;
using ImGuiNET;
using Lumina.Data.Files;
using System;
using System.Linq;
using VfxEditor.Select.Shared.Mount;

namespace VfxEditor.Select.Vfx.Mount {
    public class MountRowSelected {
        public string ImcPath;
        public int VfxId;
        public string VfxPath;
    }

    public class MountTab : SelectTab<MountRow, MountRowSelected> {
        public MountTab( SelectDialog dialog, string name ) : base( dialog, name, "Shared-Mount", SelectResultType.GameMount ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Plugin.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Mount>().Where( x => !string.IsNullOrEmpty( x.Singular ) );
            foreach( var item in sheet ) Items.Add( new MountRow( item ) );
        }

        public override void LoadSelection( MountRow item, out MountRowSelected loaded ) {
            loaded = null;
            var imcPath = item.GetImcPath();

            if( !Plugin.DataManager.FileExists( imcPath ) ) return;
            try {
                var file = Plugin.DataManager.GetFile<ImcFile>( imcPath );
                var vfxIds = file.GetParts().Select( x => x.Variants[item.Variant - 1] ).Where( x => x.VfxId != 0 ).Select( x => ( int )x.VfxId ).ToList();
                var vfxId = vfxIds.Count > 0 ? vfxIds[0] : 0;

                loaded = new() {
                    ImcPath = file.FilePath,
                    VfxId = vfxId,
                    VfxPath = vfxId > 0 ? item.GetVfxPath( vfxId ) : ""
                };
            }
            catch( Exception e ) {
                PluginLog.Error( e, "加载 IMC 文件 时发生错误" + imcPath );
            }
        }

        // ===== DRAWING ======

        protected override void OnSelect() => LoadIcon( Selected.Icon );

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            ImGui.Text( "分支: " + Selected.Variant );
            ImGui.Text( "IMC: " );
            ImGui.SameLine();
            SelectUiUtils.DisplayPath( Loaded.ImcPath );

            DrawPath( "视效", Loaded.VfxPath, Selected.Name, true );
        }

        protected override string GetName( MountRow item ) => item.Name;
    }
}
