using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTextureDistortion : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxBool TargetUv1 = new( "UV 扭曲 1", "bT1" );
        public readonly AvfxBool TargetUv2 = new( "UV 扭曲 2", "bT2" );
        public readonly AvfxBool TargetUv3 = new( "UV 扭曲 3", "bT3" );
        public readonly AvfxBool TargetUv4 = new( "UV 扭曲 4", "bT4" );
        public readonly AvfxInt UvSetIdx = new( "平面坐标集索引", "UvSN" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderU = new( "水平材质边界", "TBUT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorderV = new( "垂直材质边界", "TBVT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", defaultValue: -1 );
        public readonly AvfxCurve DPow = new( "强度", "DPow" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTextureDistortion( AvfxParticle particle ) : base( "TD", particle ) {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = new() {
                Enabled,
                TargetUv1,
                TargetUv2,
                TargetUv3,
                TargetUv4,
                UvSetIdx,
                TextureFilter,
                TextureBorderU,
                TextureBorderV,
                TextureIdx,
                DPow
            };

            Display.Add( Enabled );
            Display.Add( TargetUv1 );
            Display.Add( TargetUv2 );
            Display.Add( TargetUv3 );
            Display.Add( TargetUv4 );
            Display.Add( UvSetIdx );
            Display.Add( TextureFilter );
            Display.Add( TextureBorderU );
            Display.Add( TextureBorderV );

            DisplayTabs.Add( DPow );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            ReadNested( reader, Parsed, size );
            EnableAllSelectors();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        protected override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( "TD" );

            AssignedCopyPaste( this, GetDefaultText() );
            if( ImGui.SmallButton( "+ 材质扭曲" ) ) Assign();
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( "TD" );

            AssignedCopyPaste( this, GetDefaultText() );
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => "材质扭曲";

        public override List<UiNodeSelect> GetNodeSelects() => new() {
            new UiNodeSelect<AvfxTexture>( Particle, "材质", Particle.NodeGroups.Textures, TextureIdx )
        };
    }
}
