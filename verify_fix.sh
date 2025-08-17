#!/bin/bash

echo "🧪 开始验证ParticleSystems修复效果..."

# 进入项目根目录
cd /root/WorkSpace/CSharp/BannerlordModEditor

echo "📁 当前目录: $(pwd)"

# 1. 首先尝试构建项目
echo "🔨 构建项目..."
dotnet build BannerlordModEditor.Common.Tests

if [ $? -eq 0 ]; then
    echo "✅ 构建成功"
else
    echo "❌ 构建失败"
    exit 1
fi

# 2. 运行特定的ParticleSystems测试
echo "🧪 运行ParticleSystems测试..."
dotnet test BannerlordModEditor.Common.Tests --filter "ParticleSystems" --verbosity normal

if [ $? -eq 0 ]; then
    echo "✅ ParticleSystems测试通过！修复成功！"
else
    echo "❌ ParticleSystems测试失败，需要进一步调查"
fi

# 3. 运行我们的自定义测试程序
echo "🧪 运行自定义测试程序..."
if [ -f "TestParticleSystems.cs" ]; then
    echo "📝 编译测试程序..."
    dotnet run --project TestParticleSystems.csproj
else
    echo "⚠️ 自定义测试程序不存在"
fi

echo "🎉 测试完成！"