﻿using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using VfxEditor.Ui;
using VfxEditor.Utils;

namespace VfxEditor.AvfxFormat.Dialogs {
    public class ExportDialog : GenericDialog {
        private readonly AvfxFile VfxFile;
        private readonly List<ExportDialogCategory> Categories;
        private bool ExportDependencies = true;

        public ExportDialog( AvfxFile vfxFile ) : base( "Export", false, 600, 400 ) {
            VfxFile = vfxFile;
            Categories = new List<ExportDialogCategory> {
                new ExportDialogCategory<AvfxTimeline>( vfxFile.NodeGroupSet.Timelines, "时间线" ),
                new ExportDialogCategory<AvfxEmitter>( vfxFile.NodeGroupSet.Emitters, "发射器" ),
                new ExportDialogCategory<AvfxParticle>( vfxFile.NodeGroupSet.Particles, "粒子" ),
                new ExportDialogCategory<AvfxEffector>( vfxFile.NodeGroupSet.Effectors, "效果器" ),
                new ExportDialogCategory<AvfxBinder>( vfxFile.NodeGroupSet.Binders, "绑定器" ),
                new ExportDialogCategory<AvfxTexture>( vfxFile.NodeGroupSet.Textures, "材质" ),
                new ExportDialogCategory<AvfxModel>( vfxFile.NodeGroupSet.Models, "模型" )
            };
        }

        public void Reset() => Categories.ForEach( cat => cat.Reset() );

        public override void DrawBody() {
            using var _ = ImRaii.PushId( "##ExportDialog" );

            ImGui.Checkbox( "导出依赖关系", ref ExportDependencies );

            ImGui.SameLine();
            UiUtils.HelpMarker( @"导出选中项与它们的依赖关系 (如: 依赖材质的例子效果)。推荐选中此项" );

            ImGui.SameLine();
            if( ImGui.Button( "重置#" ) ) Reset();

            ImGui.SameLine();
            if( ImGui.Button( "导出" ) ) SaveDialog();

            using var child = ImRaii.Child( "子级", ImGui.GetContentRegionAvail(), false );
            Categories.ForEach( cat => cat.Draw() );
        }

        public void ShowDialog( AvfxNode node ) {
            Show();
            Reset();
            foreach( var category in Categories ) {
                if( category.Belongs( node ) ) {
                    category.Select( node );
                    break;
                }
            }
        }

        public List<AvfxNode> GetSelected() {
            var result = new List<AvfxNode>();
            foreach( var category in Categories ) {
                result.AddRange( category.Selected );
            }
            return result;
        }

        public void SaveDialog() {
            FileDialogManager.SaveFileDialog( "选择保存位置", ".vfxedit2,.*", "ExportedVfx", "vfxedit2", ( bool ok, string res ) => {
                if( !ok ) return;
                VfxFile.Export( GetSelected(), res, ExportDependencies );
                Visible = false;
            } );
        }
    }
}
