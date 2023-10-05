using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    [Flags]
    public enum InvisibilityFilter {
        Character = 0x01,
        Weapon = 0x02,
        OffHand = 0x04,
        Summon = 0x08,
        Unknown_1 = 0x10,
        Unknown_2 = 0x20,
        Unknown_3 = 0x40,
        Unknown_4 = 0x80,
    }

    public class C094 : TmbEntry {
        public const string MAGIC = "C094";
        public const string DISPLAY_NAME = "Invisibility";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x20;
        public override int ExtraSize => ExtraData ? 0x14 : 0;

        private bool ExtraData = true;

        private readonly ParsedInt FadeTime = new( "淡化时间" );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedFloat StartVisibility = new( "可见性开始" );
        private readonly ParsedFloat EndVisibility = new( "可见性结束" );

        // these are parsed separately
        private readonly ParsedBool EnableFilter = new( "启用筛选器" );
        private readonly ParsedFlag<InvisibilityFilter> Filter = new( "筛选器" );
        private readonly ParsedInt Unk4 = new( "未知 4" );
        private readonly ParsedInt Unk5 = new( "未知 5" );
        private readonly ParsedInt Unk6 = new( "未知 6" );

        // Unk2 = 1, Unk3 = 8 -> ExtraSize = 0x14

        public C094( TmbFile file ) : base( file ) { }

        public C094( TmbFile file, TmbReader reader ) : base( file, reader ) {
            ExtraData = reader.ReadAtOffset( ( binaryReader ) => {
                EnableFilter.Read( binaryReader );
                Filter.Read( binaryReader );
                Unk4.Read( binaryReader );
                Unk5.Read( binaryReader );
                Unk6.Read( binaryReader );
            } );
        }

        protected override List<ParsedBase> GetParsed() => new() {
            FadeTime,
            Unk1,
            StartVisibility,
            EndVisibility
        };

        public override void Write( TmbWriter writer ) {
            base.Write( writer );

            writer.WriteExtra( ( binaryWriter ) => {
                EnableFilter.Write( binaryWriter );
                Filter.Write( binaryWriter );
                Unk4.Write( binaryWriter );
                Unk5.Write( binaryWriter );
                Unk6.Write( binaryWriter );
            }, ExtraData );
        }

        public override void DrawBody() {
            base.DrawBody();

            ImGui.Checkbox( "额外数据", ref ExtraData );
            if( ExtraData ) {
                EnableFilter.Draw( Command );

                using( var disabled = ImRaii.Disabled( !EnableFilter.Value ) )
                using( var indent = ImRaii.PushIndent() ) {
                    Filter.Draw( Command );
                }

                Unk4.Draw( Command );
                Unk5.Draw( Command );
                Unk6.Draw( Command );
            }
        }
    }
}