#!/bin/bash

# BannerlordModEditor 测试执行脚本
# 用于运行完整的测试套件

set -e  # 遇到错误时退出

echo "========================================="
echo "BannerlordModEditor 全面测试套件执行"
echo "========================================="

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
    
    if ! command -v dotnet &> /dev/null; then
        log_error ".NET SDK 未安装"
        exit 1
    fi
    
    dotnet_version=$(dotnet --version)
    log_success ".NET 版本: $dotnet_version"
}

# 清理和构建
clean_and_build() {
    log_info "清理和构建项目..."
    
    dotnet clean
    if [ $? -ne 0 ]; then
        log_error "清理失败"
        exit 1
    fi
    
    dotnet restore
    if [ $? -ne 0 ]; then
        log_error "依赖恢复失败"
        exit 1
    fi
    
    dotnet build --configuration Release
    if [ $? -ne 0 ]; then
        log_error "构建失败"
        exit 1
    fi
    
    log_success "构建完成"
}

# 创建测试结果目录
create_test_results_dir() {
    local results_dir="TestResults"
    
    if [ -d "$results_dir" ]; then
        rm -rf "$results_dir"
    fi
    
    mkdir -p "$results_dir"
    log_success "测试结果目录创建完成: $results_dir"
}

# 运行单元测试
run_unit_tests() {
    log_info "运行单元测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --collect:"XPlat Code Coverage" \
        --results-directory TestResults \
        --logger "trx;LogFileName=unit_tests.trx" \
        --filter "Category!=LargeXml&Category!=Memory&Category!=Performance"
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "单元测试通过 (${duration}s)"
    else
        log_error "单元测试失败 (${duration}s)"
        return 1
    fi
}

# 运行集成测试
run_integration_tests() {
    log_info "运行集成测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=Integration" \
        --logger "trx;LogFileName=integration_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "集成测试通过 (${duration}s)"
    else
        log_error "集成测试失败 (${duration}s)"
        return 1
    fi
}

# 运行性能测试
run_performance_tests() {
    log_info "运行性能测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=Performance" \
        --logger "trx;LogFileName=performance_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "性能测试通过 (${duration}s)"
    else
        log_error "性能测试失败 (${duration}s)"
        return 1
    fi
}

# 运行大型XML测试
run_large_xml_tests() {
    log_info "运行大型XML测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=LargeXml" \
        --logger "trx;LogFileName=large_xml_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "大型XML测试通过 (${duration}s)"
    else
        log_error "大型XML测试失败 (${duration}s)"
        return 1
    fi
}

# 运行内存测试
run_memory_tests() {
    log_info "运行内存测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=Memory" \
        --logger "trx;LogFileName=memory_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "内存测试通过 (${duration}s)"
    else
        log_error "内存测试失败 (${duration}s)"
        return 1
    fi
}

# 运行错误处理测试
run_error_handling_tests() {
    log_info "运行错误处理测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=ErrorHandling" \
        --logger "trx;LogFileName=error_handling_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "错误处理测试通过 (${duration}s)"
    else
        log_error "错误处理测试失败 (${duration}s)"
        return 1
    fi
}

# 运行并发测试
run_concurrency_tests() {
    log_info "运行并发测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=Concurrency" \
        --logger "trx;LogFileName=concurrency_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "并发测试通过 (${duration}s)"
    else
        log_error "并发测试失败 (${duration}s)"
        return 1
    fi
}

# 运行回归测试
run_regression_tests() {
    log_info "运行回归测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=Regression" \
        --logger "trx;LogFileName=regression_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "回归测试通过 (${duration}s)"
    else
        log_error "回归测试失败 (${duration}s)"
        return 1
    fi
}

# 运行用户体验测试
run_user_experience_tests() {
    log_info "运行用户体验测试..."
    
    local start_time=$(date +%s)
    
    dotnet test BannerlordModEditor.Common.Tests \
        --configuration Release \
        --verbosity normal \
        --filter "Category=UserExperience" \
        --logger "trx;LogFileName=user_experience_tests.trx" \
        --results-directory TestResults
    
    local exit_code=$?
    local end_time=$(date +%s)
    local duration=$((end_time - start_time))
    
    if [ $exit_code -eq 0 ]; then
        log_success "用户体验测试通过 (${duration}s)"
    else
        log_error "用户体验测试失败 (${duration}s)"
        return 1
    fi
}

# 生成测试报告
generate_test_report() {
    log_info "生成测试报告..."
    
    # 生成覆盖率报告
    if command -v reportgenerator &> /dev/null; then
        reportgenerator -reports:TestResults/coverage.xml -targetdir:TestResults/CoverageReport -reporttypes:Html
        log_success "覆盖率报告生成完成"
    else
        log_warning "reportgenerator 未安装，跳过覆盖率报告生成"
    fi
    
    # 创建测试汇总报告
    cat > TestResults/test_summary.md << EOF
# 测试执行汇总报告

## 测试执行时间: $(date)

## 测试结果概览

$(find TestResults -name "*.trx" -exec echo "- {}" \;)

## 覆盖率信息

$(if [ -f "TestResults/CoverageReport/index.html" ]; then
    echo "覆盖率报告已生成: TestResults/CoverageReport/index.html"
else
    echo "覆盖率报告未生成"
fi)

## 测试统计

$(find TestResults -name "*.trx" -exec basename {} \; | sed 's/.trx//' | while read test; do
    echo "- $test"
done)

EOF
    
    log_success "测试汇总报告生成完成"
}

# 显示测试结果
show_test_results() {
    log_info "测试结果汇总:"
    
    echo ""
    echo "测试结果文件:"
    find TestResults -name "*.trx" -exec echo "- {}" \;
    
    echo ""
    echo "覆盖率报告:"
    if [ -f "TestResults/CoverageReport/index.html" ]; then
        echo "- TestResults/CoverageReport/index.html"
    else
        echo "- 未生成"
    fi
    
    echo ""
    echo "汇总报告:"
    echo "- TestResults/test_summary.md"
    
    echo ""
    echo "测试完成!"
}

# 主函数
main() {
    log_info "开始执行全面测试套件..."
    
    # 检查依赖
    check_dependencies
    
    # 清理和构建
    clean_and_build
    
    # 创建测试结果目录
    create_test_results_dir
    
    # 运行测试
    local failed_tests=0
    
    run_unit_tests || ((failed_tests++))
    run_integration_tests || ((failed_tests++))
    run_performance_tests || ((failed_tests++))
    run_large_xml_tests || ((failed_tests++))
    run_memory_tests || ((failed_tests++))
    run_error_handling_tests || ((failed_tests++))
    run_concurrency_tests || ((failed_tests++))
    run_regression_tests || ((failed_tests++))
    run_user_experience_tests || ((failed_tests++))
    
    # 生成报告
    generate_test_report
    
    # 显示结果
    show_test_results
    
    # 返回结果
    if [ $failed_tests -eq 0 ]; then
        log_success "所有测试通过!"
        exit 0
    else
        log_error "$failed_tests 个测试套件失败"
        exit 1
    fi
}

# 参数解析
case "${1:-}" in
    "unit")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_unit_tests
        ;;
    "integration")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_integration_tests
        ;;
    "performance")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_performance_tests
        ;;
    "large-xml")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_large_xml_tests
        ;;
    "memory")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_memory_tests
        ;;
    "error-handling")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_error_handling_tests
        ;;
    "concurrency")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_concurrency_tests
        ;;
    "regression")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_regression_tests
        ;;
    "user-experience")
        check_dependencies
        clean_and_build
        create_test_results_dir
        run_user_experience_tests
        ;;
    "report")
        generate_test_report
        show_test_results
        ;;
    "help"|"--help"|"-h")
        echo "BannerlordModEditor 测试执行脚本"
        echo ""
        echo "用法: $0 [选项]"
        echo ""
        echo "选项:"
        echo "  unit              运行单元测试"
        echo "  integration       运行集成测试"
        echo "  performance       运行性能测试"
        echo "  large-xml         运行大型XML测试"
        echo "  memory            运行内存测试"
        echo "  error-handling    运行错误处理测试"
        echo "  concurrency       运行并发测试"
        echo "  regression        运行回归测试"
        echo "  user-experience   运行用户体验测试"
        echo "  report            生成测试报告"
        echo "  help              显示帮助信息"
        echo ""
        echo "默认行为: 运行所有测试"
        ;;
    *)
        main
        ;;
esac