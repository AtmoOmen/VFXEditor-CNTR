using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public class C075 : TmbEntry {
        public const string MAGIC = "C075";
        public const string DISPLAY_NAME = "地形视效";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x40;
        public override int ExtraSize => 4 * ( 3 + 3 + 3 + 4 );

        private readonly ParsedBool Enabled = new( "启用" );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly ParsedInt Shape = new( "形状" );
        private readonly TmbOffsetFloat3 Scale = new( "缩放", defaultValue: new( 1 ) );
        private readonly TmbOffsetAngle3 Rotation = new( "旋转" );
        private readonly TmbOffsetFloat3 Position = new( "位置" );
        private readonly TmbOffsetFloat4 RGBA = new( "RGBA", defaultValue: new( 1 ) );
        private readonly ParsedInt Unk3 = new( "未知 3" );
        private readonly ParsedInt Unk4 = new( "未知 4" );

        public C075( TmbFile file ) : base( file ) { }

        public C075( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Enabled,
            Unk1,
            Shape,
            Scale,
            Rotation,
            Position,
            RGBA,
            Unk3,
            Unk4
        };
    }
}
