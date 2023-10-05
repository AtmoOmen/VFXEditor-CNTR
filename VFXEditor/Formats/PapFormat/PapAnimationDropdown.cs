using ImGuiFileDialog;
using System.Collections.Generic;
using VfxEditor.Interop.Havok;
using VfxEditor.Ui.Components;
using VfxEditor.Utils;

namespace VfxEditor.PapFormat {
    public unsafe class PapAnimationDropdown : Dropdown<PapAnimation> {
        private readonly PapFile File;

        public PapAnimationDropdown( PapFile file, List<PapAnimation> items ) : base( "动画", items, true, true ) {
            File = file;
        }

        protected override string GetText( PapAnimation item, int idx ) => item.GetName();

        protected override void OnDelete( PapAnimation item ) {
            var index = Items.IndexOf( item );

            var command = new CompoundCommand();
            command.Add( new PapAnimationRemoveCommand( File, Items, item ) );
            command.Add( new PapHavokCommand( File, () => {
                var container = File.MotionData.AnimationContainer;

                var anims = HavokData.ToList( container->Animations );
                var bindings = HavokData.ToList( container->Bindings );
                anims.RemoveAt( index );
                bindings.RemoveAt( index );

                container->Animations = HavokData.CreateArray( container->Animations.Flags, anims, sizeof( nint ), out var _ );
                container->Bindings = HavokData.CreateArray( container->Bindings.Flags, bindings, sizeof( nint ), out var _ );
            } ) );
            File.Command.Add( command );

            UiUtils.OkNotification( "已移除 Havok 数据" );
        }

        protected override void OnNew() {
            FileDialogManager.OpenFileDialog( "选择文件", ".hkx,.*", ( bool ok, string res ) => {
                if( ok ) Plugin.AddModal( new PapAddModal( File, res ) );
            } );
        }

        protected override void DrawSelected() => Selected.Draw();
    }
}
