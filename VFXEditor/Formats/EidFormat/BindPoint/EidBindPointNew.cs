﻿using ImGuiNET;
using System.IO;
using VfxEditor.EidFormat.BindPoint;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.EidFormat {
    public class EidBindPointNew : EidBindPoint {
        public readonly ParsedPaddedString Name = new( "骨骼名", "n_root", 32, 0x00 );
        public readonly ParsedInt Id = new( "Id" );
        public readonly ParsedFloat3 Position = new( "位置" );
        public readonly ParsedRadians3 Rotation = new( "旋转" );

        public EidBindPointNew() { }

        public EidBindPointNew( BinaryReader reader ) {
            Name.Read( reader );
            Id.Read( reader );
            Position.Read( reader );
            Rotation.Read( reader );
            reader.ReadInt32(); // padding
        }

        public override void Write( BinaryWriter writer ) {
            Name.Write( writer );
            Id.Write( writer );
            Position.Write( writer );
            Rotation.Write( writer );
            writer.Write( 0 );
        }

        public override int GetId() => Id.Value;

        public override void Draw() {
            ImGui.TextDisabled( "数据版本: [新]" );

            Name.Draw( CommandManager.Eid );
            Id.Draw( CommandManager.Eid );
            Position.Draw( CommandManager.Eid );
            Rotation.Draw( CommandManager.Eid );
        }
    }
}
