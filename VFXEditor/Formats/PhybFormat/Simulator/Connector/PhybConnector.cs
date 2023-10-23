using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.PhybFormat.Simulator.Connector {
    public class PhybConnector : PhybData, IPhysicsObject {
        public readonly PhybSimulator Simulator;

        public readonly ParsedShort ChainId1 = new( "链 ID 1" );
        public readonly ParsedShort ChainId2 = new( "链 ID 2" );
        public readonly ParsedShort NodeId1 = new( "节点 ID 1" );
        public readonly ParsedShort NodeId2 = new( "节点 ID 2" );
        public readonly ParsedFloat CollisionRadius = new( "碰撞半径" );
        public readonly ParsedFloat Friction = new( "摩擦" );
        public readonly ParsedFloat Dampening = new( "阻力" );
        public readonly ParsedFloat Repulsion = new( "斥力" );
        public readonly ParsedUInt CollisionFlag = new( "碰撞标志" );
        public readonly ParsedUInt ContinuousCollisionFlag = new( "持续碰撞标志" );

        public PhybConnector( PhybFile file, PhybSimulator simulator ) : base( file ) {
            Simulator = simulator;
        }

        public PhybConnector( PhybFile file, PhybSimulator simulator, BinaryReader reader ) : base( file, reader ) {
            Simulator = simulator;
        }

        protected override List<ParsedBase> GetParsed() => new() {
            ChainId1,
            ChainId2,
            NodeId1,
            NodeId2,
            CollisionRadius,
            Friction,
            Dampening,
            Repulsion,
            CollisionFlag,
            ContinuousCollisionFlag,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {
            Simulator.ConnectNodes( ChainId1.Value, ChainId2.Value, NodeId1.Value, NodeId2.Value, CollisionRadius.Value, meshes.Simulation, boneMatrixes );
        }
    }
}
