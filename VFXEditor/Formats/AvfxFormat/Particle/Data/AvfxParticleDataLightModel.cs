namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLightModel : AvfxData {
        public readonly AvfxInt ModelIdx = new( "模型索引", "MNO", size: 1 );

        public readonly UiNodeSelect<AvfxModel> ModelSelect;
        public readonly UiDisplayList Display;

        public AvfxParticleDataLightModel( AvfxParticle particle ) : base() {
            Parsed = new() {
                ModelIdx
            };

            DisplayTabs.Add( Display = new UiDisplayList( "参数" ) );
            Display.Add( ModelSelect = new UiNodeSelect<AvfxModel>( particle, "模型", particle.NodeGroups.Models, ModelIdx ) );
        }

        public override void Enable() => ModelSelect.Enable();

        public override void Disable() => ModelSelect.Disable();
    }
}
