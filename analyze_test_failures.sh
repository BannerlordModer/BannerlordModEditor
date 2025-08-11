#!/bin/bash

echo "分析测试失败原因详细报告"
echo "================================"

# 运行测试并捕获完整输出
output=$(timeout 60s dotnet test BannerlordModEditor.Common.Tests/BannerlordModEditor.Common.Tests.csproj --no-build --verbosity normal 2>&1)

# 保存输出到文件进行分析
echo "$output" > /tmp/detailed_test_analysis.txt

echo "获取失败测试列表..."
echo "================================"

# 提取所有失败测试
echo "$output" | grep "\[FAIL\]" | while read -r line; do
    # 提取测试名称
    test_name=$(echo "$line" | sed -n 's/.*\[FAIL\] *\(.*\)/\1/p')
    echo "失败测试: $test_name"
done

echo ""
echo "分析失败模式..."
echo "================================"

# 统计不同类型的失败
echo "1. 按测试文件分类失败测试："
echo "$output" | grep "\[FAIL\]" | sed -n 's/.*\[FAIL\] *\(.*\)\.Tests\.\(.*\)/\1/p' | sort | uniq -c

echo ""
echo "2. 按测试功能分类失败测试："
echo "$output" | grep "\[FAIL\]" | sed -n 's/.*\[FAIL\] *\(.*\)\.Tests\.\(.*\)Tests\.\(.*\)/\2/p' | sort | uniq -c

echo ""
echo "3. 按测试方法分类失败测试（提取常见模式）："
echo "$output" | grep "\[FAIL\]" | sed -n 's/.*\[FAIL\] *\(.*\)\.Tests\.\(.*\)/\2/p' | sed -n 's/.*_\(.*\)/\1/p' | grep -E "(RoundTrip|StructuralEquality|ShouldSerialize|DeserializeAndSerialize)" | sort | uniq -c

echo ""
echo "获取几个关键失败的详细错误信息..."
echo "================================"

# 获取前10个失败的详细错误信息
echo "$output" | grep -A10 -B2 "\[FAIL\]" | head -50

echo ""
echo "识别最常见的失败类型..."
echo "================================"

# 检查是否主要是RoundTrip测试失败
roundtrip_failures=$(echo "$output" | grep -c "\[FAIL\]" | grep -c "RoundTrip" || echo "0")
equality_failures=$(echo "$output" | grep -c "\[FAIL\]" | grep -c "StructuralEquality" || echo "0")
serialization_failures=$(echo "$output" | grep -c "\[FAIL\]" | grep -c "Serialize\|Deserialize" || echo "0")

echo "RoundTrip测试失败数量: $roundtrip_failures"
echo "StructuralEquality测试失败数量: $equality_failures"
echo "序列化相关测试失败数量: $serialization_failures"

echo ""
echo "识别哪些XML类型最常失败..."
echo "================================"

# 统计按XML类型分类的失败
echo "$output" | grep "\[FAIL\]" | sed -n 's/.*\[FAIL\] *\(.*\)Tests\.\(.*\)/\1/p' | sort | uniq -c | sort -nr

echo ""
echo "分析完成。详细结果保存在 /tmp/detailed_test_analysis.txt"