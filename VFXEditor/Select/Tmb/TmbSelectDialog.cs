using ImGuiNET;
using OtterGui.Raii;
using VfxEditor.Select.Tmb.Action;
using VfxEditor.Select.Tmb.Common;
using VfxEditor.Select.Tmb.Emote;
using VfxEditor.Select.Tmb.Npc;
using VfxEditor.Spawn;
using VfxEditor.TmbFormat;

namespace VfxEditor.Select.Tmb {
    public class TmbSelectDialog : SelectDialog {
        public TmbSelectDialog( string id, TmbManager manager, bool isSourceDialog ) : base( id, "tmb", manager, isSourceDialog ) {
            GameTabs.AddRange( new SelectTab[]{
                new ActionTab( this, "技能" ),
                new NonPlayerActionTab( this, "非玩家对象动作" ),
                new EmoteTab( this, "表情" ),
                new NpcTmbTab( this, "NPC" ),
                new CommonTab( this, "通常" )
            } );
        }

        public override void Play( string path ) {
            using var _ = ImRaii.PushId( "Spawn" );

            ImGui.SameLine();
            if( TmbSpawn.CanReset ) {
                if( ImGui.Button( "重置" ) ) TmbSpawn.Reset();
            }
            else {
                if( ImGui.Button( "播放" ) ) TmbSpawn.Apply( path );
            }
        }
    }
}
