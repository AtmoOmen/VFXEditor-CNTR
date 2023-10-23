using System;
using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Component.Node.Data {
    [Flags]
    public enum NodeComponentFlags {
        RepeatUp = 0x80,
        RepeatDown = 0x40,
        RepeatLeft = 0x20,
        RepeatRight = 0x10,
        Unknown_1 = 0x20,
        Unknown_2 = 0x40,
        Unknown_3 = 0x80
    }

    public class UldNodeComponentData : UldGenericData {
        public UldNodeComponentData() {
            Parsed.AddRange( new ParsedBase[] {
                new ParsedInt( "索引", size: 1 ),
                new ParsedInt( "上", size: 1 ),
                new ParsedInt( "下", size: 1 ),
                new ParsedInt( "左", size: 1 ),
                new ParsedInt( "右", size: 1 ),
                new ParsedInt( "指针", size: 1 ),
                new ParsedFlag<NodeComponentFlags>( "标识", size: 1 ),
                new ParsedInt( "未知", size: 1 ),
                new ParsedShort( "X 轴偏移" ),
                new ParsedShort( "Y 轴偏移" )
            } );
        }
    }
}
