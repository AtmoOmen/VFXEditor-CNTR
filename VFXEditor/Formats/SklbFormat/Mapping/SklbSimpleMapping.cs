﻿using FFXIVClientStructs.Havok;
using VfxEditor.Interop.Structs.Animation;
using VfxEditor.Parsing;
using VfxEditor.SklbFormat.Bones;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.SklbFormat.Mapping {
    public unsafe class SklbSimpleMapping : IUiItem {
        public readonly SklbMapping Mapping;

        public readonly ParsedBoneIndex BoneA = new( "骨骼节点映射", -1 );
        public readonly ParsedBoneIndex BoneB = new( "该骨骼节点", -1 );
        public readonly ParsedInt Unk1 = new( "未知 1" );
        public readonly ParsedInt Unk2 = new( "未知 2" );
        public readonly ParsedInt Unk3 = new( "未知 3" );
        public readonly ParsedFloat4 Translation = new( "平移" );
        public readonly ParsedFloat4 Rotation = new( "旋转", new( 0, 0, 0, 1 ) );
        public readonly ParsedFloat4 Scale = new( "缩放", new( 1, 1, 1, 1 ) );

        public SklbSimpleMapping( SklbMapping mapping ) {
            Mapping = mapping;
        }

        public SklbSimpleMapping( SklbMapping mapping, SimpleMapping simpleMapping ) : this( mapping ) {
            BoneA.Value = simpleMapping.BoneA;
            BoneB.Value = simpleMapping.BoneB;
            Unk1.Value = simpleMapping.Unk1;
            Unk2.Value = simpleMapping.Unk2;
            Unk3.Value = simpleMapping.Unk3;

            var transform = simpleMapping.AFromBTransform;
            var pos = transform.Translation;
            var rot = transform.Rotation;
            var scale = transform.Scale;

            Translation.Value = new( pos.X, pos.Y, pos.Z, pos.W );
            Rotation.Value = new( rot.X, rot.Y, rot.Z, rot.W );
            Scale.Value = new( scale.X, scale.Y, scale.Z, scale.W );
        }

        public string GetText() => $"{BoneA.GetText( Mapping.SkeletonA )} => {BoneB.GetText( Mapping.Bones.Bones )}";

        public void Draw() {
            BoneA.Draw( Mapping.SkeletonA );
            BoneB.Draw( Mapping.Bones.Bones );

            Unk1.Draw( CommandManager.Sklb );
            Unk2.Draw( CommandManager.Sklb );
            Unk3.Draw( CommandManager.Sklb );
            Translation.Draw( CommandManager.Sklb );
            Rotation.Draw( CommandManager.Sklb );
            Scale.Draw( CommandManager.Sklb );
        }

        public SimpleMapping ToHavok() {
            var transform = new hkQsTransformf();

            var pos = new hkVector4f {
                X = Translation.Value.X,
                Y = Translation.Value.Y,
                Z = Translation.Value.Z,
                W = Translation.Value.W
            };
            transform.Translation = pos;

            var rot = new hkQuaternionf {
                X = Rotation.Value.X,
                Y = Rotation.Value.Y,
                Z = Rotation.Value.Z,
                W = Rotation.Value.W
            };
            transform.Rotation = rot;

            var scl = new hkVector4f {
                X = Scale.Value.X,
                Y = Scale.Value.Y,
                Z = Scale.Value.Z,
                W = Scale.Value.W
            };
            transform.Scale = scl;

            var simpleMapping = new SimpleMapping() {
                BoneA = ( short )BoneA.Value,
                BoneB = ( short )BoneB.Value,
                Unk1 = Unk1.Value,
                Unk2 = Unk2.Value,
                Unk3 = Unk3.Value,
                AFromBTransform = transform
            };
            return simpleMapping;
        }
    }
}
