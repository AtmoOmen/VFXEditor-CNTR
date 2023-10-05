using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.Formats.SkpFormat.LookAt {
    public class SkpLookAtGroupElement : IUiItem {
        private readonly ParsedByte Priority = new( "优先级" );
        private readonly ParsedByte ParameterIndex = new( "Setup Parameters Index" );
        public readonly ParsedPaddedString BoneName = new( "骨骼名", 32, 0x00 );
        public readonly ParsedPaddedString ParentBoneName = new( "Parent Bone Name", 32, 0x00 );

        public SkpLookAtGroupElement() { }

        public SkpLookAtGroupElement( BinaryReader reader ) {
            Priority.Read( reader );
            ParameterIndex.Read( reader );
            BoneName.Read( reader );
            ParentBoneName.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Priority.Write( writer );
            ParameterIndex.Write( writer );
            BoneName.Write( writer );
            ParentBoneName.Write( writer );
        }

        public void Draw() {
            Priority.Draw( CommandManager.Skp );
            ParameterIndex.Draw( CommandManager.Skp );
            BoneName.Draw( CommandManager.Skp );
            ParentBoneName.Draw( CommandManager.Skp );
        }
    }
}
