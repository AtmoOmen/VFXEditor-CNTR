using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;

namespace VfxEditor.SklbFormat.Data {
    public class SklbOldData : SklbData {
        private readonly List<ParsedBase> Parsed;

        public SklbOldData( BinaryReader reader ) {
            reader.ReadInt16(); // layer offset, always 0x2E
            HavokOffset = reader.ReadInt16();

            Parsed = new() {
                Id,
                new ParsedShort2( "父级 1"),
                new ParsedShort2( "父级 2"),
                new ParsedShort2( "父级 3"),
                new ParsedShort2( "父级 4"),
                new ParsedShort( "LoD 骨骼 1" ),
                new ParsedShort( "LoD 骨骼 2" ),
                new ParsedShort( "LoD 骨骼 3" ),
                new ParsedShort( "连接骨骼 1" ),
                new ParsedShort( "连接骨骼 2" ),
                new ParsedShort( "连接骨骼 3" ),
                new ParsedShort( "连接骨骼 4" )
            };

            Parsed.ForEach( x => x.Read( reader ) );
        }

        public override long Write( BinaryWriter writer ) {
            writer.Write( ( short )0x2E );
            var havokOffset = writer.BaseStream.Position;
            writer.Write( ( short )0 ); // placeholder

            Parsed.ForEach( x => x.Write( writer ) );

            return havokOffset;
        }

        public override void Draw() {
            ImGui.TextDisabled( "头版本 [旧]" );

            Parsed.ForEach( x => x.Draw( CommandManager.Sklb ) );
        }
    }
}
