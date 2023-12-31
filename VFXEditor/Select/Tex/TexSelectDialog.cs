﻿using System;
using VfxEditor.Formats.TextureFormat;
using VfxEditor.Select.Tex.Action;
using VfxEditor.Select.Tex.Status;

namespace VfxEditor.Select.Tex {
    public class TexSelectDialog : SelectDialog {
        public TexSelectDialog( string id, TextureManager manager, bool showLocal, Action<SelectResult> action ) : base( id, "tex", manager, showLocal, action ) {
            GameTabs.AddRange( new SelectTab[]{
                new ActionTab( this, "技能" ),
                new StatusTab( this, "状态" ),
            } );
        }
    }
}
