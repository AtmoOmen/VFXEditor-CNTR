using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Animations;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Collision.Capsule {
    public class PhybCapsule : PhybData, IPhysicsObject {
        public readonly ParsedPaddedString Name = new( "名称", "replace_me", 32, 0xFE );
        public readonly ParsedPaddedString StartBone = new( "起始骨骼", 32, 0xFE );
        public readonly ParsedPaddedString EndBone = new( "结束骨骼", 32, 0xFE );
        public readonly ParsedFloat3 StartOffset = new( "起始偏移" );
        public readonly ParsedFloat3 EndOffset = new( "结束偏移" );
        public readonly ParsedFloat Radius = new( "半径" );

        public PhybCapsule( PhybFile file ) : base( file ) { }

        public PhybCapsule( PhybFile file, BinaryReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Name,
            StartBone,
            EndBone,
            StartOffset,
            EndOffset,
            Radius,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            if( !boneMatrixes.TryGetValue( StartBone.Value, out var startBone ) ) return;
            if( !boneMatrixes.TryGetValue( EndBone.Value, out var endBone ) ) return;

            var startOffset = new Vector3( StartOffset.Value.X, StartOffset.Value.Y, StartOffset.Value.Z );
            var endOffset = new Vector3( EndOffset.Value.X, EndOffset.Value.Y, EndOffset.Value.Z );

            var startPos = Vector3.Transform( startOffset, startBone.BindPose ).ToVector3();
            var endPos = Vector3.Transform( endOffset, endBone.BindPose ).ToVector3();

            meshes.Collision.AddCylinder( startPos, endPos, Radius.Value * 2f, 10 );
            meshes.Collision.AddSphere( startPos, Radius.Value, 10, 10 );
            meshes.Collision.AddSphere( endPos, Radius.Value, 10, 10 );
        }
    }
}
