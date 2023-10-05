using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C192 : TmbEntry {
        public const string MAGIC = "C192";
        public const string DISPLAY_NAME = "Voiceline";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;
        public override DangerLevel Danger => DangerLevel.Yellow;

        public override int Size => 0x38;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt VoicelineNumber = new( "语音台词编号" );
        private readonly ParsedInt Unk4 = new( "未知 4" );
        private readonly ParsedInt Unk5 = new( "未知 5" );
        private readonly ParsedInt Unk6 = new( "未知 6" );
        private readonly ParsedInt Unk7 = new( "未知 7" );
        private readonly ParsedInt Unk8 = new( "未知 8" );
        private readonly ParsedInt Unk9 = new( "未知 9" );
        private readonly ParsedInt Unk10 = new( "未知 10" );
        private readonly ParsedInt Unk11 = new( "未知 11" );

        public C192( TmbFile file ) : base( file ) { }

        public C192( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Unk1,
            Unk2,
            VoicelineNumber,
            Unk4,
            Unk5,
            Unk6,
            Unk7,
            Unk8,
            Unk9,
            Unk10,
            Unk11
        };
    }
}
