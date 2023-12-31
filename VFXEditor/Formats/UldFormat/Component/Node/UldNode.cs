﻿using Dalamud.Logging;
using ImGuiNET;
using OtterGui.Raii;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.Parsing;
using VfxEditor.UldFormat.Component.Node.Data;
using VfxEditor.UldFormat.Component.Node.Data.Component;

namespace VfxEditor.UldFormat.Component.Node {
    public enum NodeType : int {
        Container = 1,
        Image = 2,
        Text = 3,
        NineGrid = 4,
        Counter = 5,
        Collision = 8,
    }

    [Flags]
    public enum NodeFlags {
        Visible = 0x01,
        Enabled = 0x02,
        Clip = 0x04,
        Fill = 0x08,
        AnchorTop = 0x10,
        AnchorBottom = 0x20,
        AnchorLeft = 0x40,
        AnchorRight = 0x80
    }

    public class UldNode : UldWorkspaceItem {
        private readonly List<UldComponent> Components;
        private readonly UldWorkspaceItem Parent;

        public readonly ParsedInt ParentId = new( "父级 ID" );
        public readonly ParsedInt NextSiblingId = new( "下一兄弟节点 ID" );
        public readonly ParsedInt PrevSiblingId = new( "前一兄弟节点 ID" );
        public readonly ParsedInt ChildNodeId = new( "子级节点 ID" );

        public bool IsComponentNode = false;
        public readonly ParsedEnum<NodeType> Type = new( "Type" );
        public readonly ParsedInt ComponentTypeId = new( "组件 ID" );
        public UldGenericData Data = null;

        private readonly List<ParsedBase> Parsed;
        public readonly ParsedShort TabIndex = new( "标签页索引" );
        public readonly ParsedInt Unk1 = new( "未知 1" );
        public readonly ParsedInt Unk2 = new( "未知 2" );
        public readonly ParsedInt Unk3 = new( "未知 3" );
        public readonly ParsedInt Unk4 = new( "未知 4" );
        public readonly ParsedShort X = new( "X" );
        public readonly ParsedShort Y = new( "Y" );
        public readonly ParsedUInt W = new( "宽度", size: 2 );
        public readonly ParsedUInt H = new( "高度", size: 2 );
        public readonly ParsedRadians Rotation = new( "旋转" );
        public readonly ParsedFloat2 Scale = new( "缩放" );
        public readonly ParsedShort OriginX = new( "原点 X" );
        public readonly ParsedShort OriginY = new( "原点 Y" );
        public readonly ParsedUInt Priority = new( "优先级", size: 2 );
        public readonly ParsedFlag<NodeFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedInt Unk7 = new( "未知 7", size: 1 );
        public readonly ParsedShort MultiplyRed = new( "红色相乘" );
        public readonly ParsedShort MultiplyGreen = new( "绿色相乘" );
        public readonly ParsedShort MultiplyBlue = new( "蓝色相乘" );
        public readonly ParsedShort AddRed = new( "增加红色" );
        public readonly ParsedShort AddGreen = new( "增加绿色" );
        public readonly ParsedShort AddBlue = new( "增加蓝色" );
        public readonly ParsedInt Alpha = new( "Alpha", size: 1 );
        public readonly ParsedInt ClipCount = new( "片段数", size: 1 );
        public readonly ParsedUInt TimelineId = new( "时间线 ID", size: 2 );

        // need to wait until all components are initialized, so store this until then
        private readonly long DelayedPosition;
        private readonly int DelayedSize;
        private readonly int DelayedNodeType;

        public UldNode( List<UldComponent> components, UldWorkspaceItem parent ) {
            Parent = parent;
            Components = components;
            Type.ExtraCommandGenerator = () => {
                return new UldNodeDataCommand( this );
            };

            Parsed = new() {
                TabIndex,
                Unk1,
                Unk2,
                Unk3,
                Unk4,
                X,
                Y,
                W,
                H,
                Rotation,
                Scale,
                OriginX,
                OriginY,
                Priority,
                Flags,
                Unk7,
                MultiplyRed,
                MultiplyGreen,
                MultiplyBlue,
                AddRed,
                AddGreen,
                AddBlue,
                Alpha,
                ClipCount,
                TimelineId
            };
        }

        public UldNode( BinaryReader reader, List<UldComponent> components, UldWorkspaceItem parent ) : this( components, parent ) {
            var pos = reader.BaseStream.Position;

            Id.Read( reader );
            ParentId.Read( reader );
            NextSiblingId.Read( reader );
            PrevSiblingId.Read( reader );
            ChildNodeId.Read( reader );

            var nodeType = reader.ReadInt32();
            var size = reader.ReadUInt16();
            // TODO: what if offset <= 88

            if( nodeType > 1000 ) {
                IsComponentNode = true;
                ComponentTypeId.Value = nodeType;
            }
            else {
                Type.Value = ( NodeType )nodeType;
            }

            Parsed.ForEach( x => x.Read( reader ) );

            DelayedPosition = reader.BaseStream.Position;
            DelayedSize = ( int )( pos + size - reader.BaseStream.Position ) - 12;
            DelayedNodeType = nodeType;

            reader.BaseStream.Position = pos + size;
        }

        // Needs to be initialized later since it depends on component list being filled
        public void InitData( BinaryReader reader ) {
            reader.BaseStream.Position = DelayedPosition;

            UpdateData();
            if( Data == null && DelayedNodeType > 1 ) {
                PluginLog.Log( $"未知节点类型 {DelayedNodeType} / {DelayedSize} @ {reader.BaseStream.Position:X8}" );
            }
            if( Data is BaseNodeData custom ) custom.Read( reader, DelayedSize );
            else Data?.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            var pos = writer.BaseStream.Position;

            Id.Write( writer );
            ParentId.Write( writer );
            NextSiblingId.Write( writer );
            PrevSiblingId.Write( writer );
            ChildNodeId.Write( writer );

            if( IsComponentNode ) ComponentTypeId.Write( writer );
            else Type.Write( writer );

            var savePos = writer.BaseStream.Position;
            writer.Write( ( ushort )0 );

            Parsed.ForEach( x => x.Write( writer ) );
            Data?.Write( writer );

            var finalPos = writer.BaseStream.Position;
            var size = finalPos - pos;
            writer.BaseStream.Position = savePos;
            writer.Write( ( ushort )size );
            writer.BaseStream.Position = finalPos;
        }

        public void UpdateData() {
            if( IsComponentNode ) {
                var component = Components.Where( x => x.Id.Value == ComponentTypeId.Value ).FirstOrDefault();
                if( component == null ) Data = null;
                else {
                    Data = component.Type.Value switch {
                        //ComponentType.Custom => new CustomNodeData(),
                        ComponentType.Button => new ButtonNodeData(),
                        ComponentType.Window => new WindowNodeData(),
                        ComponentType.CheckBox => new CheckboxNodeData(),
                        ComponentType.RadioButton => new RadioButtonNodeData(),
                        ComponentType.Gauge => new GaugeNodeData(),
                        ComponentType.Slider => new SliderNodeData(),
                        ComponentType.TextInput => new TextInputNodeData(),
                        ComponentType.NumericInput => new NumericInputNodeData(),
                        ComponentType.List => new ListNodeData(),
                        ComponentType.Tabbed => new TabbedNodeData(),
                        ComponentType.ListItem => new ListItemNodeData(),
                        ComponentType.NineGridText => new NineGridTextNodeData(),
                        _ => new UldNodeComponentData()
                    };
                }
            }
            else {
                Data = Type.Value switch {
                    NodeType.Image => new ImageNodeData(),
                    NodeType.Text => new TextNodeData(),
                    NodeType.NineGrid => new NineGridNodeData(),
                    NodeType.Counter => new CounterNodeData(),
                    NodeType.Collision => new CollisionNodeData(),
                    _ => null
                };
            }
        }

        public override void Draw() {
            DrawRename();
            Id.Draw( CommandManager.Uld );

            if( ImGui.Checkbox( "是否为组件节点", ref IsComponentNode ) ) CommandManager.Uld.Add( new UldNodeDataCommand( this, true ) );

            if( IsComponentNode ) {
                ComponentTypeId.Draw( CommandManager.Uld );
                ImGui.SameLine();
                if( ImGui.SmallButton( "刷新" ) ) CommandManager.Uld.Add( new UldNodeDataCommand( this ) );
            }
            else Type.Draw( CommandManager.Uld );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawParameters();
            DrawData();
        }

        private void DrawParameters() {
            using var tabItem = ImRaii.TabItem( "参数" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Parameters" );
            using var child = ImRaii.Child( "Child" );

            ParentId.Draw( CommandManager.Uld );
            NextSiblingId.Draw( CommandManager.Uld );
            PrevSiblingId.Draw( CommandManager.Uld );
            ChildNodeId.Draw( CommandManager.Uld );

            Parsed.ForEach( x => x.Draw( CommandManager.Uld ) );
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "数据" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Data" );
            using var child = ImRaii.Child( "Child" );

            Data.Draw();
        }

        public override string GetDefaultText() {
            var suffix = IsComponentNode ? ComponentTypeId.Value.ToString() : Type.Value.ToString();
            return $"节点 {GetIdx()} ({suffix})";
        }

        public override string GetWorkspaceId() => $"{Parent.GetWorkspaceId()}/Node{GetIdx()}";
    }
}
