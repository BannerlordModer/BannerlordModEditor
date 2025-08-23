#!/bin/bash

# XML适配状态检查工具运行脚本
# 此脚本会运行检查工具并在docs目录中生成报告

echo "=== XML适配状态检查工具 ==="
echo "正在运行检查..."

# 进入工具目录
cd "$(dirname "$0")"

# 创建输出目录
mkdir -p output

# 运行检查工具
echo "正在执行XML适配状态检查..."
dotnet run --project XmlAdaptationChecker.csproj -- check

# 如果成功，复制报告到docs目录
if [ $? -eq 0 ]; then
    echo "检查完成！"
    
    # 创建docs目录中的报告文件
    DOCS_DIR="../../../docs"
    if [ -d "$DOCS_DIR" ]; then
        echo "正在生成docs目录中的报告..."
        
        # 创建报告文件
        echo "# XML适配状态报告" > "$DOCS_DIR/xml-adaptation-status.md"
        echo "" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "## 检查时间" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "$(date)" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "" >> "$DOCS_DIR/xml-adaptation-status.md"
        
        # 运行summary命令并追加到报告
        echo "## 摘要信息" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "" >> "$DOCS_DIR/xml-adaptation-status.md"
        dotnet run --project XmlAdaptationChecker.csproj -- summary >> "$DOCS_DIR/xml-adaptation-status.md"
        
        echo "## 详细分析" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "请运行以下命令查看详细的未适配文件列表：" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "\`\`\`bash" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "cd scripts/XmlAdaptationChecker" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "dotnet run -- check" >> "$DOCS_DIR/xml-adaptation-status.md"
        echo "\`\`\`" >> "$DOCS_DIR/xml-adaptation-status.md"
        
        echo "报告已生成到: $DOCS_DIR/xml-adaptation-status.md"
    fi
else
    echo "检查失败！"
    exit 1
fi

echo "=== 完成 ==="