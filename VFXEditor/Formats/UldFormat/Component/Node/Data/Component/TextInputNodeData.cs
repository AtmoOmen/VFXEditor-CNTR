using System;
using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data.Component {
    [Flags]
    public enum TextInputFlags {
        Capitalize = 0x01,
        Mask = 0x02,
        AutoTranslate = 0x04,
        History = 0x08,
        IME = 0x10,
        EscapeClears = 0x20,
        CapsAllowed = 0x40,
        LowerAllowed = 0x80
    }

    [Flags]
    public enum TextInputFlags2 {
        NumbersAllowed = 0x01,
        SymbolsAllowed = 0x02,
        WordWrap = 0x04,
        Multiline = 0x08,
        AutoMaxWidth = 0x10,
        Unknown_1 = 0x20,
        Unknown_2 = 0x40,
        Unknown_3 = 0x80
    }

    public class TextInputNodeData : UldNodeComponentData {
        public TextInputNodeData() : base() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedUInt( "最大宽度" ),
                new ParsedUInt( "最大行数" ),
                new ParsedUInt( "最大字节数" ),
                new ParsedUInt( "最大字符数" ),
                new ParsedFlag<TextInputFlags>( "标识", size: 1 ),
                new ParsedFlag<TextInputFlags2>( "标志 2", size: 1 ),
                new ParsedUInt( "字符集", size: 2 ),
            } );

            for( var i = 1; i <= 16; i++ ) Parsed.Add( new ParsedInt( $"额外字符集 {i}", size: 1 ) );
        }
    }
}
