using OtterGui.Raii;
using System;
using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    [Flags]
    public enum AnimationFlags {
        TimeControlEnabled = 0x01,
        Unknown_2 = 0x02,
        Unknown_3 = 0x04,
        Unknown_4 = 0x08,
        Unknown_5 = 0x10,
        Unknown_6 = 0x20,
        Unknown_7 = 0x40,
        Unknown_8 = 0x80
    }

    public class C010 : TmbEntry {
        public const string MAGIC = "C010";
        public const string DISPLAY_NAME = "动画";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x28;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间", defaultValue: 50 );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedFlag<AnimationFlags> Flags = new( "标识" );
        private readonly ParsedFloat AnimationStart = new( "动画起始帧" );
        private readonly ParsedFloat AnimationEnd = new( "动画结束帧" );
        private readonly TmbOffsetString Path = new( "路径" );
        private readonly ParsedInt Unk5 = new( "未知 1" );

        public C010( TmbFile file ) : base( file ) { }

        public C010( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Flags,
            AnimationStart,
            AnimationEnd,
            Path,
            Unk5
        };

        public override void DrawBody() {
            DrawHeader();

            Flags.Draw( Command );

            using( var disabled = ImRaii.Disabled( !Flags.HasFlag( AnimationFlags.TimeControlEnabled ) ) ) {
                Duration.Draw( Command );
                AnimationStart.Draw( Command );
                AnimationEnd.Draw( Command );
            }

            Unk1.Draw( Command );
            Path.Draw( Command );
            Unk5.Draw( Command );
        }
    }
}
