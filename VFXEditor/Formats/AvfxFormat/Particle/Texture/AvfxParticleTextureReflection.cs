using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTextureReflection : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxBool UseScreenCopy = new( "Use Screen Copy", "bUSC" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureCalculateColor> TextureCalculateColorType = new( "Calculate Color", "TCCT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", value: -1 );
        public readonly AvfxCurve Rate = new( "Rate", "Rate" );
        public readonly AvfxCurve RPow = new( "强度", "RPow" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTextureReflection( AvfxParticle particle ) : base( "TR", particle ) {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = new() {
                Enabled,
                UseScreenCopy,
                TextureFilter,
                TextureCalculateColorType,
                TextureIdx,
                Rate,
                RPow
            };

            Display.Add( Enabled );
            Display.Add( UseScreenCopy );
            Display.Add( TextureFilter );
            Display.Add( TextureCalculateColorType );

            DisplayTabs.Add( Rate );
            DisplayTabs.Add( RPow );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            ReadNested( reader, Parsed, size );
            EnableAllSelectors();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( "TR" );

            AssignedCopyPaste( this, GetDefaultText() );
            if( ImGui.SmallButton( "+ Texture Reflection" ) ) Assign();
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( "TR" );

            AssignedCopyPaste( this, GetDefaultText() );
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => "Texture Reflection";

        public override List<AvfxNodeSelect> GetNodeSelects() => new() {
            new AvfxNodeSelect<AvfxTexture>( Particle, "材质", Particle.NodeGroups.Textures, TextureIdx )
        };
    }
}
