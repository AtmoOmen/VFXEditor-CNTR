using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Collision.Ellipsoid {
    public class PhybEllipsoid : PhybData, IPhysicsObject {
        public readonly ParsedPaddedString Name = new( "名称", "replace_me", 32, 0xFE );
        public readonly ParsedPaddedString Bone = new( "骨骼", 32, 0xFE );
        public readonly ParsedFloat3 Offset1 = new( "骨骼偏移 1" );
        public readonly ParsedFloat3 Offset2 = new( "骨骼偏移 2" );
        public readonly ParsedFloat3 Offset3 = new( "骨骼偏移 3" );
        public readonly ParsedFloat3 Offset4 = new( "骨骼偏移 4" );
        public readonly ParsedFloat Radius = new( "半径" );

        public PhybEllipsoid( PhybFile file ) : base( file ) { }

        public PhybEllipsoid( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Name,
            Bone,
            Offset1,
            Offset2,
            Offset3,
            Offset4,
            Radius,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {

        }
    }
}
