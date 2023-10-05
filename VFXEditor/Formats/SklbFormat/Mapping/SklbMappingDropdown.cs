using System.Collections.Generic;
using VfxEditor.Ui.Components;

namespace VfxEditor.SklbFormat.Mapping {
    public class SklbMappingDropdown : Dropdown<SklbMapping> {
        public SklbMappingDropdown( List<SklbMapping> items ) : base( "映射", items, false, false ) { }

        protected override string GetText( SklbMapping item, int idx ) => $"映射 {idx}";

        protected override void OnDelete( SklbMapping item ) { }

        protected override void OnNew() { }

        protected override void DrawSelected() => Selected.Draw();
    }
}
