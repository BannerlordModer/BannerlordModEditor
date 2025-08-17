#!/bin/bash

# BannerIconsMapper测试套件执行脚本
# 用于运行全面的BannerIcons相关测试

set -e

echo "=========================================="
echo "BannerIconsMapper测试套件执行脚本"
echo "=========================================="

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 函数：打印带颜色的消息
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

# 检查测试数据文件
check_test_data() {
    print_info "检查测试数据文件..."
    
    if [ ! -f "BannerlordModEditor.Common.Tests/TestData/banner_icons.xml" ]; then
        print_warning "测试数据文件不存在: BannerlordModEditor.Common.Tests/TestData/banner_icons.xml"
        print_warning "部分集成测试将被跳过"
        return 1
    fi
    
    print_success "测试数据文件检查通过"
    return 0
}

# 运行构建
build_solution() {
    print_info "构建解决方案..."
    
    if dotnet build BannerlordModEditor.sln --configuration Release; then
        print_success "解决方案构建成功"
    else
        print_error "解决方案构建失败"
        exit 1
    fi
}

# 运行单元测试
run_unit_tests() {
    print_info "运行单元测试..."
    
    echo "----------------------------------------"
    echo "1. BannerIconsMapperTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsMapperTests" --logger "console;verbosity=detailed"; then
        print_success "BannerIconsMapperTests 通过"
    else
        print_error "BannerIconsMapperTests 失败"
        return 1
    fi
    
    echo "----------------------------------------"
    echo "2. BannerIconsTypeConversionTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsTypeConversionTests" --logger "console;verbosity=detailed"; then
        print_success "BannerIconsTypeConversionTests 通过"
    else
        print_error "BannerIconsTypeConversionTests 失败"
        return 1
    fi
    
    echo "----------------------------------------"
    echo "3. BannerIconsEmptyElementsTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsEmptyElementsTests" --logger "console;verbosity=detailed"; then
        print_success "BannerIconsEmptyElementsTests 通过"
    else
        print_error "BannerIconsEmptyElementsTests 失败"
        return 1
    fi
    
    echo "----------------------------------------"
    echo "4. BannerIconsBoundaryConditionsTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsBoundaryConditionsTests" --logger "console;verbosity=detailed"; then
        print_success "BannerIconsBoundaryConditionsTests 通过"
    else
        print_error "BannerIconsBoundaryConditionsTests 失败"
        return 1
    fi
    
    print_success "所有单元测试通过"
}

# 运行集成测试
run_integration_tests() {
    print_info "运行集成测试..."
    
    echo "----------------------------------------"
    echo "5. BannerIconsIntegrationTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsIntegrationTests" --logger "console;verbosity=detailed"; then
        print_success "BannerIconsIntegrationTests 通过"
    else
        print_error "BannerIconsIntegrationTests 失败"
        return 1
    fi
    
    print_success "集成测试通过"
}

# 运行性能测试
run_performance_tests() {
    print_info "运行性能测试..."
    
    echo "----------------------------------------"
    echo "6. BannerIconsPerformanceTests"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --filter "BannerIconsPerformanceTests" --logger "console;verbosity=minimal"; then
        print_success "BannerIconsPerformanceTests 通过"
    else
        print_error "BannerIconsPerformanceTests 失败"
        return 1
    fi
    
    print_success "性能测试通过"
}

# 运行代码覆盖度分析
run_coverage_analysis() {
    print_info "运行代码覆盖度分析..."
    
    echo "----------------------------------------"
    echo "代码覆盖度分析"
    echo "----------------------------------------"
    if dotnet test BannerlordModEditor.Common.Tests --collect:"XPlat Code Coverage" --logger "console;verbosity=minimal"; then
        print_success "代码覆盖度分析完成"
        print_info "覆盖度报告位置: TestResults/coverage.cobertura.xml"
    else
        print_error "代码覆盖度分析失败"
        return 1
    fi
}

# 运行完整测试套件
run_full_test_suite() {
    print_info "运行完整测试套件..."
    
    # 检查测试数据
    check_test_data
    
    # 构建解决方案
    build_solution
    
    # 运行测试
    run_unit_tests
    run_integration_tests
    run_performance_tests
    
    # 生成覆盖度报告
    run_coverage_analysis
    
    print_success "完整测试套件执行完成"
}

# 运行快速测试
run_quick_tests() {
    print_info "运行快速测试（单元测试）..."
    
    build_solution
    run_unit_tests
    
    print_success "快速测试完成"
}

# 显示帮助信息
show_help() {
    echo "使用方法:"
    echo "  $0 [选项]"
    echo ""
    echo "选项:"
    echo "  full     运行完整测试套件（默认）"
    echo "  quick    运行快速测试（仅单元测试）"
    echo "  unit     仅运行单元测试"
    echo "  integration 仅运行集成测试"
    echo "  performance 仅运行性能测试"
    echo "  coverage 仅运行代码覆盖度分析"
    echo "  help     显示此帮助信息"
    echo ""
    echo "示例:"
    echo "  $0 full          # 运行完整测试套件"
    echo "  $0 quick         # 运行快速测试"
    echo "  $0 unit          # 仅运行单元测试"
}

# 主执行逻辑
main() {
    case "${1:-full}" in
        "full")
            run_full_test_suite
            ;;
        "quick")
            run_quick_tests
            ;;
        "unit")
            build_solution
            run_unit_tests
            ;;
        "integration")
            build_solution
            run_integration_tests
            ;;
        "performance")
            build_solution
            run_performance_tests
            ;;
        "coverage")
            build_solution
            run_coverage_analysis
            ;;
        "help")
            show_help
            ;;
        *)
            print_error "未知选项: $1"
            show_help
            exit 1
            ;;
    esac
}

# 检查是否在正确的目录中
if [ ! -f "BannerlordModEditor.sln" ]; then
    print_error "请在BannerlordModEditor项目根目录中运行此脚本"
    exit 1
fi

# 执行主函数
main "$@"

echo "=========================================="
echo "测试执行完成"
echo "=========================================="