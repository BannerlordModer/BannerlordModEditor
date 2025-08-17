#!/bin/bash

# 监控docs文件夹中的新报告
echo "开始监控专家报告生成..."
echo "按 Ctrl+C 退出监控"
echo ""

# 记录已存在的文件
existing_files=$(ls -la docs/*.md | grep -v "XML_Adaptation_Progress_Tracking.md")

while true; do
    sleep 10
    
    # 检查新文件
    current_files=$(ls -la docs/*.md 2>/dev/null | grep -v "XML_Adaptation_Progress_Tracking.md")
    
    if [ "$current_files" != "$existing_files" ]; then
        echo ""
        echo "=== 发现新报告！$(date) ==="
        echo "新增的文件："
        comm -13 <(echo "$existing_files") <(echo "$current_files") || true
        echo ""
        existing_files="$current_files"
    fi
    
    # 显示简单的进度
    echo -n "."
done