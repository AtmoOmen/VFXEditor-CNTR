﻿using VfxEditor.Select.Shared.Mount;

namespace VfxEditor.Select.Shared.Skeleton {
    public class SkeletonMountTab : MountTab {
        private readonly string Prefix;
        private readonly string Extension;

        public SkeletonMountTab( SelectDialog dialog, string name, string prefix, string extension ) : base( dialog, name ) {
            Prefix = prefix;
            Extension = extension;
        }

        protected override void DrawSelected() {
            SelectUiUtils.DrawIcon( Icon );

            var path = Selected.GetSkeletonPath( Prefix, Extension );
            if( Plugin.DataManager.FileExists( path ) ) {
                DrawPath( "路径", path, Selected.Name );
            }
        }
    }
}
