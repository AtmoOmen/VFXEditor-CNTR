using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Ui.Interfaces;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticleUvSet : AvfxSelectableItem {
        public readonly AvfxEnum<TextureCalculateUV> CalculateUVType = new( "UV 计算方式", "CUvT" );
        public readonly AvfxCurve2Axis Scale = new( "缩放", "Scl" );
        public readonly AvfxCurve2Axis Scroll = new( "滚动", "Scr" );
        public readonly AvfxCurve Rot = new( "旋转", "Rot", CurveType.Angle );
        public readonly AvfxCurve RotRandom = new( "随机旋转", "RotR", CurveType.Angle );

        private readonly List<AvfxBase> Parsed;
        private readonly List<AvfxItem> Curves;
        private readonly List<IUiItem> Display;

        public AvfxParticleUvSet() : base( "UvSt" ) {
            Parsed = new() {
                CalculateUVType,
                Scale,
                Scroll,
                Rot,
                RotRandom
            };

            Display = new() {
                CalculateUVType
            };

            Curves = new() {
                Scale,
                Scroll,
                Rot,
                RotRandom
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) => ReadNested( reader, Parsed, size );

        protected override void RecurseChildrenAssigned( bool assigned ) => RecurseAssigned( Parsed, assigned );

        protected override void WriteContents( BinaryWriter writer ) => WriteNested( writer, Parsed );

        public override void Draw() {
            using var _ = ImRaii.PushId( "UV" );

            DrawItems( Display );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            DrawNamedItems( Curves );
        }

        public override string GetDefaultText() => $"UV {GetIdx()}";
    }
}
