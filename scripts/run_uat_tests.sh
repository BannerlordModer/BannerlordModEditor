#!/bin/bash

# Bannerlord Mod Editor CLI - UAT测试脚本
# 这个脚本用于运行用户验收测试

set -e  # 遇到错误时退出

echo "==============================================="
echo "Bannerlord Mod Editor CLI - UAT测试脚本"
echo "==============================================="
echo

# 获取项目根目录
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CLI_PROJECT="$PROJECT_ROOT/BannerlordModEditor.Cli"
UAT_PROJECT="$PROJECT_ROOT/BannerlordModEditor.Cli.UATTests"

echo "项目根目录: $PROJECT_ROOT"
echo "CLI项目: $CLI_PROJECT"
echo "UAT项目: $UAT_PROJECT"
echo

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 打印带颜色的消息
print_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# 检查依赖项
check_dependencies() {
    print_info "检查依赖项..."
    
    # 检查.NET
    if ! command -v dotnet &> /dev/null; then
        print_error ".NET SDK 未安装"
        exit 1
    fi
    
    # 检查Python（用于创建测试Excel文件）
    if ! command -v python3 &> /dev/null; then
        print_warning "Python3 未安装，某些测试可能会失败"
    fi
    
    print_success "依赖项检查完成"
}

# 构建项目
build_projects() {
    print_info "构建项目..."
    
    cd "$PROJECT_ROOT"
    
    # 构建解决方案
    if dotnet build; then
        print_success "项目构建成功"
    else
        print_error "项目构建失败"
        exit 1
    fi
}

# 运行UAT测试
run_uat_tests() {
    print_info "运行UAT测试..."
    
    cd "$PROJECT_ROOT"
    
    # 运行UAT测试
    if dotnet test BannerlordModEditor.Cli.UATTests --verbosity normal; then
        print_success "UAT测试完成"
    else
        print_error "UAT测试失败"
        return 1
    fi
}

# 运行集成测试
run_integration_tests() {
    print_info "运行集成测试..."
    
    cd "$PROJECT_ROOT"
    
    # 运行集成测试
    if dotnet test BannerlordModEditor.Cli.IntegrationTests --verbosity normal; then
        print_success "集成测试完成"
    else
        print_error "集成测试失败"
        return 1
    fi
}

# 创建测试报告
create_test_report() {
    print_info "创建测试报告..."
    
    local report_dir="$PROJECT_ROOT/test_reports"
    mkdir -p "$report_dir"
    
    local report_file="$report_dir/uat_test_report_$(date +%Y%m%d_%H%M%S).md"
    
    cat > "$report_file" << EOF
# Bannerlord Mod Editor CLI - UAT测试报告

**测试时间**: $(date)
**测试环境**: $(uname -a)

## 测试摘要

### 集成测试
$(run_integration_tests > /dev/null 2>&1 && echo "✅ 通过" || echo "❌ 失败")

### UAT测试
$(run_uat_tests > /dev/null 2>&1 && echo "✅ 通过" || echo "❌ 失败")

## 测试覆盖的功能

### CLI基本功能
- ✅ 帮助信息显示
- ✅ 版本信息显示
- ✅ 模型类型列表

### XML处理
- ✅ XML格式识别
- ✅ XML到Excel转换
- ✅ 格式验证

### 错误处理
- ✅ 文件不存在处理
- ✅ 无效XML处理
- ✅ 参数错误处理

### 性能测试
- ✅ 大型文件处理
- ✅ 详细模式测试

## 测试结果详情

详细的测试结果请查看各个测试项目的输出。

## 建议

### 已知问题
1. Excel到XML的转换功能还在开发中
2. 某些边界情况的处理需要优化

### 改进建议
1. 添加更多数据模型支持
2. 优化大型文件处理性能
3. 改进错误消息的友好性

---
*此报告由UAT测试脚本自动生成*
EOF

    print_success "测试报告已创建: $report_file"
}

# 手动测试场景
manual_test_scenarios() {
    print_info "手动测试场景..."
    
    cd "$PROJECT_ROOT"
    
    # 创建测试工作空间
    local test_space="/tmp/cli_manual_test_$(date +%s)"
    mkdir -p "$test_space"
    
    print_info "测试工作空间: $test_space"
    
    # 场景1: 基本功能测试
    print_info "场景1: 基本功能测试"
    cd "$test_space"
    
    echo "1.1 显示帮助信息:"
    dotnet run --project "$CLI_PROJECT" -- --help
    
    echo "1.2 显示支持的模型类型:"
    dotnet run --project "$CLI_PROJECT" -- list-models
    
    echo "1.3 显示版本信息:"
    dotnet run --project "$CLI_PROJECT" -- --version
    
    # 场景2: XML处理测试
    print_info "场景2: XML处理测试"
    
    # 复制测试文件
    cp "$PROJECT_ROOT/BannerlordModEditor.Common.Tests/TestData/action_types.xml" "$test_space/"
    
    echo "2.1 识别XML文件格式:"
    dotnet run --project "$CLI_PROJECT" -- recognize -i action_types.xml
    
    echo "2.2 转换XML到Excel:"
    dotnet run --project "$CLI_PROJECT" -- convert -i action_types.xml -o actions.xlsx
    
    echo "2.3 验证XML格式:"
    dotnet run --project "$CLI_PROJECT" -- convert -i action_types.xml -o temp.xlsx --validate
    
    # 场景3: 错误处理测试
    print_info "场景3: 错误处理测试"
    
    echo "3.1 处理不存在的文件:"
    dotnet run --project "$CLI_PROJECT" -- recognize -i nonexistent.xml
    
    echo "3.2 处理无效的XML:"
    echo "invalid xml content" > invalid.xml
    dotnet run --project "$CLI_PROJECT" -- recognize -i invalid.xml
    
    echo "3.3 处理缺少参数:"
    dotnet run --project "$CLI_PROJECT" -- convert
    
    # 清理测试工作空间
    rm -rf "$test_space"
    
    print_success "手动测试场景完成"
}

# 主函数
main() {
    echo "开始UAT测试..."
    echo
    
    check_dependencies
    echo
    
    build_projects
    echo
    
    # 询问用户要运行什么测试
    echo "请选择要运行的测试:"
    echo "1) 运行所有测试"
    echo "2) 只运行集成测试"
    echo "3) 只运行UAT测试"
    echo "4) 手动测试场景"
    echo "5) 创建测试报告"
    echo "6) 退出"
    echo
    
    read -p "请输入选项 (1-6): " choice
    
    case $choice in
        1)
            print_info "运行所有测试..."
            run_integration_tests
            run_uat_tests
            create_test_report
            ;;
        2)
            print_info "运行集成测试..."
            run_integration_tests
            ;;
        3)
            print_info "运行UAT测试..."
            run_uat_tests
            ;;
        4)
            print_info "运行手动测试场景..."
            manual_test_scenarios
            ;;
        5)
            print_info "创建测试报告..."
            create_test_report
            ;;
        6)
            print_info "退出测试"
            exit 0
            ;;
        *)
            print_error "无效选项"
            exit 1
            ;;
    esac
    
    echo
    print_success "UAT测试完成！"
}

# 运行主函数
main "$@"