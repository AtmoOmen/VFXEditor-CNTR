using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    public class WindowNodeData : UldNodeComponentData {
        public WindowNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "标题文本 ID" ),
                new ParsedUInt( "副标题文本 ID" ),
                new ParsedByteBool( "关闭按钮" ),
                new ParsedByteBool( "设置按钮" ),
                new ParsedByteBool( "帮助按钮" ),
                new ParsedByteBool( "顶栏" ),
            } );
        }
    }
}
