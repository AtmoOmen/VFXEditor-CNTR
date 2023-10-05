using ImGuiNET;
using OtterGui.Raii;
using System.Linq;
using VfxEditor.Select.Shared;

namespace VfxEditor.Select.Vfx.Action {
    public class ActionTab : SelectTab<ActionRow, ParsedPaths> {
        public ActionTab( SelectDialog dialog, string name ) : this( dialog, name, "Vfx-Action" ) { }

        public ActionTab( SelectDialog dialog, string name, string stateId ) : base( dialog, name, stateId, SelectResultType.GameAction ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()
                .Where( x => !string.IsNullOrEmpty( x.Name ) && ( x.IsPlayerAction || x.ClassJob.Value != null ) );
            foreach( var item in sheet ) {
                var action = new ActionRow( item, false );
                Items.Add( action );
                if( action.HitAction != null ) Items.Add( action.HitAction );
            }
        }

        public override void LoadSelection( ActionRow item, out ParsedPaths loaded ) {
            if( string.IsNullOrEmpty( item.TmbPath ) ) { // no need to get the file
                loaded = new ParsedPaths();
                return;
            }

            ParsedPaths.ReadFile( item.TmbPath, SelectDataUtils.AvfxRegex, out loaded );
        }

        // ===== DRAWING ======

        protected override void OnSelect() => LoadIcon( Selected.Icon );

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            if( !string.IsNullOrEmpty( Loaded.OriginalPath ) ) {
                using( var _ = ImRaii.PushId( "CopyTmb" ) ) {
                    SelectUiUtils.Copy( Loaded.OriginalPath );
                }

                ImGui.SameLine();
                ImGui.Text( "时间线:" );
                ImGui.SameLine();
                SelectUiUtils.DisplayPath( Loaded.OriginalPath );
            }

            DrawPath( "咏唱", Selected.CastVfxPath, $"{Selected.Name} 咏唱", true );
            DrawPath( "开始", Selected.StartVfxPath, $"{Selected.Name} 开始", true );
            if( !string.IsNullOrEmpty( Loaded.OriginalPath ) ) {
                DrawPaths( "VFX", Loaded.Paths, Selected.Name, true );
            }
        }

        protected override string GetName( ActionRow item ) => item.Name;
    }
}
