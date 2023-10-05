using HelixToolkit.SharpDX.Core.Animations;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Parsing.String;

namespace VfxEditor.PhybFormat.Simulator.Chain {
    public class PhybNode : PhybData, IPhysicsObject {
        public readonly PhybSimulator Simulator;

        public readonly ParsedPaddedString BoneName = new( "骨骼名", 32, 0xFE );
        public readonly ParsedFloat Radius = new( "碰撞半径" );
        public readonly ParsedFloat AttractByAnimation = new( "以动画吸引" );
        public readonly ParsedFloat WindScale = new( "风力缩放" );
        public readonly ParsedFloat GravityScale = new( "重力缩放" );
        public readonly ParsedFloat ConeMaxAngle = new( "锥体最大角度" );
        public readonly ParsedFloat3 ConeAxisOffset = new( "锥体轴偏移" );
        public readonly ParsedFloat3 ConstraintPlaneNormal = new( "约束平面法线" );
        public readonly ParsedUInt CollisionFlag = new( "碰撞标志" );
        public readonly ParsedUInt ContinuousCollisionFlag = new( "持续碰撞标志" );

        public PhybNode( PhybFile file, PhybSimulator simulator ) : base( file ) {
            Simulator = simulator;
        }

        public PhybNode( PhybFile file, PhybSimulator simulator, BinaryReader reader ) : base( file, reader ) {
            Simulator = simulator;
        }

        protected override List<ParsedBase> GetParsed() => new() {
            BoneName,
            Radius,
            AttractByAnimation,
            WindScale,
            GravityScale,
            ConeMaxAngle,
            ConeAxisOffset,
            ConstraintPlaneNormal,
            CollisionFlag,
            ContinuousCollisionFlag,
        };

        public void AddPhysicsObjects( MeshBuilders meshes, Dictionary<string, Bone> boneMatrixes ) {

        }
    }
}
