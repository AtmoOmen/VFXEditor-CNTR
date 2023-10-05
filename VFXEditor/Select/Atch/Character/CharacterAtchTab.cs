using VfxEditor.Select.Shared.Character;

namespace VfxEditor.Select.Atch.Character {
    public class CharacterAtchTab : CharacterTab {
        public CharacterAtchTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            DrawPath( "路径", Selected.AtchPath, Selected.Name );
        }
    }
}
