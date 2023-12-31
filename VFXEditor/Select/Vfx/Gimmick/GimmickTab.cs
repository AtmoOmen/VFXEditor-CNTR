﻿using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using OtterGui.Raii;
using System.Collections.Generic;
using System.Linq;
using VfxEditor.Select.Shared;

namespace VfxEditor.Select.Vfx.Gimmick {
    public class GimmickTab : SelectTab<GimmickRow, ParseAvfx> {
        public GimmickTab( SelectDialog dialog, string name ) : base( dialog, name, "Vfx-Gimmick", SelectResultType.GameGimmick ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var territories = Plugin.DataManager.GetExcelSheet<TerritoryType>().Where( x => !string.IsNullOrEmpty( x.Name ) ).ToList();
            var suffixToName = new Dictionary<string, string>();
            foreach( var zone in territories ) {
                suffixToName[zone.Name.ToString()] = zone.PlaceName.Value?.Name.ToString();
            }

            var sheet = Plugin.DataManager.GetExcelSheet<ActionTimeline>().Where( x => x.Key.ToString().Contains( "gimmick" ) );
            foreach( var item in sheet ) {
                Items.Add( new GimmickRow( item, suffixToName ) );
            }
        }

        public override void LoadSelection( GimmickRow item, out ParseAvfx loaded ) => ParseAvfx.ReadFile( item.TmbPath, out loaded );

        // ===== DRAWING ======

        protected override void DrawSelected() {
            if( Loaded.VfxExists ) {
                using( var _ = ImRaii.PushId( "CopyTmb" ) ) {
                    SelectUiUtils.Copy( Selected.TmbPath );
                }

                ImGui.SameLine();
                ImGui.Text( "时间线:" );
                ImGui.SameLine();
                SelectUiUtils.DisplayPath( Selected.TmbPath );

                DrawPaths( "视效", Loaded.VfxPaths, Selected.Name, true );
            }
        }

        protected override string GetName( GimmickRow item ) => item.Name;
    }
}
