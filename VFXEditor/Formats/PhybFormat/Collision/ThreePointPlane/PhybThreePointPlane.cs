using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Collision.ThreePointPlane {
    public class PhybThreePointPlane : PhybData, IPhysicsObject {
        public readonly ParsedPaddedString Name = new( "名称", "replace_me", 32, 0xFE );
        public readonly ParsedPaddedString Bone = new( "骨骼", 32, 0xFE );
        public readonly ParsedFloat4 Unknown1 = new( "未知 1" );
        public readonly ParsedFloat4 Unknown2 = new( "未知 2" );
        public readonly ParsedFloat4 Unknown3 = new( "未知 3" );
        public readonly ParsedFloat4 Unknown4 = new( "未知 4" );
        public readonly ParsedFloat3 BoneOffset = new( "骨骼偏移" );
        public readonly ParsedFloat3 Unknown5 = new( "未知 5" );
        public readonly ParsedFloat3 Unknown6 = new( "未知 6" );
        public readonly ParsedFloat Thickness = new( "厚度" );

        public PhybThreePointPlane( PhybFile file ) : base( file ) { }

        public PhybThreePointPlane( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Name,
            Bone,
            Unknown1,
            Unknown2,
            Unknown3,
            Unknown4,
            BoneOffset,
            Unknown5,
            Unknown6,
            Thickness,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {

        }
    }
}
