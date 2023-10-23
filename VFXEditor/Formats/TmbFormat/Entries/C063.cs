using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C063 : TmbEntry {
        public const string MAGIC = "C063";
        public const string DISPLAY_NAME = "音效";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x20;
        public override int ExtraSize => 0;

        private readonly ParsedInt Loop = new( "循环", defaultValue: 1 );
        private readonly ParsedInt Interrupt = new( "打断" );
        private readonly TmbOffsetString Path = new( "路径" );
        private readonly ParsedInt SoundIndex = new( "音频索引" );
        private readonly ParsedInt SoundPosition = new( "音频位置", defaultValue: 1 );

        public C063( TmbFile file ) : base( file ) { }

        public C063( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Loop,
            Interrupt,
            Path,
            SoundIndex,
            SoundPosition
        };
    }
}
