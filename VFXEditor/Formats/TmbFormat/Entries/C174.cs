using System.Collections.Generic;
using VfxEditor.Parsing;
using VfxEditor.TmbFormat.Utils;

namespace VfxEditor.TmbFormat.Entries {
    public enum ObjectControlPosition {
        Stowed = 0,
        RightHand = 1,
        Unknown = 2,
        SwitchHand = 3,
        RootBone = 4,
        SwitchReverse = 5
    }

    public enum ObjectControlFinal {
        Stowed = 0,
        Drawn = 1,
        Weapon = 2,
        OffHand = 3,
        RootBone = 4,
        UncontrolledRoot = 5,
        Original = 6,
    }

    public enum ObjectControl {
        Weapon_or_Pet = 0,
        OffHand = 1,
        Lemure_or_Summon = 2
    }

    public class C174 : TmbEntry {
        public const string MAGIC = "C174";
        public const string DISPLAY_NAME = "物体控制";
        public override string DisplayName => DISPLAY_NAME;
        public override string Magic => MAGIC;

        public override int Size => 0x28;
        public override int ExtraSize => 0;

        private readonly ParsedInt Duration = new( "持续时间" );
        private readonly ParsedInt Unk2 = new( "未知 2" );
        private readonly ParsedEnum<ObjectControlPosition> ObjectPosition = new( "物体位置" );
        private readonly ParsedEnum<ObjectControl> ObjectControl = new( "物体控制" );
        private readonly ParsedEnum<ObjectControlFinal> FinalPosition = new( "最终位置" );
        private readonly ParsedInt PositionDelay = new( "位置延迟" );
        private readonly ParsedInt Unk6 = new( "未知 6" );

        public C174( TmbFile file ) : base( file ) { }

        public C174( TmbFile file, TmbReader reader ) : base( file, reader ) { }

        protected override List<ParsedBase> GetParsed() => new() {
            Duration,
            Unk2,
            ObjectPosition,
            ObjectControl,
            FinalPosition,
            PositionDelay,
            Unk6
        };
    }
}
