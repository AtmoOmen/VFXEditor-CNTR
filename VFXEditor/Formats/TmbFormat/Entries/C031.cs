using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Sheets;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C031 : TmbEntry {
        public const string MAGIC = "C031";
        public const string DISPLAY_NAME = "Summon Animation";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x18;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedWeaponTimeline Animation = new( "动画" );
        private readonly ParsedEnum<ObjectControl> TargetType = new( "目标类型", size: 2 );

        public C031( TmbFile file ) : base( file ) { }

        public C031( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Animation,
            TargetType
        };
    }
}
