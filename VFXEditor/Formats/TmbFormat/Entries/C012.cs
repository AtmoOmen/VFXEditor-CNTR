using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum VfxVisibility {
        Unknown_0 = 0,
        Unknown_1 = 1,
        Everyone = 2,
        Unknown_3 = 3,
    }

    public class C012 : TmbEntry {
        public const string MAGIC = "C012";
        public const string DISPLAY_NAME = "视效";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x48;
        public override int ExtraSize => 4 * ( 3 + 3 + 3 + 4 );

        private readonly ParsedInt Duration = new( "持续时间", defaultValue: 30 );
        private readonly ParsedInt Unk1 = new( "未知 1" );
        private readonly TmbOffsetString Path = new( "路径" );
        private readonly ParsedShort BindPoint1 = new( "绑定点 1", defaultValue: 1 );
        private readonly ParsedShort BindPoint2 = new( "绑定点 2", defaultValue: 0xFF );
        private readonly ParsedShort BindPoint3 = new( "绑定点 3", defaultValue: 2 );
        private readonly ParsedShort BindPoint4 = new( "绑定点 4", defaultValue: 0xFF );
        private readonly TmbOffsetFloat3 Scale = new( "缩放", defaultValue: new( 1 ) );
        private readonly TmbOffsetAngle3 Rotation = new( "旋转" );
        private readonly TmbOffsetFloat3 Position = new( "位置" );
        private readonly TmbOffsetFloat4 RGBA = new( "RGBA", defaultValue: new( 1 ) );
        private readonly ParsedEnum<VfxVisibility> Visibility = new( "可见性" );
        private readonly ParsedInt Unk3 = new( "未知 3" );

        public C012( TmbFile file ) : base( file ) { }

        public C012( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk1,
            Path,
            BindPoint1,
            BindPoint2,
            BindPoint3,
            BindPoint4,
            Scale,
            Rotation,
            Position,
            RGBA,
            Visibility,
            Unk3
        };
    }
}
