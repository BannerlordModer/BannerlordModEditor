#!/bin/bash

# Bannerlord Mod Editor CLI 使用示例脚本
# 此脚本演示了如何使用 Excel/XML 转换工具

echo "=== Bannerlord Mod Editor CLI 使用示例 ==="
echo

# 设置颜色输出
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 检查是否存在可执行文件
if [ ! -f "BannerlordModEditor.Cli/bin/Debug/net9.0/BannerlordModEditor.Cli" ]; then
    echo -e "${RED}错误：未找到可执行文件，请先构建项目${NC}"
    echo "运行: dotnet build"
    exit 1
fi

# 设置可执行文件路径
CLI_PATH="BannerlordModEditor.Cli/bin/Debug/net9.0/BannerlordModEditor.Cli"

# 创建临时测试目录
TEST_DIR="/tmp/bannerlord_test_$$"
mkdir -p "$TEST_DIR"

echo -e "${BLUE}创建测试目录: $TEST_DIR${NC}"
echo

# 示例 1: 列出支持的模型类型
echo -e "${YELLOW}示例 1: 列出支持的模型类型${NC}"
echo "命令: $CLI_PATH list-models"
$CLI_PATH list-models
echo

# 示例 2: 搜索特定模型
echo -e "${YELLOW}示例 2: 搜索特定模型${NC}"
echo "命令: $CLI_PATH list-models --search Combat"
$CLI_PATH list-models --search Combat
echo

# 示例 3: 识别现有 XML 文件格式
echo -e "${YELLOW}示例 3: 识别 XML 文件格式${NC}"
if [ -f "BannerlordModEditor.Common.Tests/TestData/action_types.xml" ]; then
    echo "命令: $CLI_PATH recognize -i \"BannerlordModEditor.Common.Tests/TestData/action_types.xml\""
    $CLI_PATH recognize -i "BannerlordModEditor.Common.Tests/TestData/action_types.xml"
else
    echo -e "${RED}跳过：未找到测试 XML 文件${NC}"
fi
echo

# 创建测试 Excel 文件
echo -e "${YELLOW}创建测试 Excel 文件...${NC}"
cat > "$TEST_DIR/test_actions.py" << 'EOF'
import pandas as pd

# 创建测试数据
data = {
    'name': ['test_action_1', 'test_action_2', 'test_action_3'],
    'type': ['test_type_1', 'test_type_2', 'test_type_3'],
    'usage_direction': ['forward', 'backward', 'side'],
    'action_stage': ['start', 'loop', 'end']
}

df = pd.DataFrame(data)
df.to_excel('$TEST_DIR/test_actions.xlsx', index=False)
print("测试 Excel 文件已创建")
EOF

python3 "$TEST_DIR/test_actions.py" 2>/dev/null || echo -e "${RED}跳过：未找到 Python/pandas${NC}"

# 示例 4: Excel 转 XML
echo -e "${YELLOW}示例 4: Excel 转 XML${NC}"
if [ -f "$TEST_DIR/test_actions.xlsx" ]; then
    echo "命令: $CLI_PATH convert -i \"$TEST_DIR/test_actions.xlsx\" -o \"$TEST_DIR/test_actions.xml\" -m \"ActionTypesDO\""
    $CLI_PATH convert -i "$TEST_DIR/test_actions.xlsx" -o "$TEST_DIR/test_actions.xml" -m "ActionTypesDO"
    
    if [ -f "$TEST_DIR/test_actions.xml" ]; then
        echo -e "${GREEN}转换成功！查看生成的 XML 文件:${NC}"
        head -10 "$TEST_DIR/test_actions.xml"
    fi
else
    echo -e "${RED}跳过：未找到测试 Excel 文件${NC}"
fi
echo

# 示例 5: XML 转 Excel
echo -e "${YELLOW}示例 5: XML 转 Excel${NC}"
if [ -f "$TEST_DIR/test_actions.xml" ]; then
    echo "命令: $CLI_PATH convert -i \"$TEST_DIR/test_actions.xml\" -o \"$TEST_DIR/test_actions_roundtrip.xlsx\""
    $CLI_PATH convert -i "$TEST_DIR/test_actions.xml" -o "$TEST_DIR/test_actions_roundtrip.xlsx"
    
    if [ -f "$TEST_DIR/test_actions_roundtrip.xlsx" ]; then
        echo -e "${GREEN}转换成功！${NC}"
    fi
else
    echo -e "${RED}跳过：未找到测试 XML 文件${NC}"
fi
echo

# 示例 6: 格式验证
echo -e "${YELLOW}示例 6: 格式验证${NC}"
if [ -f "$TEST_DIR/test_actions.xlsx" ]; then
    echo "命令: $CLI_PATH convert -i \"$TEST_DIR/test_actions.xlsx\" -o \"$TEST_DIR/test_actions.xml\" -m \"ActionTypesDO\" --validate"
    $CLI_PATH convert -i "$TEST_DIR/test_actions.xlsx" -o "$TEST_DIR/test_actions.xml" -m "ActionTypesDO" --validate
else
    echo -e "${RED}跳过：未找到测试 Excel 文件${NC}"
fi
echo

# 示例 7: 详细输出
echo -e "${YELLOW}示例 7: 详细输出${NC}"
if [ -f "BannerlordModEditor.Common.Tests/TestData/action_types.xml" ]; then
    echo "命令: $CLI_PATH convert -i \"BannerlordModEditor.Common.Tests/TestData/action_types.xml\" -o \"$TEST_DIR/actions_detailed.xlsx\" --verbose"
    $CLI_PATH convert -i "BannerlordModEditor.Common.Tests/TestData/action_types.xml" -o "$TEST_DIR/actions_detailed.xlsx" --verbose
else
    echo -e "${RED}跳过：未找到测试 XML 文件${NC}"
fi
echo

# 清理测试文件
echo -e "${YELLOW}清理测试文件...${NC}"
rm -rf "$TEST_DIR"
echo -e "${GREEN}清理完成${NC}"
echo

echo -e "${BLUE}=== 示例完成 ===${NC}"
echo
echo "更多使用方法请参考文档: docs/Excel_XML_Conversion_Guide.md"
echo

# 显示帮助信息
echo -e "${YELLOW}显示帮助信息:${NC}"
echo "命令: $CLI_PATH --help"
$CLI_PATH --help