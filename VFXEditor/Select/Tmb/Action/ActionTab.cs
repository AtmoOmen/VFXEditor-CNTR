using Dalamud.Interface;
using ImGuiNET;
using System.Linq;
using VfxEditor.Utils;

namespace VfxEditor.Select.Tmb.Action {
    public class ActionTab : SelectTab<ActionRow> {
        public ActionTab( SelectDialog dialog, string name ) : this( dialog, name, "Tmb-Action" ) { }

        public ActionTab( SelectDialog dialog, string name, string stateId ) : base( dialog, name, stateId, SelectResultType.GameAction ) { }

        // ===== LOADING =====

        public override void LoadData() {
            var sheet = Dalamud.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()
                .Where( x => !string.IsNullOrEmpty( x.Name ) && ( x.IsPlayerAction || x.ClassJob.Value != null ) && !x.AffectsPosition );
            foreach( var item in sheet ) Items.Add( new ActionRow( item ) );
        }

        // ===== DRAWING ======

        protected override void OnSelect() => LoadIcon( Selected.Icon );

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            DrawPath( "开始", Selected.StartPath, $"{Selected.Name} Start", true );
            DrawMovementCancel( Selected.StartMotion );

            DrawPath( "结束", Selected.EndPath, $"{Selected.Name} End", true );
            DrawMovementCancel( Selected.EndMotion );

            DrawPath( "命中", Selected.HitPath, $"{Selected.Name} Hit", true );

            DrawPath( "武器", Selected.WeaponPath, $"{Selected.Name} Weapon", true );
        }

        protected override string GetName( ActionRow item ) => item.Name;

        private void DrawMovementCancel( bool disabled ) {
            if( !disabled ) return;
            if( Dialog.ShowLocal ) return;
            ImGui.Indent( 25f );
            UiUtils.IconText( FontAwesomeIcon.QuestionCircle, true );
            UiUtils.Tooltip( "这一参数位于游戏的数据表格中，无法被 VFXEditor 移除" );
            ImGui.SameLine();
            ImGui.TextDisabled( "被移动取消的动画" );
            ImGui.Unindent( 25f );
        }
    }
}
