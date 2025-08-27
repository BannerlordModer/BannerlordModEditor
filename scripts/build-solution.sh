#!/bin/bash

# BannerlordModEditor 解决方案构建脚本
# 解决多个项目文件导致的dotnet命令歧义问题

set -e  # 遇到错误时退出

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 脚本目录
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"

echo -e "${BLUE}🚀 BannerlordModEditor 解决方案构建脚本${NC}"
echo -e "${BLUE}===========================================${NC}"
echo ""

# 检查是否在正确的目录中
if [ ! -f "$PROJECT_ROOT/BannerlordModEditor.sln" ]; then
    echo -e "${RED}❌ 错误: 未找到解决方案文件 BannerlordModEditor.sln${NC}"
    echo -e "${RED}   请确保在项目根目录中运行此脚本${NC}"
    exit 1
fi

# 显示帮助信息
show_help() {
    echo -e "${YELLOW}用法: $0 [命令]${NC}"
    echo ""
    echo -e "${YELLOW}可用命令:${NC}"
    echo -e "  restore    - 还原所有项目依赖"
    echo -e "  build      - 构建整个解决方案"
    echo -e "  clean      - 清理解决方案"
    echo -e "  test       - 运行所有测试"
    echo -e "  test-ui    - 运行UI测试"
    echo -e "  test-common- 运行Common测试"
    echo -e "  run        - 运行UI应用程序"
    echo -e "  help       - 显示此帮助信息"
    echo ""
    echo -e "${YELLOW}示例:${NC}"
    echo -e "  $0 restore"
    echo -e "  $0 build"
    echo -e "  $0 test-ui"
}

# 还原依赖
restore_dependencies() {
    echo -e "${GREEN}📦 还原项目依赖...${NC}"
    cd "$PROJECT_ROOT"
    dotnet restore BannerlordModEditor.sln
    echo -e "${GREEN}✅ 依赖还原完成${NC}"
}

# 构建解决方案
build_solution() {
    echo -e "${GREEN}🔨 构建解决方案...${NC}"
    cd "$PROJECT_ROOT"
    dotnet build BannerlordModEditor.sln --configuration Debug
    echo -e "${GREEN}✅ 解决方案构建完成${NC}"
}

# 清理解决方案
clean_solution() {
    echo -e "${GREEN}🧹 清理解决方案...${NC}"
    cd "$PROJECT_ROOT"
    dotnet clean BannerlordModEditor.sln
    echo -e "${GREEN}✅ 解决方案清理完成${NC}"
}

# 运行所有测试
run_all_tests() {
    echo -e "${GREEN}🧪 运行所有测试...${NC}"
    cd "$PROJECT_ROOT"
    dotnet test BannerlordModEditor.sln --verbosity normal
    echo -e "${GREEN}✅ 所有测试完成${NC}"
}

# 运行UI测试
run_ui_tests() {
    echo -e "${GREEN}🎨 运行UI测试...${NC}"
    cd "$PROJECT_ROOT"
    dotnet test BannerlordModEditor.UI.Tests --verbosity normal
    echo -e "${GREEN}✅ UI测试完成${NC}"
}

# 运行Common测试
run_common_tests() {
    echo -e "${GREEN}⚙️ 运行Common测试...${NC}"
    cd "$PROJECT_ROOT"
    dotnet test BannerlordModEditor.Common.Tests --verbosity normal
    echo -e "${GREEN}✅ Common测试完成${NC}"
}

# 运行应用程序
run_application() {
    echo -e "${GREEN}🚀 启动应用程序...${NC}"
    cd "$PROJECT_ROOT"
    dotnet run --project BannerlordModEditor.UI --configuration Debug
}

# 主逻辑
case "${1:-help}" in
    "restore")
        restore_dependencies
        ;;
    "build")
        build_solution
        ;;
    "clean")
        clean_solution
        ;;
    "test")
        run_all_tests
        ;;
    "test-ui")
        run_ui_tests
        ;;
    "test-common")
        run_common_tests
        ;;
    "run")
        run_application
        ;;
    "help"|*)
        show_help
        ;;
esac

echo ""
echo -e "${BLUE}💡 提示: 使用 '$0 help' 查看所有可用命令${NC}"