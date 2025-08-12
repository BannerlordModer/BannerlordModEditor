#!/bin/bash

echo "分析测试失败原因并生成报告"
echo "=========================="

# 获取测试输出并保存到文件
timeout 60s dotnet test BannerlordModEditor.Common.Tests/BannerlordModEditor.Common.Tests.csproj --no-build --verbosity normal 2>&1 > /tmp/test_output_full.txt

echo "从测试输出中提取关键信息..."

# 提取失败测试数量
failed_count=$(grep -c "\[FAIL\]" /tmp/test_output_full.txt)
echo "失败测试总数: $failed_count"

# 提取不同类型的失败
structural_failures=$(grep -c "RoundTrip_StructuralEquality\|StructuralEquality" /tmp/test_output_full.txt)
equality_failures=$(grep -c "Assert.Equal\|EqualFailure" /tmp/test_output_full.txt)
roundtrip_failures=$(grep -c "RoundTrip\|Roundtrip" /tmp/test_output_full.txt)

echo "StructuralEquality测试失败: $structural_failures"
echo "Value Equality测试失败: $equality_failures"
echo "RoundTrip测试失败: $roundtrip_failures"

# 提取前10个失败测试的详细信息
echo ""
echo "前10个失败测试的详细信息:"
echo "=========================="
grep -A3 -B1 "\[FAIL\]" /tmp/test_output_full.txt | head -50

# 按XML类型统计失败
echo ""
echo "按XML类型分类的失败统计:"
echo "=========================="
echo "从测试输出中识别XML类型..."
grep "\.Tests\." /tmp/test_output_full.txt | grep "FAIL" | sed -n 's/.*\.Tests\.\(.*\)Tests\..*/\1/p' | sort | uniq -c

# 生成总结报告
report_file="/root/WorkSpace/CSharp/BannerlordModEditor/TEST_FAILURE_ANALYSIS.md"
echo "# 测试失败分析报告" > "$report_file"
echo "" >> "$report_file"
echo "## 概要" >> "$report_file"
echo "" >> "$report_file"
echo "- **总失败测试数**: $failed_count" >> "$report_file"
echo "- **StructuralEquality失败**: $structural_failures" >> "$report_file"
echo "- **Value Equality失败**: $equality_failures" >> "$report_file"
echo "- **RoundTrip失败**: $roundtrip_failures" >> "$report_file"
echo "" >> "$report_file"

echo "## 主要失败模式" >> "$report_file"
echo "" >> "$report_file"
echo "### 1. StructuralEquality失败 (最常见的失败类型)" >> "$report_file"
echo "**原因**: 反序列化→序列化→再反序列化过程中，两次反序列化的结果不一致" >> "$report_file"
echo "**可能原因**:" >> "$report_file"
echo "- 序列化过程中丢失XML属性" >> "$report_file"
echo "- 属性顺序发生变化" >> "$report_file"
echo "- 空值处理不一致" >> "$report_file"
echo "- XML格式化差异影响结构比较" >> "$report_file"
echo "" >> "$report_file"

echo "### 2. Value Equality失败" >> "$report_file"
echo "**原因**: 值比较失败，期望值与实际值不匹配" >> "$report_file"
echo "**可能原因**:" >> "$report_file"
echo "- XML值解析错误" >> "$report_file"
echo "- 数据类型转换问题" >> "$report_file"
echo "- 默认值处理不当" >> "$report_file"
echo "- Specified属性设置问题" >> "$report_file"
echo "" >> "$report_file"

echo "## 修复建议" >> "$report_file"
echo "" >> "$report_file"
echo "### 优先级修复列表" >> "$report_file"
echo "1. **高优先级**: StructuralEquality失败修复 - 影响XML序列化核心功能" >> "$report_file"
echo "2. **中优先级**: ParticleSystems相关XML修复 - 复杂嵌套结构" >> "$report_file"
echo "3. **中优先级**: ActionTypes和Item相关XML修复 - 游戏核心配置" >> "$report_file"
echo "4. **低优先级**: 其他辅助配置XML修复" >> "$report_file"
echo "" >> "$report_file"

echo "### 通用修复策略" >> "$report_file"
echo "1. 检查ShouldSerialize方法实现" >> "$report_file"
echo "2. 验证属性默认值设置" >> "$report_file"
echo "3. 修复Specified属性处理" >> "$report_file"
echo "4. 统一XML格式化策略" >> "$report_file"
echo "" >> "$report_file"

echo "报告已生成: $report_file"
echo "完整测试输出保存在: /tmp/test_output_full.txt"