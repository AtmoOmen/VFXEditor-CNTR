using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum SummonWeaponObjectControl {
        Weapon = 0,
        OffHand = 1,
    }

    public class C203 : TmbEntry {
        public const string MAGIC = "C203";
        public const string DISPLAY_NAME = "Summon Weapon Visibility";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x2C;
        public override int ExtraSize => 0;

        private readonly ParsedInt Unk1 = new( "未知 1" ); // chara/action/magic/2ff_sage/mgc007.tmb => 48
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedInt Unk3 = new( "未知 3" );
        private readonly ParsedInt Unk4 = new( "未知 4" );
        private readonly ParsedEnum<SummonWeaponObjectControl> ObjectControl = new( "物体控制" );
        private readonly ParsedInt Unk6 = new( "未知 6" );
        private readonly ParsedInt Unk7 = new( "未知 7" );
        private readonly ParsedFloat Unk8 = new( "未知 8" );

        public C203( TmbFile file ) : base( file ) { }

        public C203( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Unk1,
            Unk2,
            Unk3,
            Unk4,
            ObjectControl,
            Unk6,
            Unk7,
            Unk8
        };
    }
}
