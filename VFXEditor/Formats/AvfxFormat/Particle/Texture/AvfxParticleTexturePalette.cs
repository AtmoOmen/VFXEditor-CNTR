using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Particle.Texture;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleTexturePalette : AvfxParticleAttribute {
        public readonly AvfxBool Enabled = new( "启用", "bEna" );
        public readonly AvfxEnum<TextureFilterType> TextureFilter = new( "材质筛选器", "TFT" );
        public readonly AvfxEnum<TextureBorderType> TextureBorder = new( "Texture Border", "TBT" );
        public readonly AvfxInt TextureIdx = new( "材质索引", "TxNo", value: -1 );
        public readonly AvfxCurve Offset = new( "偏移", "POff" );

        private readonly List<AvfxBase> Parsed;

        public AvfxParticleTexturePalette( AvfxParticle particle ) : base( "TP", particle ) {
            InitNodeSelects();
            Display.Add( new TextureNodeSelectDraw( NodeSelects ) );

            Parsed = new() {
                Enabled,
                TextureFilter,
                TextureBorder,
                TextureIdx,
                Offset
            };

            Display.Add( Enabled );
            Display.Add( TextureFilter );
            Display.Add( TextureBorder );

            DisplayTabs.Add( Offset );
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            ReadNested( reader, Parsed, size );
            EnableAllSelectors();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        public override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void DrawUnassigned() {
            using var _ = ImRaii.PushId( "TP" );

            AssignedCopyPaste( this, GetDefaultText() );
            if( ImGui.SmallButton( "+ Texture Palette" ) ) Assign();
        }

        public override void DrawAssigned() {
            using var _ = ImRaii.PushId( "TP" );

            AssignedCopyPaste( this, GetDefaultText() );
            DrawNamedItems( DisplayTabs );
        }

        public override string GetDefaultText() => "Texture Palette";

        public override List<AvfxNodeSelect> GetNodeSelects() => new() {
            new AvfxNodeSelect<AvfxTexture>( Particle, "材质", Particle.NodeGroups.Textures, TextureIdx )
        };
    }
}
