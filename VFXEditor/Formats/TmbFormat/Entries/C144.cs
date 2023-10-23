using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C144 : TmbEntry {
        public const string MAGIC = "C144";
        public const string DISPLAY_NAME = "";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x2C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3" );
        private readonly ParsedFloat Unk4 = new( "未知 4" );
        private readonly ParsedFloat Unk5 = new( "未知 5" );
        private readonly ParsedFloat Unk6 = new( "未知 6" );
        private readonly ParsedFloat Unk7 = new( "未知 7" );
        private readonly ParsedFloat Unk8 = new( "未知 8" );

        public C144( TmbFile file ) : base( file ) { }

        public C144( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Unk1,
            Unk2,
            Unk3,
            Unk4,
            Unk5,
            Unk6,
            Unk7,
            Unk8
        };
    }
}
