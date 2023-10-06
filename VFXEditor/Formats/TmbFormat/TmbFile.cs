using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.FileManager;
using VfxEditor.TmbFormat.Actor;
using VfxEditor.TmbFormat.Entries;
using VfxEditor.TmbFormat.Tmfcs;
using VfxEditor.TmbFormat.Utils;
using VfxEditor.Ui.Components.SplitViews;
using VfxEditor.Utils;

// Rework based on https://github.com/AsgardXIV/XAT
namespace VfxEditor.TmbFormat {
    public class TmbFile : FileManagerFile {
        public readonly Tmdh HeaderTmdh;
        public readonly Tmpp HeaderTmpp;
        public readonly Tmal HeaderTmal;

        public readonly List<Tmfc> Tmfcs = new();
        public readonly List<Tmac> Actors = new();
        public readonly List<Tmtr> Tracks = new();
        public readonly List<TmbEntry> AllEntries = new();

        public readonly TmbActorDropdown ActorsDropdown;
        public readonly TmfcDropdown TmfcDropdown;

        private TmbEntry DraggingEntry = null;

        private readonly List<Tmtr> UnusedTracks;
        private readonly UiSplitView<Tmtr> UnusedTrackView;

        public TmbFile( BinaryReader binaryReader, bool verify ) : this( binaryReader, new( Plugin.TmbManager ), verify ) { }

        public TmbFile( BinaryReader binaryReader, CommandManager manager, bool verify ) : base( manager ) {
            ActorsDropdown = new( this );
            TmfcDropdown = new( this );

            var startPos = binaryReader.BaseStream.Position;
            var reader = new TmbReader( binaryReader );

            reader.ReadInt32(); // TMLB
            var size = reader.ReadInt32();
            var numEntries = reader.ReadInt32(); // entry count (not including TMLB)

            HeaderTmdh = new Tmdh( this, reader );
            HeaderTmpp = new Tmpp( this, reader );
            HeaderTmal = new Tmal( this, reader );

            for( var i = 0; i < numEntries - ( HeaderTmpp.IsAssigned ? 3 : 2 ); i++ ) {
                reader.ParseItem( this, Actors, Tracks, AllEntries, Tmfcs, ref Verified );
            }

            HeaderTmal.PickActors( reader );
            Actors.ForEach( x => x.PickTracks( reader ) );
            Tracks.ForEach( x => x.PickEntries( reader ) );

            RefreshIds();

            if( verify ) Verified = FileUtils.Verify( binaryReader, ToBytes(), null );

            binaryReader.BaseStream.Seek( startPos + size, SeekOrigin.Begin );

            UnusedTracks = Tracks.Where( x => !Actors.Where( a => a.Tracks.Contains( x ) ).Any() ).ToList();
            UnusedTrackView = new( "Track", UnusedTracks, false, false );
        }

        public override void Write( BinaryWriter writer ) {
            var startPos = writer.BaseStream.Position;
            FileUtils.WriteString( writer, "TMLB" );
            writer.Write( 0 ); // placeholder for size

            RefreshIds();

            var timelineCount = Actors.Count + Actors.Select( x => x.Tracks.Count ).Sum() + Tracks.Select( x => x.Entries.Count ).Sum();

            List<TmbItem> items = new() { HeaderTmdh };
            if( HeaderTmpp.IsAssigned ) items.Add( HeaderTmpp );
            items.Add( HeaderTmal );
            items.AddRange( Actors );
            items.AddRange( Tracks );
            items.AddRange( AllEntries );

            var itemLength = items.Sum( x => x.Size );
            var extraLength = items.Sum( x => x.ExtraSize );
            var timelineLength = timelineCount * sizeof( short );
            var tmbWriter = new TmbWriter( itemLength, extraLength, timelineLength );

            writer.Write( items.Count );
            foreach( var item in items ) {
                tmbWriter.StartPosition = tmbWriter.Position;
                item.Write( tmbWriter );
            }

            tmbWriter.WriteTo( writer );
            tmbWriter.Dispose();

            // Fill in size placeholder
            var endPos = writer.BaseStream.Position;
            writer.BaseStream.Seek( startPos + 4, SeekOrigin.Begin );
            writer.Write( ( int )( endPos - startPos ) );
            writer.BaseStream.Seek( endPos, SeekOrigin.Begin );
        }

        public override void Draw() {
            var maxDanger = AllEntries.Count == 0 ? DangerLevel.None : AllEntries.Select( x => x.Danger ).Max();
            if( maxDanger == DangerLevel.DontAddRemove ) DontAddRemoveWarning();
            else if( maxDanger == DangerLevel.Detectable || Tmfcs.Count > 0 ) DetectableWarning();

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawParameters();

            using( var tab = ImRaii.TabItem( "对象" ) ) {
                if( tab ) ActorsDropdown.Draw();
            }

            using( var tab = ImRaii.TabItem( "F 曲线" ) ) {
                if( tab ) TmfcDropdown.Draw();
            }

            DrawUnused();
        }

        private void DrawParameters() {
            using var tabItem = ImRaii.TabItem( "参数" );
            if( !tabItem ) return;

            HeaderTmdh.Draw();
            HeaderTmpp.Draw();
        }

        private void DrawUnused() {
            if( UnusedTracks.Count == 0 ) return;

            using var tabItem = ImRaii.TabItem( "未使用" );
            if( !tabItem ) return;

            ImGui.TextDisabled( "剩下的音轨，从未被触发，仅做研究用" );
            ImGui.Separator();
            UnusedTrackView.Draw();
        }

        public void RefreshIds() {
            short id = 2;
            foreach( var actor in Actors ) actor.Id = id++;
            foreach( var track in Tracks ) track.Id = id++;
            foreach( var entry in AllEntries ) entry.Id = id++;
        }

        public void StartDragging( TmbEntry entry ) {
            ImGui.SetDragDropPayload( "TMB_ENTRY", IntPtr.Zero, 0 );
            DraggingEntry = entry;
        }

        public unsafe void StopDragging( Tmtr destination ) {
            if( DraggingEntry == null ) return;
            var payload = ImGui.AcceptDragDropPayload( "TMB_ENTRY" );
            if( payload.NativePtr == null ) return;

            var command = new TmbRefreshIdsCommand( this );
            foreach( var track in Tracks ) {
                track.DeleteEntry( command, DraggingEntry ); // will add to command
            }
            destination.AddEntry( command, DraggingEntry );
            Command.Add( command );

            DraggingEntry = null;
        }

        // ===============

        public static TmbFile FromPapEmbedded( string path, CommandManager manager ) {
            if( !File.Exists( path ) ) return null;
            using BinaryReader br = new( File.Open( path, FileMode.Open ) );
            return new TmbFile( br, manager, true );
        }

        public static void DetectableWarning() {
            ImGui.PushStyleColor( ImGuiCol.Text, UiUtils.RED_COLOR );
            ImGui.TextWrapped( "更改此文件有被潜在检测的可能性" );
            ImGui.PopStyleColor();
            ImGui.SameLine();
            if( ImGui.SmallButton( "指南 (英文)" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki/Notes-on-TMFC" );
        }

        public static void GenericWarning() {
            ImGui.PushStyleColor( ImGuiCol.Text, UiUtils.RED_COLOR );
            ImGui.TextWrapped( "请不要对此项做出任何修改" );
            ImGui.PopStyleColor();
        }

        public static void DontAddRemoveWarning() {
            ImGui.PushStyleColor( ImGuiCol.Text, UiUtils.RED_COLOR );
            ImGui.TextWrapped( "请不要在此文件中增加或移除任何条目" );
            ImGui.PopStyleColor();
        }
    }
}
