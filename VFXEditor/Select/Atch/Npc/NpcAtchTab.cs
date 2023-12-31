﻿using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Select.Shared.Npc;

namespace VfxEditor.Select.Atch.Npc {
    public class NpcAtchTab : SelectTab<NpcRow> {
        public NpcAtchTab( SelectDialog dialog, string name ) : base( dialog, name, "Atch-Npc", SelectResultType.GameNpc ) {
        }

        public override void LoadData() {
            var file = Plugin.DataManager.GetFile( "chara/xls/attachoffset/attachoffsetexist.waoe" );
            using var ms = new MemoryStream( file.Data );
            using var reader = new BinaryReader( ms );

            var count = reader.ReadUInt16() + 1;
            var offsetsExist = new HashSet<ushort>();

            for( var i = 0; i < count; i++ ) {
                offsetsExist.Add( reader.ReadUInt16() );
            }

            // ================

            var nameToString = NpcTab.NameToString;

            var baseToName = JsonConvert.DeserializeObject<Dictionary<string, uint>>( File.ReadAllText( SelectDataUtils.BnpcPath ) );
            var battleNpcSheet = Plugin.DataManager.GetExcelSheet<BNpcBase>();
            foreach( var entry in baseToName ) {
                if( !nameToString.TryGetValue( entry.Value, out var name ) ) continue;

                var bnpcRow = battleNpcSheet.GetRow( uint.Parse( entry.Key ) );
                if( !NpcTab.BnpcValid( bnpcRow ) ) continue;

                var modelChara = bnpcRow.ModelChara.Value;
                var id = ( ushort )( modelChara.Type == 2 ? modelChara.Model + 10000 : modelChara.Model );
                if( !offsetsExist.Contains( id ) ) continue;

                Items.Add( new NpcRow( modelChara, name ) );
            }

        }

        protected override void DrawSelected() {
            DrawPath( "路径", Selected.AtchPath, Selected.Name );
        }

        protected override string GetName( NpcRow item ) => item.Name;
    }
}
