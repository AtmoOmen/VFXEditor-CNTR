using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.ScdFormat {
    public enum SelfCommand {
        None,
        FadeIn,
        ChgPriority,
        FreezePan,
        Stay,
        ChgPan,
        BanRear,
        NoPlay,
        ChgDepth,
        MonoSpeaker,
        ChgVolume,
        ChgBusNo,
        ChgBusVolume
    }

    public enum TargetCommand {
        None,
        Stop,
        ChgPriority,
        FadeOut,
        ChgPitch,
        PriorityStop,
        ChgPan,
        ChgVolume,
        OldStop,
        BanRear,
        ChgDepth,
        ChgBusNo,
        LowExternalIdOldStop,
        MinVolumeStop
    }

    public class AttributeResultCommand {
        public readonly ParsedEnum<SelfCommand> SelfCommandSelect = new( "选择命令", size: 1 );
        public readonly ParsedEnum<TargetCommand> TargetCommandSelect = new( "目标命令", size: 1 );
        public readonly ParsedReserve Reserved1 = new( 2 );
        public readonly ParsedInt SelfArgument = new( "自引用参数" );
        public readonly ParsedInt TargetArgument = new( "目标参数" );

        public AttributeResultCommand() { }

        public void Read( BinaryReader reader ) {
            SelfCommandSelect.Read( reader );
            TargetCommandSelect.Read( reader );
            Reserved1.Read( reader );
            SelfArgument.Read( reader );
            TargetArgument.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            SelfCommandSelect.Write( writer );
            TargetCommandSelect.Write( writer );
            Reserved1.Write( writer );
            SelfArgument.Write( writer );
            TargetArgument.Write( writer );
        }

        public void Draw() {
            SelfCommandSelect.Draw( CommandManager.Scd );
            TargetArgument.Draw( CommandManager.Scd );
            SelfArgument.Draw( CommandManager.Scd );
            TargetArgument.Draw( CommandManager.Scd );
        }
    }
}
