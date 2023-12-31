﻿using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;

namespace VfxEditor.Select.Pap.Job {
    public class JobRowSelected {
        // Race -> Idle, MoveA, MoveB
        public Dictionary<string, Dictionary<string, string>> General;
        // Race -> Start, Loop
        public Dictionary<string, Dictionary<string, string>> Poses;
        // Race -> Auto1, Auto2, ...
        public Dictionary<string, Dictionary<string, string>> AutoAttack;
    }

    public class JobTab : SelectTab<JobRow, JobRowSelected> {
        public JobTab( SelectDialog dialog, string name ) : base( dialog, name, "Job-Pap", SelectResultType.GameJob ) { }

        public override void LoadData() {
            foreach( var item in SelectDataUtils.JobAnimationIds ) Items.Add( new JobRow( item.Key, item.Value ) );
        }

        public override void LoadSelection( JobRow item, out JobRowSelected loaded ) {
            var general = new Dictionary<string, Dictionary<string, string>>();
            var poses = new Dictionary<string, Dictionary<string, string>>();
            var autoAttack = new Dictionary<string, Dictionary<string, string>>();

            var jobId = item.Id;
            var movementJobId = SelectDataUtils.JobMovementOverride.TryGetValue( item.Name, out var _movement ) ? _movement : jobId;
            var drawJobId = SelectDataUtils.JobDrawOverride.TryGetValue( item.Name, out var _draw ) ? _draw : jobId;
            var autoJobId = SelectDataUtils.JobAutoOverride.TryGetValue( item.Name, out var _auto ) ? _auto : jobId;

            foreach( var (raceName, raceData) in SelectDataUtils.RaceAnimationIds ) {
                var skeleton = raceData.SkeletonId;

                // General

                var idlePath = SelectDataUtils.GetSkeletonPath( skeleton, $"{jobId}/resident/idle.pap" );
                var movePathA = SelectDataUtils.GetSkeletonPath( skeleton, $"{movementJobId}/resident/move_a.pap" );
                var movePathB = SelectDataUtils.GetSkeletonPath( skeleton, $"{movementJobId}/resident/move_b.pap" );
                var drawPath = SelectDataUtils.GetSkeletonPath( skeleton, $"{drawJobId}/resident/sub.pap" );

                var raceGeneral = new Dictionary<string, string>();
                if( Plugin.DataManager.FileExists( idlePath ) ) raceGeneral.Add( "闲置动作", idlePath );
                if( Plugin.DataManager.FileExists( movePathA ) ) raceGeneral.Add( "移动动作 A", movePathA );
                if( Plugin.DataManager.FileExists( movePathB ) ) raceGeneral.Add( "移动动作 B", movePathB );
                if( Plugin.DataManager.FileExists( drawPath ) ) raceGeneral.Add( "Draw Weapon", drawPath );
                general.Add( raceName, raceGeneral );

                // Pose

                var start = SelectDataUtils.GetSkeletonPath( skeleton, $"{jobId}/emote/b_pose01_start.pap" );
                var loop = SelectDataUtils.GetSkeletonPath( skeleton, $"{jobId}/emote/b_pose01_loop.pap" );

                if( Plugin.DataManager.FileExists( start ) && Plugin.DataManager.FileExists( loop ) ) {
                    poses.Add( raceName, new Dictionary<string, string>() {
                        { "开始", start },
                        { "循环", loop }
                    } );
                }

                // Auto

                var autoPaths = new List<string>();

                for( var i = 1; i <= 3; i++ ) {
                    var autoPath = SelectDataUtils.GetSkeletonPath( skeleton, $"{autoJobId}/battle/auto_attack{i}.pap" );
                    var autoShotPath = SelectDataUtils.GetSkeletonPath( skeleton, $"{autoJobId}/battle/auto_attack_shot{i}.pap" );
                    if( Plugin.DataManager.FileExists( autoPath ) ) autoPaths.Add( autoPath );
                    if( Plugin.DataManager.FileExists( autoShotPath ) ) autoPaths.Add( autoShotPath );
                }

                var raceAutos = new Dictionary<string, string>();
                for( var i = 0; i < autoPaths.Count; i++ ) {
                    raceAutos.Add( $"自动攻击 {i + 1}", autoPaths[i] );
                }

                autoAttack.Add( raceName, raceAutos );
            }

            loaded = new JobRowSelected {
                General = general,
                Poses = poses,
                AutoAttack = autoAttack
            };
        }

        protected override void DrawSelected() {
            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            if( ImGui.BeginTabItem( "一般" ) ) {
                DrawPapsWithHeader( Loaded.General, Selected.Name );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "姿势" ) ) {
                DrawPapsWithHeader( Loaded.Poses, Selected.Name );
                ImGui.EndTabItem();
            }
            if( ImGui.BeginTabItem( "自动攻击" ) ) {
                DrawPapsWithHeader( Loaded.AutoAttack, Selected.Name );
                ImGui.EndTabItem();
            }
        }

        protected override string GetName( JobRow item ) => item.Name;
    }
}
