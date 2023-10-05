using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C215 : TmbEntry {
        public const string MAGIC = "C215";
        public const string DISPLAY_NAME = "";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3" );
        private readonly ParsedInt Unk4 = new( "未知 4" );

        public C215( TmbFile file ) : base( file ) { }

        public C215( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Unk1,
            Unk2,
            Unk3,
            Unk4
        };
    }
}
