using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C093 : TmbEntry {
        public const string MAGIC = "C093";
        public const string DISPLAY_NAME = "颜色";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x28;
        public override int ExtraSize => 4 * ( 4 + 4 );

        private readonly ParsedInt Duration = new( "持续时间", value: 30 );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly TmbOffsetFloat4 Color1 = new( "颜色 1", defaultValue: new( 1 ) );
        private readonly TmbOffsetFloat4 Color2 = new( "颜色 2", defaultValue: new( 1 ) );
        private readonly ParsedInt Unk4 = new( "未知 4" );

        public C093( TmbFile file ) : base( file ) { }

        public C093( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Color1,
            Color2,
            Unk4
        };
    }
}
