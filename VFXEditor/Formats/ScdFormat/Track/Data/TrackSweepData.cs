using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public class TrackSweepData : ScdTrackData {
        public readonly ParsedFloat Pitch = new( "音高" );
        public readonly ParsedInt Time = new( "时间" );

        public override void Read( BinaryReader reader ) {
            Pitch.Read( reader );
            Time.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            Pitch.Write( writer );
            Time.Write( writer );
        }

        public override void Draw() {
            Pitch.Draw( CommandManager.Scd );
            Time.Draw( CommandManager.Scd );
        }
    }
}
