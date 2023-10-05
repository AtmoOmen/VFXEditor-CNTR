using ImGuiNET;
using OtterGui.Raii;
using VfxEditor.AvfxFormat;
using VfxEditor.Select.Vfx.Action;
using VfxEditor.Select.Vfx.Common;
using VfxEditor.Select.Vfx.Cutscene;
using VfxEditor.Select.Vfx.Emote;
using VfxEditor.Select.Vfx.Gimmick;
using VfxEditor.Select.Vfx.Housing;
using VfxEditor.Select.Vfx.Item;
using VfxEditor.Select.Vfx.JournalCutscene;
using VfxEditor.Select.Vfx.Mount;
using VfxEditor.Select.Vfx.Npc;
using VfxEditor.Select.Vfx.Status;
using VfxEditor.Select.Vfx.Zone;
using VfxEditor.Spawn;

namespace VfxEditor.Select.Vfx {
    public class VfxSelectDialog : SelectDialog {
        public VfxSelectDialog( string id, AvfxManager manager, bool isSourceDialog ) : base( id, "avfx", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new VfxItemTab( this, "物品" ),
                new StatusTab( this, "状态" ),
                new ActionTab( this, "技能" ),
                new NonPlayerActionTab( this, "非玩家对象动作" ),
                new EmoteTab( this, "表情" ),
                new ZoneTab( this, "区域" ),
                new GimmickTab( this, "机制" ),
                new HousingTab( this, "房屋" ),
                new NpcVfxTab( this, "NPC" ),
                new MountTab( this, "坐骑" ),
                new CutsceneTab( this, "过场动画" ),
                new JournalCutsceneTab( this, "可回放过场动画" ),
                new CommonTab( this, "通常" )
            } );
        }

        public override void Play( string path ) {
            using var _ = ImRaii.PushId( "Spawn" );

            ImGui.SameLine();
            if( VfxSpawn.Active ) {
                if( ImGui.Button( "移除" ) ) VfxSpawn.Remove();
            }
            else {
                if( ImGui.Button( "生成" ) ) ImGui.OpenPopup( "SpawnPopup" );
                VfxSpawn.DrawPopup( path, false );
            }
        }
    }
}
