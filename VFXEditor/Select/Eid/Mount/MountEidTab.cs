using VfxEditor.Select.Shared.Mount;

namespace VfxEditor.Select.Eid.Mount {
    public class MountEidTab : MountTab {
        public MountEidTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            DrawPath( "路径", Selected.GetEidPath(), Selected.Name );
        }
    }
}
