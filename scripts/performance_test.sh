#!/bin/bash

# Bannerlord Mod Editor CLI - 性能测试脚本
# 这个脚本用于测试CLI工具的性能表现

set -e

echo "================================================"
echo "Bannerlord Mod Editor CLI - 性能测试脚本"
echo "================================================"
echo

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

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

# 获取项目根目录
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CLI_PROJECT="$PROJECT_ROOT/BannerlordModEditor.Cli"
TEST_DATA_DIR="$PROJECT_ROOT/BannerlordModEditor.Common.Tests/TestData"

# 性能测试结果
PERFORMANCE_RESULTS=()

# 添加性能测试结果
add_performance_result() {
    local test_name="$1"
    local execution_time="$2"
    local memory_usage="$3"
    local file_size="$4"
    
    PERFORMANCE_RESULTS+=("$test_name|$execution_time|$memory_usage|$file_size")
}

# 测量执行时间
measure_execution_time() {
    local command="$1"
    local description="$2"
    
    print_info "开始测试: $description"
    
    local start_time=$(date +%s.%N)
    
    # 执行命令
    eval $command
    
    local end_time=$(date +%s.%N)
    local execution_time=$(echo "$end_time - $start_time" | bc -l)
    
    print_success "测试完成: $description (耗时: ${execution_time}s)"
    
    echo "$execution_time"
}

# 获取内存使用情况
get_memory_usage() {
    local pid=$1
    if [ -n "$pid" ]; then
        local memory=$(ps -p $pid -o rss= 2>/dev/null | tail -1)
        if [ "$memory" != "RSS" ]; then
            echo $((memory / 1024))  # 转换为MB
        else
            echo "0"
        fi
    else
        echo "0"
    fi
}

# 测试基本命令性能
test_basic_commands_performance() {
    print_info "测试基本命令性能..."
    
    # 测试list-models命令
    local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- list-models" "list-models命令")
    add_performance_result "list-models" "$time" "0" "0"
    
    # 测试help命令
    local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- --help" "help命令")
    add_performance_result "help" "$time" "0" "0"
    
    # 测试version命令
    local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- --version" "version命令")
    add_performance_result "version" "$time" "0" "0"
}

# 测试XML识别性能
test_xml_recognition_performance() {
    print_info "测试XML识别性能..."
    
    local xml_files=("action_types.xml" "combat_parameters.xml" "map_icons.xml" "flora_kinds.xml")
    
    for file in "${xml_files[@]}"; do
        if [ -f "$TEST_DATA_DIR/$file" ]; then
            local file_size=$(stat -c%s "$TEST_DATA_DIR/$file")
            local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- recognize -i \"$TEST_DATA_DIR/$file\"" "识别 $file")
            add_performance_result "recognize-$file" "$time" "0" "$file_size"
        else
            print_warning "测试文件不存在: $TEST_DATA_DIR/$file"
        fi
    done
}

# 测试XML到Excel转换性能
test_xml_to_excel_performance() {
    print_info "测试XML到Excel转换性能..."
    
    local xml_files=("action_types.xml" "combat_parameters.xml" "map_icons.xml")
    local temp_dir="/tmp/cli_performance_test_$(date +%s)"
    mkdir -p "$temp_dir"
    
    for file in "${xml_files[@]}"; do
        if [ -f "$TEST_DATA_DIR/$file" ]; then
            local file_size=$(stat -c%s "$TEST_DATA_DIR/$file")
            local output_file="$temp_dir/${file%.xml}.xlsx"
            
            local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- convert -i \"$TEST_DATA_DIR/$file\" -o \"$output_file\"" "转换 $file")
            
            if [ -f "$output_file" ]; then
                local output_size=$(stat -c%s "$output_file")
                add_performance_result "convert-$file" "$time" "0" "$file_size->$output_size"
            else
                add_performance_result "convert-$file" "$time" "0" "$file_size->FAILED"
            fi
        else
            print_warning "测试文件不存在: $TEST_DATA_DIR/$file"
        fi
    done
    
    # 清理临时文件
    rm -rf "$temp_dir"
}

# 测试大型文件处理性能
test_large_file_performance() {
    print_info "测试大型文件处理性能..."
    
    # 查找最大的XML文件
    local largest_file=$(find "$TEST_DATA_DIR" -name "*.xml" -exec stat -c"%s %n" {} \; | sort -nr | head -1 | cut -d' ' -f2-)
    
    if [ -n "$largest_file" ] && [ -f "$largest_file" ]; then
        local file_size=$(stat -c%s "$largest_file")
        local temp_dir="/tmp/cli_large_file_test_$(date +%s)"
        mkdir -p "$temp_dir"
        local output_file="$temp_dir/large_output.xlsx"
        
        print_info "测试大型文件: $largest_file ($(($file_size / 1024))KB)"
        
        local time=$(measure_execution_time "cd $PROJECT_ROOT && dotnet run --project $CLI_PROJECT -- convert -i \"$largest_file\" -o \"$output_file\"" "转换大型文件")
        
        if [ -f "$output_file" ]; then
            local output_size=$(stat -c%s "$output_file")
            add_performance_result "large-file-conversion" "$time" "0" "$file_size->$output_size"
        else
            add_performance_result "large-file-conversion" "$time" "0" "$file_size->FAILED"
        fi
        
        # 清理临时文件
        rm -rf "$temp_dir"
    else
        print_warning "未找到大型XML文件用于测试"
    fi
}

# 测试并发性能
test_concurrent_performance() {
    print_info "测试并发性能..."
    
    local temp_dir="/tmp/cli_concurrent_test_$(date +%s)"
    mkdir -p "$temp_dir"
    
    # 启动多个并发转换任务
    local xml_files=("action_types.xml" "combat_parameters.xml" "map_icons.xml")
    local pids=()
    
    for file in "${xml_files[@]}"; do
        if [ -f "$TEST_DATA_DIR/$file" ]; then
            local output_file="$temp_dir/concurrent_${file%.xml}.xlsx"
            
            # 在后台启动转换任务
            cd $PROJECT_ROOT
            dotnet run --project $CLI_PROJECT -- convert -i "$TEST_DATA_DIR/$file" -o "$output_file" &
            pids+=($!)
        fi
    done
    
    # 等待所有任务完成
    local start_time=$(date +%s.%N)
    for pid in "${pids[@]}"; do
        wait $pid
    done
    local end_time=$(date +%s.%N)
    local total_time=$(echo "$end_time - $start_time" | bc -l)
    
    add_performance_result "concurrent-3-tasks" "$total_time" "0" "0"
    
    # 清理临时文件
    rm -rf "$temp_dir"
}

# 生成性能测试报告
generate_performance_report() {
    print_info "生成性能测试报告..."
    
    local report_dir="$PROJECT_ROOT/test_reports"
    mkdir -p "$report_dir"
    
    local report_file="$report_dir/performance_test_report_$(date +%Y%m%d_%H%M%S).md"
    
    cat > "$report_file" << EOF
# Bannerlord Mod Editor CLI - 性能测试报告

**测试时间**: $(date)
**测试环境**: $(uname -a)

## 性能测试结果

| 测试名称 | 执行时间(秒) | 内存使用(MB) | 文件大小 |
|----------|-------------|-------------|----------|
EOF
    
    for result in "${PERFORMANCE_RESULTS[@]}"; do
        IFS='|' read -r test_name execution_time memory_usage file_size <<< "$result"
        echo "| $test_name | $execution_time | $memory_usage | $file_size |" >> "$report_file"
    done
    
    cat >> "$report_file" << EOF

## 性能分析

### 基本命令性能
- list-models命令应该快速响应
- help命令应该快速显示
- version命令应该快速显示

### XML处理性能
- XML识别应该在合理时间内完成
- XML到Excel转换应该在可接受时间内完成
- 大型文件处理应该有良好的性能

### 并发性能
- 并发处理多个文件应该有良好的性能表现

## 性能建议

1. **优化建议**:
   - 考虑使用并行处理提高大型文件处理速度
   - 优化内存使用以处理更大的文件
   - 改进错误处理以减少性能影响

2. **监控建议**:
   - 监控内存使用情况
   - 监控CPU使用率
   - 监控磁盘I/O性能

## 环境信息

- **操作系统**: $(uname -s)
- **处理器**: $(uname -m)
- **内存**: $(free -h | grep Mem | awk '{print $2}')
- **磁盘空间**: $(df -h . | tail -1 | awk '{print $4}')

---
*此报告由性能测试脚本自动生成*
EOF

    print_success "性能测试报告已生成: $report_file"
}

# 主函数
main() {
    echo "开始性能测试..."
    echo
    
    # 检查依赖项
    if ! command -v bc &> /dev/null; then
        print_warning "bc命令未安装，某些计算可能不准确"
    fi
    
    # 运行性能测试
    test_basic_commands_performance
    echo
    
    test_xml_recognition_performance
    echo
    
    test_xml_to_excel_performance
    echo
    
    test_large_file_performance
    echo
    
    test_concurrent_performance
    echo
    
    # 生成报告
    generate_performance_report
    
    echo
    print_success "性能测试完成！"
    
    # 显示摘要
    echo "=== 性能测试摘要 ==="
    echo "测试项目数量: ${#PERFORMANCE_RESULTS[@]}"
    echo
    
    echo "性能测试结果:"
    printf "%-30s %-15s %-15s %s\n" "测试名称" "执行时间(秒)" "内存使用(MB)" "文件大小"
    echo "----------------------------------------------------------------------------------------"
    
    for result in "${PERFORMANCE_RESULTS[@]}"; do
        IFS='|' read -r test_name execution_time memory_usage file_size <<< "$result"
        printf "%-30s %-15s %-15s %s\n" "$test_name" "$execution_time" "$memory_usage" "$file_size"
    done
}

# 运行主函数
main "$@"