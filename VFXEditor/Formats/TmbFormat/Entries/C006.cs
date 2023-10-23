using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C006 : TmbEntry {
        public const string MAGIC = "C006";
        public const string DISPLAY_NAME = "漂浮文本设置";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x18;
        public override int ExtraSize => 0;

        private readonly ParsedBool Enabled = new( "启用" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3" );

        public C006( TmbFile file ) : base( file ) { }

        public C006( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Enabled,
            Unk2,
            Unk3
        };
    }
}
