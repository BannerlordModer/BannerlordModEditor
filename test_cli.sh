#!/bin/bash

# 测试脚本：Bannerlord Mod Editor CLI 功能测试

echo "=== Bannerlord Mod Editor CLI 功能测试 ==="
echo

# 获取项目根目录
PROJECT_ROOT="/root/WorkSpace/CSharp/BME/BannerlordModEditor-CLI"
cd "$PROJECT_ROOT"

# 测试基本命令
echo "1. 测试基本命令..."
echo "1.1 列出支持的模型类型："
dotnet run -- list-models
echo

echo "1.2 显示帮助信息："
dotnet run -- --help
echo

# 测试识别功能
echo "2. 测试XML识别功能..."
echo "2.1 识别 action_types.xml："
dotnet run -- recognize -i "BannerlordModEditor.Common.Tests/TestData/action_types.xml"
echo

echo "2.2 识别 combat_parameters.xml："
dotnet run -- recognize -i "BannerlordModEditor.Common.Tests/TestData/combat_parameters.xml"
echo

# 测试转换功能
echo "3. 测试转换功能..."
echo "3.1 XML 转 Excel："
dotnet run -- convert -i "BannerlordModEditor.Common.Tests/TestData/action_types.xml" -o "/tmp/test_actions.xlsx"
echo

if [ -f "/tmp/test_actions.xlsx" ]; then
    echo "✓ Excel 文件创建成功"
    
    echo "3.2 验证 Excel 格式："
    dotnet run -- convert -i "/tmp/test_actions.xlsx" -o "/tmp/test_output.xml" -m "ActionTypesDO" --validate
    
    echo "3.3 Excel 转 XML："
    dotnet run -- convert -i "/tmp/test_actions.xlsx" -o "/tmp/test_output.xml" -m "ActionTypesDO"
    
    if [ -f "/tmp/test_output.xml" ]; then
        echo "✓ XML 文件创建成功"
        echo "3.4 验证生成的XML文件："
        dotnet run -- recognize -i "/tmp/test_output.xml"
    else
        echo "✗ XML 文件创建失败"
    fi
else
    echo "✗ Excel 文件创建失败"
fi

echo
echo "=== 测试完成 ==="