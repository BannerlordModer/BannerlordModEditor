#!/bin/bash

# 脚本用于修复XML序列化中缺少命名空间的问题
# 查找所有需要修复的文件并应用修复

echo "开始修复XML序列化中的命名空间问题..."

# 创建临时文件来存储需要处理的文件列表
grep -r "serializer\.Serialize([^,]+,[^,]+);" BannerlordModEditor.Common.Tests/ --include="*.cs" > files_to_fix.txt

echo "找到需要修复的文件:"
cat files_to_fix.txt

# 逐个处理每个文件
while IFS= read -r line; do
    if [ -n "$line" ]; then
        # 提取文件路径
        file_path=$(echo "$line" | cut -d: -f1)
        echo "处理文件: $file_path"
        
        # 应用修复
        sed -i 's/\(serializer\.Serialize([^,]+,[^,]+)\);/\1\n                var ns = new XmlSerializerNamespaces();\n                ns.Add("", "");\n                serializer.Serialize(writer, model, ns);/g' "$file_path"
    fi
done < files_to_fix.txt

echo "修复完成\!"
rm -f files_to_fix.txt
