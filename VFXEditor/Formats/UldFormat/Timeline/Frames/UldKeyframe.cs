using System;
using System.Collections.Generic;
using System.IO;
using VfxEditor.Parsing;

namespace VfxEditor.UldFormat.Timeline.Frames {
    public class UldKeyframe {
        public readonly ParsedUInt Time = new( "时间" );
        public readonly ParsedInt Interpolation = new( "插值", size: 1 );
        public readonly ParsedInt Unk1 = new( "未知", size: 1 );
        public readonly ParsedFloat Acceleration = new( "加速" );
        public readonly ParsedFloat Deceleration = new( "减速" );

        public readonly List<ParsedBase> Data = new();

        public UldKeyframe( KeyGroupType groupType ) {
            Data.AddRange( groupType switch {
                KeyGroupType.Float1 => new ParsedBase[] {
                    new ParsedFloat( "值 1" )
                },
                KeyGroupType.Float2 => new ParsedBase[] {
                    new ParsedFloat( "值 1" ), 
                    new ParsedFloat( "值 2" )
                },
                KeyGroupType.Float3 => new ParsedBase[] {
                    new ParsedFloat( "值 1" ), 
                    new ParsedFloat( "值 2" ), 
                    new ParsedFloat( "值 3" )
                },
                KeyGroupType.SByte1 => new ParsedBase[] {
                    new ParsedSByte( "值 1" ),
                    new ParsedReserve( 3 )
                },
                KeyGroupType.SByte2 => new ParsedBase[] {
                    new ParsedSByte( "值 1" ), 
                    new ParsedSByte( "值 2" ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.SByte3 => new ParsedBase[] {
                    new ParsedSByte( "值 1" ), 
                    new ParsedSByte( "值 2" ), 
                    new ParsedSByte( "值 3" ),
                    new ParsedReserve( 1 )
                },
                KeyGroupType.Byte1 => new ParsedBase[] {
                    new ParsedInt( "值", size: 1 ),
                    new ParsedReserve( 3 )
                },
                KeyGroupType.Byte2 => new ParsedBase[] {
                    new ParsedInt( "值 1", size: 1 ), 
                    new ParsedInt( "值 2", size: 1 ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.Byte3 => new ParsedBase[] {
                    new ParsedInt( "值 1", size: 1 ), 
                    new ParsedInt( "值 2", size: 1 ), 
                    new ParsedInt( "值 3", size: 1 ),
                    new ParsedReserve( 1 )
                },
                KeyGroupType.Short1 => new ParsedBase[] {
                    new ParsedShort( "值 1" ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.Short2 => new ParsedBase[] {
                    new ParsedShort( "值 1" ), 
                    new ParsedShort( "值 2" ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.Short3 => new ParsedBase[] {
                    new ParsedShort( "值 1" ), 
                    new ParsedShort( "值 2" ), 
                    new ParsedShort( "值 3" ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.UShort1 => new ParsedBase[] {
                    new ParsedUInt( "值 1", size: 2 ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.UShort2 => new ParsedBase[] {
                    new ParsedUInt( "值 1", size: 2 ), 
                    new ParsedUInt( "值 2", size: 2 ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.UShort3 => new ParsedBase[] {
                    new ParsedUInt( "值 1", size: 2 ), 
                    new ParsedUInt( "值 2", size: 2 ), 
                    new ParsedUInt( "值 3", size: 2 ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.Int1 => new ParsedBase[] {
                    new ParsedInt( "值 1" )
                },
                KeyGroupType.Int2 => new ParsedBase[] {
                    new ParsedInt( "值 1" ), 
                    new ParsedInt( "值 2" )
                },
                KeyGroupType.Int3 => new ParsedBase[] {
                    new ParsedInt( "值 1" ), 
                    new ParsedInt( "值 2" ), 
                    new ParsedInt( "值 3" )
                },
                KeyGroupType.UInt1 => new ParsedBase[] {
                    new ParsedUInt( "值 1" )
                },
                KeyGroupType.UInt2 => new ParsedBase[] {
                    new ParsedUInt( "值 1" ), 
                    new ParsedUInt( "值 2" )
                },
                KeyGroupType.UInt3 => new ParsedBase[] {
                    new ParsedUInt( "值 1" ), 
                    new ParsedUInt( "值 2" ), 
                    new ParsedUInt( "值 3" )
                },
                KeyGroupType.Bool1 => new ParsedBase[] {
                    new ParsedByteBool( "值 1" ),
                    new ParsedReserve( 3 )
                },
                KeyGroupType.Bool2 => new ParsedBase[] {
                    new ParsedByteBool( "值 1" ), 
                    new ParsedByteBool( "值 2" ),
                    new ParsedReserve( 2 )
                },
                KeyGroupType.Bool3 => new ParsedBase[] {
                    new ParsedByteBool( "值 1" ), 
                    new ParsedByteBool( "值 2" ), 
                    new ParsedByteBool( "值 3" ),
                    new ParsedReserve( 1 )
                },
                KeyGroupType.Color => new ParsedBase[] {
                    new ParsedInt( "红色相乘", size: 2 ),
                    new ParsedInt( "绿色相乘", size: 2 ),
                    new ParsedInt( "蓝色相乘", size: 2 ),
                    new ParsedInt( "增加红色", size: 2 ),
                    new ParsedInt( "增加绿色", size: 2 ),
                    new ParsedInt( "增加蓝色", size: 2 )
                },
                KeyGroupType.Label => new ParsedBase[] {
                    new ParsedUInt( "标签 ID", size: 2 ), 
                    new ParsedInt( "标签命令", size: 1 ), 
                    new ParsedInt( "跳跃 ID", size: 1 )
                },
                _ => Array.Empty<ParsedBase>()
            } );
        }

        public UldKeyframe( BinaryReader reader, KeyGroupType groupType ) : this( groupType ) {
            var pos = reader.BaseStream.Position;

            Time.Read( reader );
            var size = reader.ReadUInt16();
            Interpolation.Read( reader );
            Unk1.Read( reader );
            Acceleration.Read( reader );
            Deceleration.Read( reader );

            Data.ForEach( x => x.Read( reader ) );

            reader.BaseStream.Position = pos + size;
        }

        public void Write( BinaryWriter writer ) {
            var pos = writer.BaseStream.Position;

            Time.Write( writer );

            var savePos = writer.BaseStream.Position;
            writer.Write( ( ushort )0 );

            Interpolation.Write( writer );
            Unk1.Write( writer );
            Acceleration.Write( writer );
            Deceleration.Write( writer );

            Data.ForEach( x => x.Write( writer ) );

            var finalPos = writer.BaseStream.Position;
            var size = finalPos - pos;
            writer.BaseStream.Position = savePos;
            writer.Write( ( ushort )size );
            writer.BaseStream.Position = finalPos;
        }

        public void Draw() {
            Time.Draw( CommandManager.Uld );
            Interpolation.Draw( CommandManager.Uld );
            Unk1.Draw( CommandManager.Uld );
            Acceleration.Draw( CommandManager.Uld );
            Deceleration.Draw( CommandManager.Uld );

            Data.ForEach( x => x.Draw( CommandManager.Uld ) );
        }
    }
}
