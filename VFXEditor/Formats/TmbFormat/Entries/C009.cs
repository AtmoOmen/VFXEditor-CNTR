using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C009 : TmbEntry {
        public const string MAGIC = "C009";
        public const string DISPLAY_NAME = "Animation (PAP Only)";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x18;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间", defaultValue: 50 );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly TmbOffsetString Path = new( "路径" );

        public C009( TmbFile file ) : base( file ) { }

        public C009( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Path
        };
    }
}
