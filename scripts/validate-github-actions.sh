#!/bin/bash

# GitHub Actions 配置验证脚本
# 用于验证本地测试是否能正确生成.trx文件和覆盖率报告

echo "开始验证 GitHub Actions 配置..."

# 清理之前的测试结果
echo "清理之前的测试结果..."
rm -rf TestResults/
rm -rf coverage/
rm -f *.trx

# 创建测试结果目录
mkdir -p TestResults

echo "=============================================="
echo "1. 运行单元测试"
echo "=============================================="
dotnet test BannerlordModEditor.Common.Tests \
    --configuration Release \
    --verbosity normal \
    --collect:"XPlat Code Coverage" \
    --results-directory TestResults \
    --logger "trx;LogFileName=unit_tests.trx"

echo "=============================================="
echo "2. 运行UI测试"
echo "=============================================="
dotnet test BannerlordModEditor.UI.Tests \
    --configuration Release \
    --verbosity normal \
    --collect:"XPlat Code Coverage" \
    --results-directory TestResults \
    --logger "trx;LogFileName=ui_tests.trx"

echo "=============================================="
echo "3. 生成覆盖率报告"
echo "=============================================="
# 安装报告生成工具
dotnet tool install -g dotnet-reportgenerator-globaltool

# 生成覆盖率报告
reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/CoverageReport -reporttypes:Html

echo "=============================================="
echo "4. 验证测试结果"
echo "=============================================="
echo "生成的 .trx 文件："
find TestResults -name "*.trx" -type f

echo ""
echo "生成的覆盖率文件："
find TestResults -name "*.xml" -type f

echo ""
echo "覆盖率报告目录："
if [ -d "TestResults/CoverageReport" ]; then
    echo "✅ 覆盖率报告已生成"
    ls -la TestResults/CoverageReport/
else
    echo "❌ 覆盖率报告未生成"
fi

echo "=============================================="
echo "5. 检查文件结构"
echo "=============================================="
echo "TestResults 目录结构："
tree TestResults/ 2>/dev/null || ls -la TestResults/

echo ""
echo "验证完成！"
echo "如果所有文件都正确生成，说明 GitHub Actions 配置应该可以正常工作。"