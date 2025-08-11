#!/bin/bash

echo "提取测试失败详细信息"
echo "======================"

# 从测试输出中提取失败测试及其错误信息
failed_tests_file="/tmp/failed_tests_analysis.txt"
> "$failed_tests_file"  # 清空文件

# 运行测试并捕获输出
timeout 60s dotnet test BannerlordModEditor.Common.Tests/BannerlordModEditor.Common.Tests.csproj --no-build --verbosity normal 2>&1 | \
while IFS= read -r line; do
    # 检查是否是失败测试行
    if [[ "$line" =~ \[FAIL\] ]]; then
        # 提取测试名称
        test_name=$(echo "$line" | sed -n 's/.*\[FAIL\][[:space:]]*\(.*\)/\1/p')
        echo "失败测试: $test_name" >> "$failed_tests_file"
        echo "错误详情:" >> "$failed_tests_file"
    elif [[ "$line" =~ (Assert\.True\(\) Failure|Assert\.Equal\(\) Failure|Expected:|Actual:) ]]; then
        # 记录错误详情
        echo "  $line" >> "$failed_tests_file"
    elif [[ "$line" =~ Stack[[:space:]]Trace: ]]; then
        # 记录堆栈跟踪开始
        echo "  $line" >> "$failed_tests_file"
    elif [[ "$line" =~ (at[[:space:]].*Tests\.|at[[:space:]]System\.) ]]; then
        # 记录堆栈跟踪详情
        echo "    $line" >> "$failed_tests_file"
    fi
done

echo "分析完成，结果保存在 $failed_tests_file"
echo "前20行内容："
head -20 "$failed_tests_file"
echo ""
echo "失败测试总数："
grep -c "失败测试:" "$failed_tests_file"