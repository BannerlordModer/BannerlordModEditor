#!/bin/bash

echo "运行测试并统计结果..."

# 运行测试并捕获完整输出
output=$(timeout 30s dotnet test BannerlordModEditor.Common.Tests/BannerlordModEditor.Common.Tests.csproj --no-build --verbosity normal 2>&1)

# 保存输出到文件进行分析
echo "$output" > /tmp/test_output.txt

echo "测试输出内容："
echo "==================="
echo "$output" | tail -20

echo ""
echo "分析测试统计..."

# 尝试不同的统计模式
echo "检查英文统计模式..."
echo "$output" | grep -E "Tests run:|Passed:|Failed:|Skipped:" | tail -3

echo "检查中文统计模式..."
echo "$output" | grep -E "测试总数:|通过数:|失败数:|跳过数:" | tail -3

# 检查失败测试数量
echo ""
echo "检查失败测试数量..."
failed_count=$(echo "$output" | grep -c "\[FAIL\]" || echo "0")
echo "失败测试数量: $failed_count"

# 检查通过测试数量
echo ""
echo "检查通过测试数量..."
passed_count=$(echo "$output" | grep -c "\[PASS\]" || echo "0")
echo "通过测试数量: $passed_count"

# 检查跳过测试数量
echo ""
echo "检查跳过测试数量..."
skipped_count=$(echo "$output" | grep -c "\[SKIP\]" || echo "0")
echo "跳过测试数量: $skipped_count"

echo ""
echo "总结："
echo "==================="
echo "失败测试: $failed_count"
echo "通过测试: $passed_count" 
echo "跳过测试: $skipped_count"
echo "总计: $(($failed_count + $passed_count + $skipped_count))"