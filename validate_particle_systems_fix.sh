#!/bin/bash

echo "🧪 开始验证ParticleSystems XML序列化修复效果..."
echo "📁 当前目录: $(pwd)"

# 1. 首先构建项目
echo "🔨 构建项目..."
dotnet build BannerlordModEditor.Common.Tests

if [ $? -eq 0 ]; then
    echo "✅ 构建成功"
else
    echo "❌ 构建失败"
    exit 1
fi

# 2. 运行快速测试
echo "🧪 运行ParticleSystemsQuickTest..."
dotnet test BannerlordModEditor.Common.Tests --filter "ParticleSystemsQuickTest" --verbosity normal

if [ $? -eq 0 ]; then
    echo "✅ 快速测试通过"
else
    echo "❌ 快速测试失败"
fi

# 3. 运行原始问题测试
echo "🧪 运行ParticleSystemsHardcodedMisc1XmlTests..."
dotnet test BannerlordModEditor.Common.Tests --filter "ParticleSystemsHardcodedMisc1XmlTests" --verbosity normal

if [ $? -eq 0 ]; then
    echo "✅ ParticleSystemsHardcodedMisc1XmlTests通过！修复成功！"
else
    echo "❌ ParticleSystemsHardcodedMisc1XmlTests失败，需要进一步调查"
fi

# 4. 运行所有ParticleSystems测试
echo "🧪 运行所有ParticleSystems测试..."
dotnet test BannerlordModEditor.Common.Tests --filter "ParticleSystems" --verbosity normal

if [ $? -eq 0 ]; then
    echo "✅ 所有ParticleSystems测试通过！"
else
    echo "❌ 部分ParticleSystems测试失败"
fi

echo "🎉 验证完成！"