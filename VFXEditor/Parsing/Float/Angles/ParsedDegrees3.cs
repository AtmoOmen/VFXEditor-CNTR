using OtterGui.Raii;
using System.Numerics;
using VfxEditor.Utils;

namespace VfxEditor.Parsing {
    public class ParsedDegrees3 : ParsedFloat3 {
        public ParsedDegrees3( string name ) : base( name ) { }

        public ParsedDegrees3( string name, Vector3 defaultValue ) : base( name, defaultValue ) { }

        public override void Draw( CommandManager manager ) {
            using var _ = ImRaii.PushId( Name );
            Copy( manager );

            if( UiUtils.DrawDegrees3( Name, Value, out var newValue ) ) {
                manager.Add( new ParsedSimpleCommand<Vector3>( this, newValue ) );
            }
        }
    }
}
