using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace VfxEditor.Select {
    public class RaceData {
        public readonly string SkeletonId;
        public readonly int HairOffset;
        public readonly List<int> FaceIds;

        public RaceData( string skeletonId, int hairOffset ) {
            SkeletonId = skeletonId;
            HairOffset = hairOffset;
        }
    }

    public partial class SelectDataUtils {
        public static string BnpcPath => Path.Combine( Plugin.RootLocation, "Files", "bnpc.json" );
        public static string NpcFilesPath => Path.Combine( Plugin.RootLocation, "Files", "npc_files.json" );
        public static string MiscVfxPath => Path.Combine( Plugin.RootLocation, "Files", "vfx_misc.txt" );
        public static string MiscTmbPath => Path.Combine( Plugin.RootLocation, "Files", "tmb_misc.txt" );
        public static string MiscUldPath => Path.Combine( Plugin.RootLocation, "Files", "uld_misc.txt" );
        public static string SklbFacesPath => Path.Combine( Plugin.RootLocation, "Files", "sklb_faces.txt" );

        [GeneratedRegex( "\\u0000([a-zA-Z0-9\\/_]*?)\\.avfx", RegexOptions.Compiled )]
        private static partial Regex AvfxRegexPattern();
        public static readonly Regex AvfxRegex = AvfxRegexPattern();

        // https://github.com/imchillin/CMTool/blob/master/ConceptMatrix/Views/SpecialControl.xaml.cs#L365

        public static Dictionary<string, List<int>> FaceMap {
            get {
                if( _FaceMapInternal == null ) PopulateFaceMap();
                return _FaceMapInternal;
            }
        }

        private static Dictionary<string, List<int>> _FaceMapInternal;

        private static void PopulateFaceMap() {
            if( _FaceMapInternal != null ) return;
            _FaceMapInternal = new();

            var sklbFiles = File.ReadAllLines( SklbFacesPath );
            // chara/human/c0101/skeleton/face/f0222/skl_c0101f0222.sklb
            foreach( var line in sklbFiles ) {
                var modelFace = line.Split( "skl_" )[1].Replace( ".sklb", "" ).Split( "f" );
                var model = modelFace[0];
                var face = Convert.ToInt32( modelFace[1] );

                if( !_FaceMapInternal.ContainsKey( model ) ) _FaceMapInternal[model] = new();
                _FaceMapInternal[model].Add( face );
            }
        }

        public static readonly Dictionary<string, RaceData> RaceAnimationIds = new() {
            { "中原之民男性", new RaceData( "c0101", 0 ) },
            { "中原之民女性", new RaceData( "c0201", 100 ) },
            { "高地之民男性", new RaceData( "c0301", 200 ) },
            { "高地之民女性", new RaceData( "c0401", 300 ) },
            { "精灵族男性", new RaceData( "c0501", 400 ) },
            { "精灵族女性", new RaceData( "c0601", 500 ) },
            { "猫魅族男性", new RaceData( "c0701", 800 ) },
            { "猫魅族女性", new RaceData( "c0801", 900 ) },
            { "鲁加族男性", new RaceData( "c0901", 1000 ) },
            { "鲁加族女性", new RaceData( "c1001", 1100 ) },
            { "拉拉菲尔族男性", new RaceData( "c1101", 600 ) },
            { "拉拉菲尔族女性", new RaceData( "c1201", 700 ) },
            { "敖龙族男性", new RaceData( "c1301", 1200 ) },
            { "敖龙族女性", new RaceData( "c1401", 1300 ) },
            { "硌狮族男性", new RaceData( "c1501", 1400 ) },
            // 1601 coming soon (tm)
            { "维埃拉族男性", new RaceData( "c1701", 1600 ) },
            { "维埃拉族女性", new RaceData( "c1801", 1700 ) },
        };

        public static readonly Dictionary<string, string> JobAnimationIds = new() {
            { "战士", "bt_2ax_emp" },
            { "骑士", "bt_swd_sld" },
            { "绝枪战士", "bt_2gb_emp" },
            { "暗黑骑士", "bt_2sw_emp" },
            { "占星术士", "bt_2gl_emp" },
            { "贤者", "bt_2ff_emp" },
            { "学者", "bt_2bk_emp" },
            { "白魔法师", "bt_stf_sld" },
            { "机工士", "bt_2gn_emp" },
            { "舞者", "bt_chk_chk" },
            { "吟游诗人", "bt_2bw_emp" },
            { "武士", "bt_2kt_emp" },
            { "龙骑士", "bt_2sp_emp" },
            { "武僧", "bt_clw_clw" },
            { "忍者", "bt_dgr_dgr" },
            { "钐镰客", "bt_2km_emp" },
            { "赤魔法师", "bt_2rp_emp" },
            { "黑魔法师", "bt_jst_sld" },
            { "召唤师", "bt_2bk_emp" },
            { "青魔法师", "bt_rod_emp" },
        };

        public static readonly Dictionary<string, string> JobMovementOverride = new() {
            { "黑魔法师", "bt_stf_sld" },
            { "忍者", "bt_nin_nin" },
        };

        public static readonly Dictionary<string, string> JobDrawOverride = new() {
            { "黑魔法师", "bt_stf_sld" }
        };

        public static readonly Dictionary<string, string> JobAutoOverride = new() {
            { "黑魔法师", "bt_stf_sld" }
        };

        public static readonly int MaxChangePoses = 6;

        public static readonly int HairEntries = 100;

        public static Dictionary<string, string> FileExistsFilter( Dictionary<string, string> dict ) =>
            dict.Where( x => Plugin.DataManager.FileExists( x.Value ) ).ToDictionary( x => x.Key, x => x.Value );

        public static string GetSkeletonPath( string skeletonId, string path ) => $"chara/human/{skeletonId}/animation/a0001/{path}";

        public static Dictionary<string, string> GetAllSkeletonPaths( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new Dictionary<string, string>();

            return RaceAnimationIds.ToDictionary( x => x.Key, x => GetSkeletonPath( x.Value.SkeletonId, path ) );
        }

        public static Dictionary<string, string> GetAllJobPaps( string jobId, string path ) => FileExistsFilter( GetAllSkeletonPaths( $"{jobId}/{path}.pap" ) );

        public static Dictionary<string, Dictionary<string, string>> GetAllJobPaps( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new Dictionary<string, Dictionary<string, string>>();

            return JobAnimationIds.ToDictionary( x => x.Key, x => GetAllJobPaps( x.Value, path ) );
        }

        public static Dictionary<string, string> GetAllFacePaps( string modelId, string path ) {
            Dictionary<string, string> ret = new();

            if( FaceMap.TryGetValue( modelId, out var faces ) ) {
                foreach( var face in faces ) {
                    ret.Add( $"面部 {face}", $"chara/human/{modelId}/animation/f{face:D4}/nonresident/{path}.pap" );
                }
            }

            return FileExistsFilter( ret );
        }

        public static Dictionary<string, Dictionary<string, string>> GetAllFacePaps( string path ) {
            if( string.IsNullOrEmpty( path ) ) return new Dictionary<string, Dictionary<string, string>>();

            return RaceAnimationIds.ToDictionary( x => x.Key, x => GetAllFacePaps( x.Value.SkeletonId, path ) );
        }
    }
}
