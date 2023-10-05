using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;
using VfxEditor.Ui.Interfaces;

namespace VfxEditor.ScdFormat {
    public class ScdAttributeEntry : ScdEntry, IUiItem {
        public readonly ParsedByte Version = new( "版本" );
        public readonly ParsedReserve Reserved = new( 1 );
        public readonly ParsedShort AttributeId = new( "属性 ID" );
        public readonly ParsedShort SearchAttributeId = new( "搜索属性 ID" );
        public readonly ParsedByte ConditionFirst = new( "第一情况" );
        public readonly ParsedByte ArgumentCount = new( "参数数量" );
        public readonly ParsedInt SoundLabelLow = new( "低音标签" );
        public readonly ParsedInt SoundLabelHigh = new( "高音标签" );

        public readonly AttributeResultCommand ResultFirst = new();
        public readonly AttributeExtendData Extend1 = new();
        public readonly AttributeExtendData Extend2 = new();
        public readonly AttributeExtendData Extend3 = new();
        public readonly AttributeExtendData Extend4 = new();

        private readonly List<ParsedBase> Parsed;

        public ScdAttributeEntry() {
            Parsed = new() {
                Version,
                Reserved,
                AttributeId,
                SearchAttributeId,
                ConditionFirst,
                ArgumentCount,
                SoundLabelLow,
                SoundLabelHigh,
            };
        }

        public override void Read( BinaryReader reader ) {
            Parsed.ForEach( x => x.Read( reader ) );

            ResultFirst.Read( reader );
            Extend1.Read( reader );
            Extend2.Read( reader );
            Extend3.Read( reader );
            Extend4.Read( reader );
        }

        public override void Write( BinaryWriter writer ) {
            Parsed.ForEach( x => x.Write( writer ) );

            ResultFirst.Write( writer );
            Extend1.Write( writer );
            Extend2.Write( writer );
            Extend3.Write( writer );
            Extend4.Write( writer );
        }

        public void Draw() {
            Parsed.ForEach( x => x.Draw( CommandManager.Scd ) );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;

            DrawResult();
            DrawExtendData( "扩展 1", Extend1 );
            DrawExtendData( "扩展 2", Extend2 );
            DrawExtendData( "扩展 3", Extend3 );
            DrawExtendData( "扩展 4", Extend4 );
        }

        private void DrawResult() {
            using var tabItem = ImRaii.TabItem( "结果" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Result" );
            ResultFirst.Draw();
        }

        private static void DrawExtendData( string name, AttributeExtendData extendData ) {
            using var tabItem = ImRaii.TabItem( name );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( name );
            extendData.Draw();
        }
    }
}
