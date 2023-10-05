using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.AvfxFormat {
    public class AvfxEmitVertexes : AvfxBase {
        public readonly List<AvfxEmitVertex> EmitVertexes = new();

        public AvfxEmitVertexes() : base( "VEmt" ) { }

        public override void ReadContents( BinaryReader reader, int size ) {
            for( var i = 0; i < size / 28; i++ ) EmitVertexes.Add( new AvfxEmitVertex( reader ) );
        }

        protected override void RecurseChildrenAssigned( bool assigned ) { }

        public override void WriteContents( BinaryWriter writer ) {
            foreach( var vert in EmitVertexes ) vert.Write( writer );
        }
    }

    public class AvfxEmitVertex {
        public readonly ParsedFloat3 Position = new( "位置" );
        public readonly ParsedFloat3 Normal = new( "普通" );
        public readonly ParsedIntColor Color = new( "颜色" );

        public AvfxEmitVertex() { }

        public AvfxEmitVertex( BinaryReader reader ) {
            Position.Read( reader );
            Normal.Read( reader );
            Color.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Position.Write( writer );
            Normal.Write( writer );
            Color.Write( writer );
        }
    }
}