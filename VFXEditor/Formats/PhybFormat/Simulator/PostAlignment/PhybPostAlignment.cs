using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Simulator.PostAlignment {
    public class PhybPostAlignment : PhybData, IPhysicsObject {
        public readonly PhybSimulator Simulator;

        public readonly ParsedPaddedString CollisionName = new( "碰撞名称", 32, 0xFE );
        public readonly ParsedShort ChainId = new( "链 ID" );
        public readonly ParsedShort NodeId = new( "节点 ID" );

        public PhybPostAlignment( PhybFile file, PhybSimulator simulator ) : base( file ) {
            Simulator = simulator;
        }

        public PhybPostAlignment( PhybFile file, PhybSimulator simulator, BinaryReader reader ) : base( file, reader ) {
            Simulator = simulator;
        }

        protected override List<ParsedBase> GetParsed() => new() {
            CollisionName,
            ChainId,
            NodeId,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {

        }
    }
}
