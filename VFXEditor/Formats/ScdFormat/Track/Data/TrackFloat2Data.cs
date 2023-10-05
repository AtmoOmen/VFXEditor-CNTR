using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class TrackFloat2Data : ScdTrackData {
        public readonly ParsedFloat2 Value = new( "值" );

        public override void Read( BinaryReader reader ) {
            Value.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            Value.Write( writer );
        }

        public override void Draw() {
            Value.Draw( CommandManager.Scd );
        }
    }
}
