using ImGuiNET;
using OtterGui.Raii;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AvfxFormat.Nodes;
using static VfxEditor.AvfxFormat.Enums;

namespace VfxEditor.AvfxFormat {
    public class AvfxParticle : AvfxNode {
        public const string NAME = "Ptcl";

        public readonly AvfxInt LoopStart = new( "循环开始", "LpSt" );
        public readonly AvfxInt LoopEnd = new( "循环结束", "LpEd" );
        public readonly AvfxEnum<ParticleType> ParticleVariety = new( "类型", "PrVT" );
        public readonly AvfxEnum<RotationDirectionBase> RotationDirectionBaseType = new( "旋转方向基准", "RBDT" );
        public readonly AvfxEnum<RotationOrder> RotationOrderType = new( "旋转计算顺序", "RoOT" );
        public readonly AvfxEnum<CoordComputeOrder> CoordComputeOrderType = new( "坐标计算顺序", "CCOT" );
        public readonly AvfxEnum<DrawMode> DrawModeType = new( "绘制模式", "RMT" );
        public readonly AvfxEnum<CullingType> CullingTypeType = new( "剔除类型", "CulT" );
        public readonly AvfxEnum<EnvLight> EnvLightType = new( "环境光", "EnvT" );
        public readonly AvfxEnum<DirLight> DirLightType = new( "定向光", "DirT" );
        public readonly AvfxEnum<UVPrecision> UvPrecisionType = new( "平面坐标精度", "UVPT" );
        public readonly AvfxInt DrawPriority = new( "绘制优先级", "DwPr" );
        public readonly AvfxBool IsDepthTest = new( "深度测试", "DsDt" );
        public readonly AvfxBool IsDepthWrite = new( "深度写入", "DsDw" );
        public readonly AvfxBool IsSoftParticle = new( "柔性粒子", "DsSp" );
        public readonly AvfxInt CollisionType = new( "碰撞类型", "Coll" );
        public readonly AvfxBool Bs11 = new( "BS11", "bS11" );
        public readonly AvfxBool IsApplyToneMap = new( "应用色调映射", "bATM" );
        public readonly AvfxBool IsApplyFog = new( "应用雾效", "bAFg" );
        public readonly AvfxBool ClipNearEnable = new( "启用近裁剪", "bNea" );
        public readonly AvfxBool ClipFarEnable = new( "启用远裁剪", "bFar" );
        public readonly AvfxFloat ClipNearStart = new( "近裁剪开始位置", "NeSt" ); // float2
        public readonly AvfxFloat ClipNearEnd = new( "近裁剪结束位置", "NeEd" );
        public readonly AvfxFloat ClipFarStart = new( "远裁剪开始位置", "FaSt" ); // float2
        public readonly AvfxFloat ClipFarEnd = new( "远裁剪结束位置", "FaEd" );
        public readonly AvfxEnum<ClipBasePoint> ClipBasePointType = new( "裁剪基准点", "FaBP" );
        public readonly AvfxInt UVSetCount = new( "平面坐标集数量", "UvSN" );
        public readonly AvfxInt ApplyRateEnvironment = new( "应用环境光强度", "EvAR" );
        public readonly AvfxInt ApplyRateDirectional = new( "应用定向光强度", "DlAR" );
        public readonly AvfxInt ApplyRateLightBuffer = new( "应用光缓冲率", "LBAR" );
        public readonly AvfxBool DOTy = new( "DOTy", "DOTy" );
        public readonly AvfxFloat DepthOffset = new( "深度偏移", "DpOf" );
        public readonly AvfxBool SimpleAnimEnable = new( "使用简易动画", "bSCt" );
        public readonly AvfxLife Life = new();
        public readonly AvfxCurve Gravity = new( "重力", "Gra" );
        public readonly AvfxCurve GravityRandom = new( "随机重力", "GraR" );
        public readonly AvfxCurve AirResistance = new( "空气阻力", "ARs", locked: true );
        public readonly AvfxCurve AirResistanceRandom = new( "随机空气阻力", "ARsR", locked: true );
        public readonly AvfxCurve3Axis Scale = new( "缩放", "Scl", locked: true );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle, locked: true );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos", locked: true );
        public readonly AvfxCurve RotVelX = new( "X 轴旋转速度", "VRX" );
        public readonly AvfxCurve RotVelY = new( "Y 轴旋转速度", "VRY" );
        public readonly AvfxCurve RotVelZ = new( "Z 轴旋转速度", "VRZ" );
        public readonly AvfxCurve RotVelXRandom = new( "随机 X 轴旋转速度", "VRXR" );
        public readonly AvfxCurve RotVelYRandom = new( "随机 Y 轴旋转速度", "VRYR" );
        public readonly AvfxCurve RotVelZRandom = new( "随机 Z 轴旋转速度", "VRZR" );
        public readonly AvfxCurveColor Color = new( "颜色", locked: true );
        public AvfxData Data;

        // initialize these later
        public readonly AvfxParticleTextureColor1 TC1;
        public readonly AvfxParticleTextureColor2 TC2;
        public readonly AvfxParticleTextureColor2 TC3;
        public readonly AvfxParticleTextureColor2 TC4;
        public readonly AvfxParticleTextureNormal TN;
        public readonly AvfxParticleTextureReflection TR;
        public readonly AvfxParticleTextureDistortion TD;
        public readonly AvfxParticleTexturePalette TP;
        public readonly AvfxParticleSimple Simple;

        public readonly List<AvfxParticleUvSet> UvSets = new();
        public readonly UiUvSetSplitView UvView;

        private readonly List<AvfxBase> Parsed;
        private readonly List<AvfxBase> Parsed2;

        public readonly AvfxNodeGroupSet NodeGroups;

        public readonly AvfxDisplaySplitView<AvfxItem> AnimationSplitDisplay;

        public readonly AvfxDisplaySplitView<AvfxItem> TextureDisplaySplit;

        private readonly UiDisplayList Parameters;

        public AvfxParticle( AvfxNodeGroupSet groupSet ) : base( NAME, AvfxNodeGroupSet.ParticleColor ) {
            NodeGroups = groupSet;

            // Initialize the remaining ones

            TC1 = new AvfxParticleTextureColor1( this );
            TC2 = new AvfxParticleTextureColor2( "材质颜色 2", "TC2", this );
            TC3 = new AvfxParticleTextureColor2( "材质颜色 3", "TC3", this );
            TC4 = new AvfxParticleTextureColor2( "材质颜色 4", "TC4", this );
            TN = new AvfxParticleTextureNormal( this );
            TR = new AvfxParticleTextureReflection( this );
            TD = new AvfxParticleTextureDistortion( this );
            TP = new AvfxParticleTexturePalette( this );
            Simple = new AvfxParticleSimple( this );

            // Parsing

            Parsed = new() {
                LoopStart,
                LoopEnd,
                ParticleVariety,
                RotationDirectionBaseType,
                RotationOrderType,
                CoordComputeOrderType,
                DrawModeType,
                CullingTypeType,
                EnvLightType,
                DirLightType,
                UvPrecisionType,
                DrawPriority,
                IsDepthTest,
                IsDepthWrite,
                IsSoftParticle,
                CollisionType,
                Bs11,
                IsApplyToneMap,
                IsApplyFog,
                ClipNearEnable,
                ClipFarEnable,
                ClipNearStart,
                ClipNearEnd,
                ClipFarStart,
                ClipFarEnd,
                ClipBasePointType,
                UVSetCount,
                ApplyRateEnvironment,
                ApplyRateDirectional,
                ApplyRateLightBuffer,
                DOTy,
                DepthOffset,
                SimpleAnimEnable,
                Life,
                Simple,
                Gravity,
                GravityRandom,
                AirResistance,
                AirResistanceRandom,
                Scale,
                Rotation,
                Position,
                RotVelX,
                RotVelY,
                RotVelZ,
                RotVelXRandom,
                RotVelYRandom,
                RotVelZRandom,
                Color
            };

            Parsed2 = new() {
                TC1,
                TC2,
                TC3,
                TC4,
                TN,
                TR,
                TD,
                TP
            };

            // Drawing

            Parameters = new( "参数", new() {
                new UiNodeGraphView( this ),
                LoopStart,
                LoopEnd,
                SimpleAnimEnable,
                RotationDirectionBaseType,
                RotationOrderType,
                CoordComputeOrderType,
                DrawModeType,
                CullingTypeType,
                EnvLightType,
                DirLightType,
                UvPrecisionType,
                DrawPriority,
                IsDepthTest,
                IsDepthWrite,
                IsSoftParticle,
                CollisionType,
                Bs11,
                IsApplyToneMap,
                IsApplyFog,
                ClipNearEnable,
                ClipFarEnable,
                new UiFloat2( "近裁剪", ClipNearStart, ClipNearEnd ),
                new UiFloat2( "远裁剪", ClipFarStart, ClipFarEnd ),
                ClipBasePointType,
                ApplyRateEnvironment,
                ApplyRateDirectional,
                ApplyRateLightBuffer,
                DOTy,
                DepthOffset
            } );

            AnimationSplitDisplay = new( "动画", new() {
                Life,
                Simple,
                Gravity,
                GravityRandom,
                AirResistance,
                AirResistanceRandom,
                Scale,
                Rotation,
                Position,
                RotVelX,
                RotVelY,
                RotVelZ,
                RotVelXRandom,
                RotVelYRandom,
                RotVelZRandom,
                Color
            } );

            UvView = new( UvSets );

            TextureDisplaySplit = new( "材质", new() {
                TC1,
                TC2,
                TC3,
                TC4,
                TN,
                TR,
                TD,
                TP
            } );

            ParticleVariety.Parsed.ExtraCommandGenerator = () => {
                return new AvfxParticleDataCommand( this );
            };
        }

        public override void ReadContents( BinaryReader reader, int size ) {
            Peek( reader, Parsed, size );
            Peek( reader, Parsed2, size );
            var particleType = ParticleVariety.GetValue();

            ReadNested( reader, ( BinaryReader _reader, string _name, int _size ) => {
                if( _name == "Data" ) {
                    SetData( particleType );
                    Data?.Read( _reader, _size );
                }
                else if( _name == "UvSt" ) {
                    var uvSet = new AvfxParticleUvSet();
                    uvSet.Read( _reader, _size );
                    UvSets.Add( uvSet );
                }
            }, size );

            UvView.UpdateIdx();
        }

        protected override void RecurseChildrenAssigned( bool assigned ) {
            RecurseAssigned( Parsed, assigned );
            RecurseAssigned( Data, assigned );
            RecurseAssigned( Parsed2, assigned );
        }

        protected override void WriteContents( BinaryWriter writer ) {
            UVSetCount.SetValue( UvSets.Count );
            WriteNested( writer, Parsed );

            foreach( var uvSet in UvSets ) uvSet.Write( writer );

            Data?.Write( writer );
            WriteNested( writer, Parsed2 );
        }

        public void SetData( ParticleType type ) {
            Data = type switch {
                ParticleType.Parameter => null,
                ParticleType.Powder => new AvfxParticleDataPowder(),
                ParticleType.Windmill => new AvfxParticleDataWindmill(),
                ParticleType.Line => new AvfxParticleDataLine(),
                ParticleType.Model => new AvfxParticleDataModel( this ),
                ParticleType.Polyline => new AvfxParticleDataPolyline(),
                ParticleType.Quad => null,
                ParticleType.Polygon => new AvfxParticleDataPolygon(),
                ParticleType.Decal => new AvfxParticleDataDecal(),
                ParticleType.DecalRing => new AvfxParticleDataDecalRing(),
                ParticleType.Disc => new AvfxParticleDataDisc(),
                ParticleType.LightModel => new AvfxParticleDataLightModel( this ),
                ParticleType.Laser => new AvfxParticleDataLaser(),
                _ => null,
            };
            Data?.SetAssigned( true );
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Particle" );
            DrawRename();
            ParticleVariety.Draw();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "参数" ) ) {
                if( tab ) Parameters.Draw();
            }

            DrawData();

            using( var tab = ImRaii.TabItem( "动画" ) ) {
                if( tab ) AnimationSplitDisplay.Draw();
            }

            using( var tab = ImRaii.TabItem( "平面坐标集" ) ) {
                if( tab ) UvView.Draw();
            }

            using( var tab = ImRaii.TabItem( "材质" ) ) {
                if( tab ) TextureDisplaySplit.Draw();
            }
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "Data" );
            if( !tabItem ) return;

            Data.Draw();
        }

        public override string GetDefaultText() => $"粒子 {GetIdx()} ({ParticleVariety.GetValue()})";

        public override string GetWorkspaceId() => $"Ptcl{GetIdx()}";
    }
}
