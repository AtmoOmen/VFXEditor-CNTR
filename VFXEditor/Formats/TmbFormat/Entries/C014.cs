using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C014 : TmbEntry {
        public const string MAGIC = "C014";
        public const string DISPLAY_NAME = "武器位置";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x1C;
        public override int ExtraSize => 0;

        private readonly ParsedBool Enabled = new( "启用" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedEnum<ObjectControlPosition> ObjectPosition = new( "物体位置" );
        private readonly ParsedEnum<ObjectControl> ObjectControl = new( "物体控制" );

        public C014( TmbFile file ) : base( file ) { }

        public C014( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Enabled,
            Unk2,
            ObjectPosition,
            ObjectControl
        };
    }
}
