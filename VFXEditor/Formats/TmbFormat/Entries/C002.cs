using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C002 : TmbEntry {
        public const string MAGIC = "C002";
        public const string DISPLAY_NAME = "TMB";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间", defaultValue: 50 );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly TmbOffsetString Path = new( "路径" );

        public C002( TmbFile file ) : base( file ) { }

        public C002( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Unk2,
            Path
        };
    }
}
