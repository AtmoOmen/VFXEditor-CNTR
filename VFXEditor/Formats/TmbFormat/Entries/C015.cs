using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C015 : TmbEntry {
        public const string MAGIC = "C015";
        public const string DISPLAY_NAME = "Weapon Size";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt WeaponSize = new( "大小" );
        private readonly ParsedEnum<ObjectControl> ObjectControl = new( "物体控制" );

        public C015( TmbFile file ) : base( file ) { }

        public C015( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Unk1,
            Unk2,
            WeaponSize,
            ObjectControl
        };
    }
}
