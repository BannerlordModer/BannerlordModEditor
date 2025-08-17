using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// ParticleSystems DO层的单元测试
    /// 专门测试核心修复功能的单元测试
    /// </summary>
    public class ParticleSystemsDOUnitTests
    {
        #region DecalMaterials 序列化测试

        [Fact]
        public void DecalMaterialsDO_ShouldSerialize_WhenHasEmptyDecalMaterials_IsTrue()
        {
            // Arrange
            var decalMaterials = new DecalMaterialsDO
            {
                HasEmptyDecalMaterials = true
            };

            // Act
            var shouldSerialize = decalMaterials.ShouldSerializeDecalMaterials();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void DecalMaterialsDO_ShouldSerialize_WhenHasEmptyDecalMaterials_IsFalse_And_ListIsEmpty()
        {
            // Arrange
            var decalMaterials = new DecalMaterialsDO
            {
                HasEmptyDecalMaterials = false,
                DecalMaterialList = new List<DecalMaterialDO>()
            };

            // Act
            var shouldSerialize = decalMaterials.ShouldSerializeDecalMaterials();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void DecalMaterialsDO_ShouldSerialize_WhenList_HasItems()
        {
            // Arrange
            var decalMaterials = new DecalMaterialsDO
            {
                HasEmptyDecalMaterials = false,
                DecalMaterialList = new List<DecalMaterialDO>
                {
                    new DecalMaterialDO { Value = "test_material" }
                }
            };

            // Act
            var shouldSerialize = decalMaterials.ShouldSerializeDecalMaterials();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void DecalMaterialDO_ShouldSerializeValue_WhenValue_IsNotNull()
        {
            // Arrange
            var decalMaterial = new DecalMaterialDO { Value = "test_value" };

            // Act
            var shouldSerialize = decalMaterial.ShouldSerializeValue();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void DecalMaterialDO_ShouldSerializeValue_WhenValue_IsNull()
        {
            // Arrange
            var decalMaterial = new DecalMaterialDO { Value = null };

            // Act
            var shouldSerialize = decalMaterial.ShouldSerializeValue();

            // Assert
            Assert.False(shouldSerialize);
        }

        #endregion

        #region ParameterDO 序列化测试

        [Fact]
        public void ParameterDO_ShouldSerializeBase_WhenBase_IsNotEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Base = "1.0" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeBase();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeBase_WhenBase_IsEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Base = "" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeBase();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeBase_WhenBase_IsNull()
        {
            // Arrange
            var parameter = new ParameterDO { Base = null };

            // Act
            var shouldSerialize = parameter.ShouldSerializeBase();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeBias_WhenBias_IsNotEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Bias = "0.5" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeBias();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeBias_WhenBias_IsEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Bias = "" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeBias();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeCurve_WhenCurve_IsNotEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Curve = "linear" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeCurve();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeCurve_WhenCurve_IsEmpty()
        {
            // Arrange
            var parameter = new ParameterDO { Curve = "" };

            // Act
            var shouldSerialize = parameter.ShouldSerializeCurve();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeParameterCurve_WhenParameterCurve_IsNotNull()
        {
            // Arrange
            var parameter = new ParameterDO 
            { 
                ParameterCurve = new CurveDO { Name = "test_curve" }
            };

            // Act
            var shouldSerialize = parameter.ShouldSerializeParameterCurve();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeParameterCurve_WhenParameterCurve_IsNull()
        {
            // Arrange
            var parameter = new ParameterDO { ParameterCurve = null };

            // Act
            var shouldSerialize = parameter.ShouldSerializeParameterCurve();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeParameterCurve_WhenHasEmptyCurve_IsTrue()
        {
            // Arrange
            var parameter = new ParameterDO 
            { 
                ParameterCurve = null,
                HasEmptyCurve = true
            };

            // Act
            var shouldSerialize = parameter.ShouldSerializeParameterCurve();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeColorElement_WhenHasEmptyColor_IsTrue()
        {
            // Arrange
            var parameter = new ParameterDO 
            { 
                ColorElement = null,
                HasEmptyColor = true
            };

            // Act
            var shouldSerialize = parameter.ShouldSerializeColorElement();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParameterDO_ShouldSerializeAlphaElement_WhenHasEmptyAlpha_IsTrue()
        {
            // Arrange
            var parameter = new ParameterDO 
            { 
                AlphaElement = null,
                HasEmptyAlpha = true
            };

            // Act
            var shouldSerialize = parameter.ShouldSerializeAlphaElement();

            // Assert
            Assert.True(shouldSerialize);
        }

        #endregion

        #region KeyDO 序列化测试

        [Fact]
        public void KeyDO_ShouldSerializePosition_WhenPosition_IsNotEmpty()
        {
            // Arrange
            var key = new KeyDO { Position = "0.5,0.5,0.5" };

            // Act
            var shouldSerialize = key.ShouldSerializePosition();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void KeyDO_ShouldSerializePosition_WhenPosition_IsEmpty()
        {
            // Arrange
            var key = new KeyDO { Position = "" };

            // Act
            var shouldSerialize = key.ShouldSerializePosition();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void KeyDO_ShouldSerializeTangent_WhenTangent_IsNotEmpty()
        {
            // Arrange
            var key = new KeyDO { Tangent = "1.0,1.0,1.0" };

            // Act
            var shouldSerialize = key.ShouldSerializeTangent();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void KeyDO_ShouldSerializeTangent_WhenTangent_IsEmpty()
        {
            // Arrange
            var key = new KeyDO { Tangent = "" };

            // Act
            var shouldSerialize = key.ShouldSerializeTangent();

            // Assert
            Assert.False(shouldSerialize);
        }

        #endregion

        #region CurveDO 序列化测试

        [Fact]
        public void CurveDO_ShouldSerializeVersion_WhenVersion_IsNotEmpty()
        {
            // Arrange
            var curve = new CurveDO { Version = "1.0" };

            // Act
            var shouldSerialize = curve.ShouldSerializeVersion();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeVersion_WhenVersion_IsEmpty()
        {
            // Arrange
            var curve = new CurveDO { Version = "" };

            // Act
            var shouldSerialize = curve.ShouldSerializeVersion();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeDefault_WhenDefault_IsNotEmpty()
        {
            // Arrange
            var curve = new CurveDO { Default = "0.5" };

            // Act
            var shouldSerialize = curve.ShouldSerializeDefault();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeDefault_WhenDefault_IsEmpty()
        {
            // Arrange
            var curve = new CurveDO { Default = "" };

            // Act
            var shouldSerialize = curve.ShouldSerializeDefault();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeCurveMultiplier_WhenCurveMultiplier_IsNotEmpty()
        {
            // Arrange
            var curve = new CurveDO { CurveMultiplier = "2.0" };

            // Act
            var shouldSerialize = curve.ShouldSerializeCurveMultiplier();

            // Act & Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeCurveMultiplier_WhenCurveMultiplier_IsEmpty()
        {
            // Arrange
            var curve = new CurveDO { CurveMultiplier = "" };

            // Act
            var shouldSerialize = curve.ShouldSerializeCurveMultiplier();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void CurveDO_ShouldSerializeKeys_WhenHasEmptyKeys_IsTrue()
        {
            // Arrange
            var curve = new CurveDO 
            { 
                Keys = null,
                HasEmptyKeys = true
            };

            // Act
            var shouldSerialize = curve.ShouldSerializeKeys();

            // Assert
            Assert.True(shouldSerialize);
        }

        #endregion

        #region EmitterDO 空元素处理测试

        [Fact]
        public void EmitterDO_ShouldSerializeChildren_WhenHasEmptyChildren_IsTrue()
        {
            // Arrange
            var emitter = new EmitterDO
            {
                HasEmptyChildren = true,
                Children = null
            };

            // Act
            var shouldSerialize = emitter.ShouldSerializeChildren();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void EmitterDO_ShouldSerializeChildren_WhenChildren_IsNotNull()
        {
            // Arrange
            var emitter = new EmitterDO
            {
                HasEmptyChildren = false,
                Children = new ChildrenDO()
            };

            // Act
            var shouldSerialize = emitter.ShouldSerializeChildren();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void EmitterDO_ShouldSerializeFlags_WhenHasEmptyFlags_IsTrue()
        {
            // Arrange
            var emitter = new EmitterDO
            {
                HasEmptyFlags = true,
                Flags = null
            };

            // Act
            var shouldSerialize = emitter.ShouldSerializeFlags();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void EmitterDO_ShouldSerializeParameters_WhenHasEmptyParameters_IsTrue()
        {
            // Arrange
            var emitter = new EmitterDO
            {
                HasEmptyParameters = true,
                Parameters = null
            };

            // Act
            var shouldSerialize = emitter.ShouldSerializeParameters();

            // Assert
            Assert.True(shouldSerialize);
        }

        #endregion

        #region ParametersDO 序列化测试

        [Fact]
        public void ParametersDO_ShouldSerializeDecalMaterials_WhenHasDecalMaterials_IsTrue()
        {
            // Arrange
            var parameters = new ParametersDO
            {
                HasDecalMaterials = true,
                DecalMaterials = null
            };

            // Act
            var shouldSerialize = parameters.ShouldSerializeDecalMaterials();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParametersDO_ShouldSerializeParameters_WhenHasEmptyParameters_IsTrue()
        {
            // Arrange
            var parameters = new ParametersDO
            {
                HasEmptyParameters = true,
                ParameterList = new List<ParameterDO>(),
                DecalMaterials = null
            };

            // Act
            var shouldSerialize = parameters.ShouldSerializeParameters();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParametersDO_ShouldSerializeParameters_WhenParameterList_HasItems()
        {
            // Arrange
            var parameters = new ParametersDO
            {
                HasEmptyParameters = false,
                ParameterList = new List<ParameterDO>
                {
                    new ParameterDO { Name = "test_param" }
                },
                DecalMaterials = null
            };

            // Act
            var shouldSerialize = parameters.ShouldSerializeParameters();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ParametersDO_ShouldSerializeParameters_WhenDecalMaterials_IsNotNull()
        {
            // Arrange
            var parameters = new ParametersDO
            {
                HasEmptyParameters = false,
                ParameterList = new List<ParameterDO>(),
                DecalMaterials = new DecalMaterialsDO()
            };

            // Act
            var shouldSerialize = parameters.ShouldSerializeParameters();

            // Assert
            Assert.True(shouldSerialize);
        }

        #endregion

        #region ColorDO 和 AlphaDO 序列化测试

        [Fact]
        public void ColorDO_ShouldSerializeKeys_WhenHasEmptyKeys_IsTrue()
        {
            // Arrange
            var color = new ColorDO 
            { 
                Keys = null,
                HasEmptyKeys = true
            };

            // Act
            var shouldSerialize = color.ShouldSerializeKeys();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void ColorDO_ShouldSerializeKeys_WhenKeys_IsNotNull()
        {
            // Arrange
            var color = new ColorDO 
            { 
                Keys = new KeysDO(),
                HasEmptyKeys = false
            };

            // Act
            var shouldSerialize = color.ShouldSerializeKeys();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void AlphaDO_ShouldSerializeKeys_WhenHasEmptyKeys_IsTrue()
        {
            // Arrange
            var alpha = new AlphaDO 
            { 
                Keys = null,
                HasEmptyKeys = true
            };

            // Act
            var shouldSerialize = alpha.ShouldSerializeKeys();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void AlphaDO_ShouldSerializeKeys_WhenKeys_IsNotNull()
        {
            // Arrange
            var alpha = new AlphaDO 
            { 
                Keys = new KeysDO(),
                HasEmptyKeys = false
            };

            // Act
            var shouldSerialize = alpha.ShouldSerializeKeys();

            // Assert
            Assert.True(shouldSerialize);
        }

        #endregion

        #region 边界条件测试

        [Fact]
        public void ParticleSystemsDO_ShouldSerializeEffects_WhenEffects_IsNull()
        {
            // Arrange
            var particleSystems = new ParticleSystemsDO { Effects = null };

            // Act
            var shouldSerialize = particleSystems.ShouldSerializeEffects();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParticleSystemsDO_ShouldSerializeEffects_WhenEffects_IsEmpty()
        {
            // Arrange
            var particleSystems = new ParticleSystemsDO { Effects = new List<EffectDO>() };

            // Act
            var shouldSerialize = particleSystems.ShouldSerializeEffects();

            // Assert
            Assert.False(shouldSerialize);
        }

        [Fact]
        public void ParticleSystemsDO_ShouldSerializeEffects_WhenEffects_HasItems()
        {
            // Arrange
            var particleSystems = new ParticleSystemsDO 
            { 
                Effects = new List<EffectDO> 
                { 
                    new EffectDO { Name = "test_effect" } 
                } 
            };

            // Act
            var shouldSerialize = particleSystems.ShouldSerializeEffects();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void EffectDO_ShouldSerializeName_WhenName_IsNotNull()
        {
            // Arrange
            var effect = new EffectDO { Name = "test_effect" };

            // Act
            var shouldSerialize = effect.ShouldSerializeName();

            // Assert
            Assert.True(shouldSerialize);
        }

        [Fact]
        public void EffectDO_ShouldSerializeName_WhenName_IsNull()
        {
            // Arrange
            var effect = new EffectDO { Name = null };

            // Act
            var shouldSerialize = effect.ShouldSerializeName();

            // Assert
            Assert.False(shouldSerialize);
        }

        #endregion

        #region XmlTestUtils 特殊处理测试

        [Fact]
        public void XmlTestUtils_Should_SetEmptyDecalMaterialsFlag_WhenXmlHasEmptyDecalMaterials()
        {
            // Arrange
            var xmlWithEmptyDecalMaterials = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test"">
        <emitters>
            <emitter name=""test_emitter"">
                <parameters>
                    <decal_materials></decal_materials>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xmlWithEmptyDecalMaterials);

            // Assert
            Assert.NotNull(obj);
            Assert.NotNull(obj.Effects);
            Assert.Single(obj.Effects);
            Assert.NotNull(obj.Effects[0].Emitters);
            Assert.Single(obj.Effects[0].Emitters!.EmitterList);
            Assert.NotNull(obj.Effects[0].Emitters!.EmitterList[0].Parameters);
            Assert.NotNull(obj.Effects[0].Emitters!.EmitterList[0].Parameters!.DecalMaterials);
            Assert.True(obj.Effects[0].Emitters!.EmitterList[0].Parameters!.DecalMaterials!.HasEmptyDecalMaterials);
        }

        [Fact]
        public void XmlTestUtils_Should_SetEmptyParametersFlag_WhenXmlHasEmptyParametersWithDecalMaterials()
        {
            // Arrange
            var xmlWithEmptyParameters = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test"">
        <emitters>
            <emitter name=""test_emitter"">
                <parameters>
                    <decal_materials>
                        <decal_material value=""test_material""/>
                    </decal_materials>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xmlWithEmptyParameters);

            // Assert
            Assert.NotNull(obj);
            Assert.NotNull(obj.Effects);
            Assert.Single(obj.Effects);
            Assert.NotNull(obj.Effects[0].Emitters);
            Assert.Single(obj.Effects[0].Emitters!.EmitterList);
            Assert.NotNull(obj.Effects[0].Emitters!.EmitterList[0].Parameters);
            Assert.True(obj.Effects[0].Emitters!.EmitterList[0].Parameters!.HasEmptyParameters);
            Assert.NotNull(obj.Effects[0].Emitters!.EmitterList[0].Parameters!.DecalMaterials);
        }

        #endregion
    }
}