﻿using Dalamud.Logging;
using ImGuiFileDialog;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Numerics;
using VfxEditor.Formats.TextureFormat;
using VfxEditor.Utils;

namespace VfxEditor.Ui.Tools {
    public class UtilitiesTab {
        private string ExtractPath = "";

        public void Draw() {
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );
            ImGui.TextDisabled( "解压原始游戏文件" );

            using( var indent = ImRaii.PushIndent() )
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 4, 3 ) ) ) {
                ImGui.InputTextWithHint( "##Extract", "游戏路径", ref ExtractPath, 255 );

                ImGui.SameLine();
                if( ImGui.Button( "解压" ) ) {
                    var cleanedPath = ExtractPath.Replace( "\\", "/" );
                    if( Plugin.DataManager.FileExists( cleanedPath ) ) {
                        try {
                            var ext = cleanedPath.Contains( '.' ) ? cleanedPath.Split( "." )[1] : "bin";
                            var file = Plugin.DataManager.GetFile( cleanedPath );
                            UiUtils.WriteBytesDialog( $".{ext}", file.Data, ext );
                        }
                        catch( Exception e ) {
                            PluginLog.Error( e, "无法读取文件" );
                        }
                    }
                }
            }

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            ImGui.TextDisabled( "图像转换" );

            using var indent2 = ImRaii.PushIndent();

            if( ImGui.Button( ".atex 转 PNG" ) ) {
                FileDialogManager.OpenFileDialog( "选择文件", ".atex,.*", ( ok, res ) => {
                    if( !ok ) return;
                    var texFile = TextureDataFile.LoadFromLocal( res );
                    texFile.SaveAsPng( res + ".png" );
                } );
            }

            ImGui.SameLine();
            if( ImGui.Button( ".atex 转 DDS" ) ) {
                FileDialogManager.OpenFileDialog( "选择文件", ".atex,.*", ( ok, res ) => {
                    if( !ok ) return;
                    var texFile = TextureDataFile.LoadFromLocal( res );
                    texFile.SaveAsDds( res + ".dds" );
                } );
            }
        }

        /*
         * Skill Swapping
         * 
         * Get TMB and PAP for both
         *  - split into end/start/hit
         *  - skip basic destinations (normal_hit, etc)
         *  
         *  get unique animation ids (intersection of animation ids in pap and animation ids used in tmb)
         *  
         *  in TMB, replace C010 with new placeholders (only replace the ones from modified .pap files)
         *  
         *  race edge case
         *  - source exists / dest doesn't -> skip
         *  - dest exists / source doesn't -> use closest available (midlander m?)
         *  
         *  action.pap
         *  - both in action.pap -> do nothing (don't even need unique animation ids)
         *  - neither in action.pap -> normal
         *  - source in action.pap, dest not
         *      - don't need to do anything, can just use them
         *  - source not in action.pap dest is [RISK OF MULTIPLE MODIFYING ACTION.PAP]
         *      - just add them to the end
         *      - what about TMFC?
         */
    }
}
