using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.PhybFormat.Simulator.Spring {
    public class PhybSpring : PhybData, IPhysicsObject {
        public readonly PhybSimulator Simulator;

        public readonly ParsedShort ChainId1 = new( "链 ID 1" );
        public readonly ParsedShort ChainId2 = new( "链 ID 2" );
        public readonly ParsedShort NodeId1 = new( "节点 ID 1" );
        public readonly ParsedShort NodeId2 = new( "节点 ID 2" );
        public readonly ParsedFloat StretchStiffness = new( "延申刚度" );
        public readonly ParsedFloat CompressStiffness = new( "压缩刚度" );

        public PhybSpring( PhybFile file, PhybSimulator simulator ) : base( file ) {
            Simulator = simulator;
        }

        public PhybSpring( PhybFile file, PhybSimulator simulator, BinaryReader reader ) : base( file, reader ) {
            Simulator = simulator;
        }

        protected override List<ParsedBase> GetParsed() => new() {
            ChainId1,
            ChainId2,
            NodeId1,
            NodeId2,
            StretchStiffness,
            CompressStiffness,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            Simulator.ConnectNodes( ChainId1.Value, ChainId2.Value, NodeId1.Value, NodeId2.Value, 0.02f, meshes.Spring, boneMatrixes );
        }
    }
}
