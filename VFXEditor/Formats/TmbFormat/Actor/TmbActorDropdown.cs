﻿using System.Numerics;
using VfxEditor.FileManager;
using VfxEditor.TmbFormat.Entries;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Ui.Components;

namespace VfxEditor.TmbFormat.Actor {
    public class TmbActorDropdown : Dropdown<Tmac> {
        private readonly TmbFile File;

        public TmbActorDropdown( TmbFile file ) : base( "Actor", file.Actors, true, true ) {
            File = file;
        }

        protected override string GetText( Tmac item, int idx ) => $"角色 {idx}";

        protected override bool DoColor( Tmac item, out Vector4 color ) => TmbEntry.DoColor( item.MaxDanger, out color );

        protected override void OnDelete( Tmac item ) {
            var command = new TmbRefreshIdsCommand( File );
            command.Add( new GenericRemoveCommand<Tmac>( Items, item ) );
            command.Add( new GenericRemoveCommand<Tmac>( File.HeaderTmal.Actors, item ) );
            item.DeleteChildren( command, File );
            File.Command.Add( command );
        }

        protected override void OnNew() {
            var newActor = new Tmac( File );

            var command = new TmbRefreshIdsCommand( File );
            command.Add( new GenericAddCommand<Tmac>( Items, newActor ) );
            command.Add( new GenericAddCommand<Tmac>( File.HeaderTmal.Actors, newActor ) );
            File.Command.Add( command );
        }

        protected override void DrawSelected() => Selected.Draw();
    }
}
