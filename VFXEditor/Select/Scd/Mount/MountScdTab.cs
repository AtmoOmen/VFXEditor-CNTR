using VfxEditor.Select.Shared.Mount;

namespace VfxEditor.Select.Scd.Mount {
    public class MountScdTab : MountTab {
        public MountScdTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            DrawPath( "坐骑", Selected.GetMountSound(), $"{Selected.Name} Mount" );
            DrawPath( "Bgm", Selected.Bgm, $"{Selected.Name} BGM" );
        }
    }
}
