#!/bin/bash

# 批量修复ShouldSerialize方法的脚本
# 将 !string.IsNullOrEmpty(Text) 改为 Text != null

# 查找所有需要修复的文件
FILES=$(find /root/WorkSpace/CSharp/BannerlordModEditor -name "*.cs" -exec grep -l "ShouldSerialize.*() => !string\.IsNullOrEmpty(" {} \;)

echo "找到 $(echo "$FILES" | wc -l) 个需要修复的文件"

# 逐个修复文件
for file in $FILES; do
    echo "正在修复文件: $file"
    
    # 使用sed进行批量替换
    # 将 !string.IsNullOrEmpty(Text) 替换为 Text != null
    sed -i 's/!string\.IsNullOrEmpty(\([^)]*\))/\1 != null/g' "$file"
    
    echo "已修复: $file"
done

echo "批量修复完成！"