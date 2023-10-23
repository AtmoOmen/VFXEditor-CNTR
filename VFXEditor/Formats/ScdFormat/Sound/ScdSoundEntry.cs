﻿using ImGuiNET;
using OtterGui.Raii;
using System;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.ScdFormat.Sound.Data;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.ScdFormat {
    public enum SoundType {
        Invalid = 0,
        Normal = 1,
        Random,
        Stereo,
        Cycle,
        Order,
        FourChannelSurround,
        Engine,
        Dialog,
        FixedPosition = 10,
        DynamixStream,
        GroupRandom,
        GroupOrder,
        Atomosgear,
        ConditionalJump,
        Empty,
        MidiMusic = 128
    }

    [Flags]
    public enum SoundAttribute {
        Invalid = 0,
        Loop = 0x0001,
        Reverb = 0x0002,
        FixedVolume = 0x0004,
        FixedPosition = 0x0008,
        Music = 0x0020,
        BypassPLIIz = 0x0040,
        UseExternalAttr = 0x0080,
        ExistRoutingSetting = 0x0100,
        MusicSurround = 0x0200,
        BusDucking = 0x0400,
        Acceleration = 0x0800,
        DynamixEnd = 0x1000,
        ExtraDesc = 0x2000,
        DynamixPlus = 0x4000,
        Atomosgear = 0x8000,
    }

    public class ScdSoundEntry : ScdEntry, IUiItem {
        public readonly ParsedByte BusNumber = new( "总线编号" );
        public readonly ParsedByte Priority = new( "优先级" );
        public readonly ParsedEnum<SoundType> Type = new( "Type", size: 1 );
        public readonly ParsedFlag<SoundAttribute> Attributes = new( "属性" );
        public readonly ParsedFloat Volume = new( "音量" );
        public readonly ParsedShort LocalNumber = new( "本地编号" ); // TODO: ushort
        public readonly ParsedByte UserId = new( "用户 ID" );
        public readonly ParsedByte PlayHistory = new( "播放历史" ); // TODO: sbyte

        public readonly SoundRoutingInfo RoutingInfo = new(); // include sendInfos, soundEffectParam
        public SoundBusDucking BusDucking = new();
        public SoundAcceleration Acceleration = new();
        public SoundAtomos Atomos = new();
        public SoundExtra Extra = new();
        public SoundRandomTracks RandomTracks = new(); // Includes Cycle
        public SoundTracks Tracks = new();

        private bool RoutingEnabled => Attributes.Value.HasFlag( SoundAttribute.ExistRoutingSetting );
        private bool BusDuckingEnabled => Attributes.Value.HasFlag( SoundAttribute.BusDucking );
        private bool AccelerationEnabled => Attributes.Value.HasFlag( SoundAttribute.Acceleration );
        private bool AtomosEnabled => Attributes.Value.HasFlag( SoundAttribute.Atomosgear );
        private bool ExtraEnabled => Attributes.Value.HasFlag( SoundAttribute.ExtraDesc );
        private bool RandomTracksEnabled => Type.Value == SoundType.Random || Type.Value == SoundType.Cycle || Type.Value == SoundType.GroupRandom || Type.Value == SoundType.GroupOrder;

        public override void Read( BinaryReader reader ) {
            var trackCount = reader.ReadByte();
            BusNumber.Read( reader );
            Priority.Read( reader );
            Type.Read( reader );
            Attributes.Read( reader );
            Volume.Read( reader );
            LocalNumber.Read( reader );
            UserId.Read( reader );
            PlayHistory.Read( reader );

            if( RoutingEnabled ) RoutingInfo.Read( reader );
            if( BusDuckingEnabled ) BusDucking.Read( reader );
            if( AccelerationEnabled ) Acceleration.Read( reader );
            if( AtomosEnabled ) Atomos.Read( reader );
            if( ExtraEnabled ) Extra.Read( reader );

            if( RandomTracksEnabled ) RandomTracks.Read( reader, Type.Value, trackCount );
            else Tracks.Read( reader, trackCount );
        }

        public override void Write( BinaryWriter writer ) {
            writer.Write( ( byte )( RandomTracksEnabled ? RandomTracks.Tracks.Count : Tracks.Tracks.Count ) );
            BusNumber.Write( writer );
            Priority.Write( writer );
            Type.Write( writer );
            Attributes.Write( writer );
            Volume.Write( writer );
            LocalNumber.Write( writer );
            UserId.Write( writer );
            PlayHistory.Write( writer );

            if( RoutingEnabled ) RoutingInfo.Write( writer );
            if( BusDuckingEnabled ) BusDucking.Write( writer );
            if( AccelerationEnabled ) Acceleration.Write( writer );
            if( AtomosEnabled ) Atomos.Write( writer );
            if( ExtraEnabled ) Extra.Write( writer );

            if( RandomTracksEnabled ) RandomTracks.Write( writer, Type.Value );
            else Tracks.Write( writer );
        }

        public void Draw() {
            ImGui.TextDisabled( "请确保音效和布局的数量保持一致" );

            BusNumber.Draw( CommandManager.Scd );
            Priority.Draw( CommandManager.Scd );
            Type.Draw( CommandManager.Scd );
            Volume.Draw( CommandManager.Scd );
            LocalNumber.Draw( CommandManager.Scd );
            UserId.Draw( CommandManager.Scd );
            PlayHistory.Draw( CommandManager.Scd );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "属性" ) ) {
                Attributes.Draw( CommandManager.Scd );
                ImGui.EndTabItem();
            }

            if( RoutingEnabled && ImGui.BeginTabItem( "路由" ) ) {
                RoutingInfo.Draw();
                ImGui.EndTabItem();
            }
            if( BusDuckingEnabled && ImGui.BeginTabItem( "总线下降" ) ) {
                BusDucking.Draw();
                ImGui.EndTabItem();
            }
            if( AccelerationEnabled && ImGui.BeginTabItem( "加速" ) ) {
                Acceleration.Draw();
                ImGui.EndTabItem();
            }
            if( AtomosEnabled && ImGui.BeginTabItem( "Atomos" ) ) {
                Atomos.Draw();
                ImGui.EndTabItem();
            }
            if( ExtraEnabled && ImGui.BeginTabItem( "额外" ) ) {
                Extra.Draw();
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "轨道" ) ) {
                if( RandomTracksEnabled ) RandomTracks.Draw( Type.Value );
                else Tracks.Draw();
                ImGui.EndTabItem();
            }
        }
    }
}
