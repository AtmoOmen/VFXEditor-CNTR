using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using VfxEditor.FileManager;
using VfxEditor.Formats.AtchFormat.Entry;
using VfxEditor.Utils;

namespace VfxEditor.Formats.AtchFormat {
    public class AtchFile : FileManagerFile {
        public static readonly Dictionary<string, string> WeaponNames = new() {
            { "2ax", "大斧" },
            { "2bk", "魔导书" },
            { "2bw", "弓" },
            { "2ff", "贤具" },
            { "2gb", "枪刃" },
            { "2gl", "天球仪" },
            { "2gn", "火枪" },
            { "2km", "镰刀" },
            { "2kt", "武士刀" },
            { "2rp", "刺剑" },
            { "2sp", "长枪" },
            { "2st", "巨杖" },
            { "2sw", "双手剑" },
            { "aai", "炼金术师" },
            { "aal", "炼金术师" },
            { "aar", "铸甲匠" },
            { "abl", "铸甲匠" },
            { "aco", "烹调师" },
            { "agl", "雕金匠" },
            { "ali", "炼金术师" },
            { "alm", "炼金术师" },
            { "alt", "制革匠" },
            { "ase", "裁衣匠" },
            // { "atr", "" },
            // { "avt", "" },
            { "awo", "刻木匠" },
            { "bag", "机工士背包" },
            { "chk", "战轮" },
            // { "clb", "" },
            { "clg", "手套" },
            // { "cls", "" }, // Linked to axes
            { "clw", "爪" },
            // { "col", "" },
            // { "cor", "" },
            // { "cos", "" },
            { "crd", "占星术士卡牌" },
            // { "crr", "" },
            // { "crt", "" },
            { "csl", "刻木匠" },
            { "csr", "刻木匠" },
            { "dgr", "Dagger" },
            { "drm", "鼓" },
            { "ebz", "钐镰客魂衣" },
            // { "egp", "" },
            // { "elg", "" },
            // { "fch", "" },
            // { "fdr", "" },
            { "fha", "捕鱼人" },
            // { "fl2", "Harp" },
            { "flt", "笛" },
            { "frg", "忍者烟雾" },
            { "fry", "制革匠/烹调师" },
            { "fsh", "捕鱼人" },
            { "fsw", "格斗武器" },
            // { "fud", "" },
            // { "gdb", "" },
            // { "gdh", "" },
            // { "gdl", "" }
            // { "gdr", "" },
            // { "gdt", "" },
            // { "gdw", "" },
            { "gsl", "机工士召唤物" },
            // { "gsr", "" }, // Diadem cannon?
            // { "gun", "" },
            // { "hel", "" },
            { "hmm", "锻铁匠/铸甲匠" },
            { "hrp", "吟游诗人乐器" },
            { "htc", "园艺工" },
            { "ksh", "武士刀鞘" },
            // { "let", "" },
            // { "lpr", "" }, // Linked to 1923
            { "mlt", "雕金匠" },
            { "mrb", "炼金术师" },
            { "mrh", "炼金术师" },
            { "msg", "机工士霰弹枪" },
            { "mwp", "机工士火枪" },
            { "ndl", "裁衣匠" },
            // { "nik", "" }, // Linked to Nier pod, maybe Nikana or something
            { "nph", "园艺工" },
            { "orb", "赤魔法师刺剑水晶" },
            // { "oum", "" },
            // { "pen", "" }, // Linked to daggers
            { "pic", "采矿工" },
            // { "pra", "" },
            { "prf", "制革匠" },
            { "qvr", "箭囊" },
            // { "rap", "" },
            { "rbt", "忍者兔子" },
            { "rod", "青魔杖" },
            // { "rop", "" },
            { "saw", "刻木匠" },
            // { "sht", "" },
            { "sic", "捕鱼人" },
            { "sld", "盾" },
            { "stf", "杖" },
            { "stv", "烹调师" },
            { "swd", "剑" },
            { "syl", "机工士狙击枪" },
            // { "syr", "" },
            // { "syu", "" },
            // { "tan", "" },
            { "tbl", "雕金匠" },
            // { "tcs", "" },
            { "tgn", "雕金匠" },
            { "tmb", "裁衣匠" },
            // { "trm", "" }, // Linked to flute
            // { "trr", "" },
            // { "trw", "" }, // Linked to greatswords
            // { "vln", "" },
            { "whl", "裁衣匠" },
            // { "wng", "" },
            // { "ypd", "" },
            { "ytk", "铸甲匠" },
        };

        public readonly ushort NumStates;
        public readonly List<AtchEntry> Entries = new();
        private readonly AtchEntrySplitView EntryView;

        public AtchFile( BinaryReader reader ) : base( new CommandManager( Plugin.AtchManager ) ) {
            Verified = VerifiedStatus.UNSUPPORTED; // verifying these is fucked. The format is pretty simple though, so it's not a big deal

            var numEntries = reader.ReadUInt16();
            NumStates = reader.ReadUInt16();

            for( var i = 0; i < numEntries; i++ ) {
                Entries.Add( new( reader ) );
            }

            var bitFields = new List<uint>();
            for( var i = 0; i < 4; i++ ) bitFields.Add( reader.ReadUInt32() );

            for( var i = 0; i < numEntries; i++ ) {
                var bitField = bitFields[i >> 5];
                Entries[i].Accessory.Value = ( ( bitField >> ( i & 0x1F ) ) & 1 ) == 1;
            }

            var dataEnd = reader.BaseStream.Position;

            Entries.ForEach( x => x.ReadBody( reader, NumStates ) );
            EntryView = new( Entries );
        }

        public override void Write( BinaryWriter writer ) {
            writer.Write( ( ushort )Entries.Count );
            writer.Write( NumStates );

            Entries.ForEach( x => x.Write( writer ) );

            var bitFields = new List<uint>();
            for( var i = 0; i < 4; i++ ) bitFields.Add( 0 );

            for( var i = 0; i < Entries.Count; i++ ) {
                var idx = i >> 5;
                var value = ( Entries[i].Accessory.Value ? 1u : 0u ) << ( i & 0x1F );
                bitFields[idx] = bitFields[idx] | value;
            }

            bitFields.ForEach( writer.Write );

            var stringStartPos = 2 + 2 + ( 4 * Entries.Count ) + 16 + ( 32 * Entries.Count * NumStates );
            using var stringMs = new MemoryStream();
            using var stringWriter = new BinaryWriter( stringMs );
            var stringPos = new Dictionary<string, int>();

            Entries.ForEach( x => x.WriteBody( writer, stringStartPos, stringWriter, stringPos ) );

            writer.Write( stringMs.ToArray() );
        }

        public override void Draw() {
            DrawCurrentWeapons();

            ImGui.Separator();

            EntryView.Draw();
        }

        private void DrawCurrentWeapons() {
            if( Plugin.ClientState == null || Plugin.PlayerObject == null ) return;

            var weapons = new List<string>();

            var dataStart = Plugin.PlayerObject.Address + 0x6E8 + 32;
            for( var i = 0; i < 3; i++ ) {
                var data = dataStart + ( 104 * i );
                if( Marshal.ReadInt64( data + 8 ) == 0 || Marshal.ReadInt64( data + 16 ) == 0 || Marshal.ReadInt32( data + 32 ) == 0 ) continue;

                var nameArr = Marshal.PtrToStringAnsi( data + 32 ).ToCharArray();
                Array.Reverse( nameArr );
                weapons.Add( new string( nameArr ) );
            }

            if( weapons.Count == 0 ) return;

            ImGui.Separator();

            ImGui.TextDisabled( $"当前武器: {weapons.Aggregate( ( x, y ) => x + " | " + y )}" );
        }
    }
}
