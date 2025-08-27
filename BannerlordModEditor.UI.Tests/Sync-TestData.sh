#!/bin/bash

# 同步测试数据从Common.Tests到UI.Tests
# Shell脚本版本

set -e  # 遇到错误立即退出

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# 默认路径
SOURCE_PATH="../BannerlordModEditor.Common.Tests/TestData"
DEST_PATH="./TestData"
FORCE=false

# 解析命令行参数
while [[ $# -gt 0 ]]; do
    case $1 in
        -f|--force)
            FORCE=true
            shift
            ;;
        -s|--source)
            SOURCE_PATH="$2"
            shift 2
            ;;
        -d|--destination)
            DEST_PATH="$2"
            shift 2
            ;;
        -h|--help)
            echo "用法: $0 [选项]"
            echo "选项:"
            echo "  -f, --force     强制覆盖已存在的文件"
            echo "  -s, --source    指定源路径 (默认: ../BannerlordModEditor.Common.Tests/TestData)"
            echo "  -d, --destination 指定目标路径 (默认: ./TestData)"
            echo "  -h, --help      显示此帮助信息"
            exit 0
            ;;
        *)
            echo "未知参数: $1"
            exit 1
            ;;
    esac
done

echo -e "${GREEN}开始同步测试数据...${NC}"
echo -e "${YELLOW}源路径: $SOURCE_PATH${NC}"
echo -e "${YELLOW}目标路径: $DEST_PATH${NC}"

# 检查源路径是否存在
if [[ ! -d "$SOURCE_PATH" ]]; then
    echo -e "${RED}错误: 源路径不存在: $SOURCE_PATH${NC}"
    exit 1
fi

# 创建目标目录（如果不存在）
if [[ ! -d "$DEST_PATH" ]]; then
    echo -e "${YELLOW}创建目标目录: $DEST_PATH${NC}"
    mkdir -p "$DEST_PATH"
fi

# 定义需要复制的文件列表
required_files=(
    "attributes.xml"
    "bone_body_types.xml"
    "skills.xml"
    "module_sounds.xml"
    "crafting_pieces.xml"
    "item_modifiers.xml"
)

# 同步文件
files_copied=0
files_skipped=0
files_not_found=0

for file in "${required_files[@]}"; do
    source_file="$SOURCE_PATH/$file"
    dest_file="$DEST_PATH/$file"
    
    echo -e "${CYAN}处理文件: $file${NC}"
    
    if [[ -f "$source_file" ]]; then
        if [[ -f "$dest_file" ]] && [[ "$FORCE" != true ]]; then
            echo -e "${YELLOW}  跳过: 文件已存在 (使用 -f 参数强制覆盖)${NC}"
            ((files_skipped++))
        else
            if cp "$source_file" "$dest_file"; then
                echo -e "${GREEN}  ✓ 已复制${NC}"
                ((files_copied++))
            else
                echo -e "${RED}  复制失败${NC}"
                exit 1
            fi
        fi
    else
        echo -e "${RED}  警告: 源文件不存在: $source_file${NC}"
        ((files_not_found++))
    fi
done

# 显示摘要
echo ""
echo -e "${GREEN}同步完成!${NC}"
echo -e "${GREEN}已复制: $files_copied 个文件${NC}"
echo -e "${YELLOW}已跳过: $files_skipped 个文件${NC}"
if [[ $files_not_found -gt 0 ]]; then
    echo -e "${RED}未找到: $files_not_found 个文件${NC}"
    echo -e "${YELLOW}警告: 某些必需的测试文件不存在，请检查Common.Tests/TestData目录${NC}"
fi

# 验证结果
echo ""
echo -e "${CYAN}验证目标目录...${NC}"
if ls "$DEST_PATH"/*.xml 1> /dev/null 2>&1; then
    echo -e "${CYAN}目标目录中的XML文件:$(ls "$DEST_PATH"/*.xml | xargs -n 1 basename | tr '\n' ' ')${NC}"
else
    echo -e "${YELLOW}目标目录中没有XML文件${NC}"
fi

# 检查是否有缺失的文件
missing_files=()
for file in "${required_files[@]}"; do
    if [[ ! -f "$DEST_PATH/$file" ]]; then
        missing_files+=("$file")
    fi
done

if [[ ${#missing_files[@]} -gt 0 ]]; then
    echo -e "${RED}缺失的文件: ${missing_files[*]}${NC}"
    exit 1
fi

echo -e "${GREEN}所有必需的测试文件都已同步完成!${NC}"