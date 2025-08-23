#!/bin/bash

# XML适配状态检查工具测试运行脚本
# 此脚本运行所有测试并生成覆盖率报告

set -e

echo "=========================================="
echo "XML适配状态检查工具测试套件"
echo "=========================================="

# 获取脚本所在目录
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 日志函数
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# 检查依赖
check_dependencies() {
    log_info "检查依赖..."
    
    # 检查.NET SDK
    if ! command -v dotnet &> /dev/null; then
        log_error "未找到.NET SDK，请先安装.NET 9.0 SDK"
        exit 1
    fi
    
    # 检查dotnet版本
    local dotnet_version=$(dotnet --version | grep -oE '^[0-9]+\.[0-9]+')
    if [[ "$dotnet_version" != "9.0" ]]; then
        log_warning "当前.NET版本为$dotnet_version，建议使用.NET 9.0"
    fi
    
    # 检查是否安装了测试工具
    if ! dotnet tool list --global | grep -q "coverlet"; then
        log_info "安装coverlet代码覆盖率工具..."
        dotnet tool install --global coverlet.console
    fi
    
    log_success "依赖检查完成"
}

# 构建项目
build_project() {
    log_info "构建项目..."
    
    cd "$PROJECT_DIR"
    
    # 恢复依赖
    log_info "恢复NuGet包..."
    dotnet restore
    
    # 构建项目
    log_info "编译项目..."
    dotnet build --configuration Release --no-restore
    
    log_success "项目构建完成"
}

# 运行单元测试
run_unit_tests() {
    log_info "运行单元测试..."
    
    cd "$PROJECT_DIR"
    
    # 创建测试结果目录
    mkdir -p test-results
    
    # 运行单元测试
    log_info "执行单元测试..."
    dotnet test \
        --configuration Release \
        --no-build \
        --logger "trx;LogFileName=test-results/unit-tests.trx" \
        --logger "console;verbosity=minimal" \
        --collect:"XPlat Code Coverage" \
        --results-directory test-results \
        --filter "TestCategory!=Integration&TestCategory!=Performance"
    
    log_success "单元测试完成"
}

# 运行集成测试
run_integration_tests() {
    log_info "运行集成测试..."
    
    cd "$PROJECT_DIR"
    
    # 运行集成测试
    log_info "执行集成测试..."
    dotnet test \
        --configuration Release \
        --no-build \
        --logger "trx;LogFileName=test-results/integration-tests.trx" \
        --logger "console;verbosity=minimal" \
        --filter "TestCategory=Integration"
    
    log_success "集成测试完成"
}

# 运行性能测试
run_performance_tests() {
    log_info "运行性能测试..."
    
    cd "$PROJECT_DIR"
    
    # 运行性能测试
    log_info "执行性能测试..."
    dotnet test \
        --configuration Release \
        --no-build \
        --logger "trx;LogFileName=test-results/performance-tests.trx" \
        --logger "console;verbosity=minimal" \
        --filter "TestCategory=Performance"
    
    log_success "性能测试完成"
}

# 生成覆盖率报告
generate_coverage_report() {
    log_info "生成代码覆盖率报告..."
    
    cd "$PROJECT_DIR"
    
    # 检查是否有覆盖率文件
    if [ -f "test-results/coverage.xml" ]; then
        log_info "生成HTML覆盖率报告..."
        
        # 生成HTML报告（如果有reportgenerator工具）
        if command -v reportgenerator &> /dev/null; then
            reportgenerator \
                -reports:test-results/coverage.xml \
                -targetdir:test-results/coverage-report \
                -reporttypes:HtmlInline_AzurePipelines
            log_success "HTML覆盖率报告已生成到 test-results/coverage-report/"
        else
            log_warning "未找到reportgenerator工具，跳过HTML报告生成"
        fi
    else
        log_warning "未找到覆盖率数据文件"
    fi
}

# 运行所有测试
run_all_tests() {
    log_info "运行所有测试..."
    
    cd "$PROJECT_DIR"
    
    # 创建测试结果目录
    mkdir -p test-results
    
    # 运行所有测试
    log_info "执行所有测试..."
    dotnet test \
        --configuration Release \
        --no-build \
        --logger "trx;LogFileName=test-results/all-tests.trx" \
        --logger "console;verbosity=minimal" \
        --collect:"XPlat Code Coverage" \
        --results-directory test-results
    
    log_success "所有测试完成"
}

# 显示测试结果摘要
show_test_summary() {
    log_info "测试结果摘要"
    
    cd "$PROJECT_DIR"
    
    if [ -d "test-results" ]; then
        echo "测试结果文件:"
        find test-results -name "*.trx" -exec echo "  - {}" \;
        
        echo ""
        echo "覆盖率文件:"
        find test-results -name "coverage*" -exec echo "  - {}" \;
        
        echo ""
        echo "报告目录:"
        echo "  - test-results/"
    fi
}

# 清理测试文件
cleanup_test_files() {
    log_info "清理测试文件..."
    
    cd "$PROJECT_DIR"
    
    # 保留重要的测试结果，清理临时文件
    if [ -d "test-results" ]; then
        find test-results -name "*.tmp" -delete 2>/dev/null || true
        find test-results -name "*.log" -delete 2>/dev/null || true
    fi
    
    log_success "清理完成"
}

# 显示帮助信息
show_help() {
    echo "用法: $0 [选项]"
    echo ""
    echo "选项:"
    echo "  -h, --help          显示帮助信息"
    echo "  -u, --unit          只运行单元测试"
    echo "  -i, --integration   只运行集成测试"
    echo "  -p, --performance   只运行性能测试"
    echo "  -a, --all           运行所有测试（默认）"
    echo "  -c, --coverage      生成覆盖率报告"
    echo "  -b, --build         只构建项目"
    echo "  --clean             清理测试文件"
    echo ""
    echo "示例:"
    echo "  $0                  运行所有测试"
    echo "  $0 -u              只运行单元测试"
    echo "  $0 -i -c           运行集成测试并生成覆盖率报告"
}

# 主函数
main() {
    local run_unit=true
    local run_integration=true
    local run_performance=true
    local generate_coverage=false
    local build_only=false
    local clean_only=false
    
    # 解析命令行参数
    while [[ $# -gt 0 ]]; do
        case $1 in
            -h|--help)
                show_help
                exit 0
                ;;
            -u|--unit)
                run_unit=true
                run_integration=false
                run_performance=false
                shift
                ;;
            -i|--integration)
                run_unit=false
                run_integration=true
                run_performance=false
                shift
                ;;
            -p|--performance)
                run_unit=false
                run_integration=false
                run_performance=true
                shift
                ;;
            -a|--all)
                run_unit=true
                run_integration=true
                run_performance=true
                shift
                ;;
            -c|--coverage)
                generate_coverage=true
                shift
                ;;
            -b|--build)
                build_only=true
                shift
                ;;
            --clean)
                clean_only=true
                shift
                ;;
            *)
                log_error "未知选项: $1"
                show_help
                exit 1
                ;;
        esac
    done
    
    # 清理模式
    if [ "$clean_only" = true ]; then
        cleanup_test_files
        exit 0
    fi
    
    # 显示开始信息
    echo "测试开始时间: $(date)"
    echo "项目目录: $PROJECT_DIR"
    echo ""
    
    # 检查依赖
    check_dependencies
    
    # 构建项目
    build_project
    
    # 如果只是构建，则退出
    if [ "$build_only" = true ]; then
        log_success "项目构建完成"
        exit 0
    fi
    
    # 运行测试
    if [ "$run_unit" = true ]; then
        run_unit_tests
    fi
    
    if [ "$run_integration" = true ]; then
        run_integration_tests
    fi
    
    if [ "$run_performance" = true ]; then
        run_performance_tests
    fi
    
    # 生成覆盖率报告
    if [ "$generate_coverage" = true ]; then
        generate_coverage_report
    fi
    
    # 显示测试结果摘要
    show_test_summary
    
    # 显示结束信息
    echo ""
    echo "测试完成时间: $(date)"
    log_success "测试套件执行完成！"
}

# 捕获退出信号
trap 'log_error "脚本执行被中断"; exit 1' INT TERM

# 运行主函数
main "$@"