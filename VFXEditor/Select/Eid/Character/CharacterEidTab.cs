using VfxEditor.Select.Shared.Character;

namespace VfxEditor.Select.Eid.Character {
    public class CharacterEidTab : CharacterTab {
        public CharacterEidTab( SelectDialog dialog, string name ) : base( dialog, name ) { }

        protected override void DrawSelected() {
            DrawPath( "路径", Selected.EidPath, Selected.Name );
        }
    }
}
