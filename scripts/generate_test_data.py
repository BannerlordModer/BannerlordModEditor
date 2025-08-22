#!/usr/bin/env python3
"""
Bannerlord Mod Editor CLI - 测试数据生成器

这个脚本用于生成各种测试数据，包括：
- 大型XML文件
- 包含特殊字符的XML文件
- 空XML文件
- 损坏的XML文件
- Excel文件（使用pandas）
"""

import os
import sys
import random
import string
import xml.etree.ElementTree as ET
from datetime import datetime
import pandas as pd

def ensure_directory_exists(directory):
    """确保目录存在"""
    if not os.path.exists(directory):
        os.makedirs(directory)

def generate_large_xml_file(file_path, item_count=1000):
    """生成大型XML文件"""
    print(f"生成大型XML文件: {file_path} ({item_count} 个条目)")
    
    root = ET.Element("action_types")
    
    for i in range(item_count):
        action = ET.SubElement(root, "action")
        action.set("name", f"action_{i:04d}")
        action.set("type", random.choice(["swing", "thrust", "block", "kick", "dodge"]))
        action.set("usage_direction", random.choice(["one_handed", "two_handed", "both", "none"]))
        action.set("action_stage", random.choice(["attack", "defense", "special", "passive"]))
    
    tree = ET.ElementTree(root)
    tree.write(file_path, encoding="utf-8", xml_declaration=True)
    print(f"✓ 大型XML文件已生成: {file_path}")

def generate_xml_with_special_characters(file_path):
    """生成包含特殊字符的XML文件"""
    print(f"生成包含特殊字符的XML文件: {file_path}")
    
    root = ET.Element("action_types")
    
    special_chars = ["&", "<", ">", "\"", "'", "©", "®", "™", "…", "–", "—"]
    
    for i in range(10):
        action = ET.SubElement(root, "action")
        action.set("name", f"special_action_{i}_{random.choice(special_chars)}")
        action.set("type", f"special_{random.choice(special_chars)}")
        action.set("usage_direction", f"direction_{random.choice(special_chars)}")
        action.set("action_stage", f"stage_{random.choice(special_chars)}")
    
    tree = ET.ElementTree(root)
    tree.write(file_path, encoding="utf-8", xml_declaration=True)
    print(f"✓ 特殊字符XML文件已生成: {file_path}")

def generate_empty_xml_file(file_path):
    """生成空的XML文件"""
    print(f"生成空XML文件: {file_path}")
    
    root = ET.Element("root")
    tree = ET.ElementTree(root)
    tree.write(file_path, encoding="utf-8", xml_declaration=True)
    print(f"✓ 空XML文件已生成: {file_path}")

def generate_corrupted_xml_file(file_path):
    """生成损坏的XML文件"""
    print(f"生成损坏的XML文件: {file_path}")
    
    corrupted_content = """<?xml version="1.0" encoding="utf-8"?>
<action_types>
    <action name="test_action" type="swing" usage_direction="one_handed"
    <!-- 缺少结束标签 -->
    <action name="test_action_2" type="thrust" usage_direction="two_handed" action_stage="attack"
    <invalid_xml_content>
    </action_types>
"""
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(corrupted_content)
    print(f"✓ 损坏的XML文件已生成: {file_path}")

def generate_excel_from_xml(xml_file_path, excel_file_path):
    """从XML文件生成Excel文件"""
    print(f"从XML生成Excel文件: {xml_file_path} -> {excel_file_path}")
    
    try:
        # 解析XML文件
        tree = ET.parse(xml_file_path)
        root = tree.getroot()
        
        # 提取数据
        data = []
        for action in root.findall(".//action"):
            row = {
                'name': action.get('name', ''),
                'type': action.get('type', ''),
                'usage_direction': action.get('usage_direction', ''),
                'action_stage': action.get('action_stage', '')
            }
            data.append(row)
        
        # 创建DataFrame
        df = pd.DataFrame(data)
        
        # 保存为Excel
        df.to_excel(excel_file_path, index=False)
        print(f"✓ Excel文件已生成: {excel_file_path}")
        
    except Exception as e:
        print(f"✗ 生成Excel文件失败: {e}")

def generate_test_data_directory(output_dir):
    """生成完整的测试数据目录"""
    print(f"生成测试数据目录: {output_dir}")
    
    ensure_directory_exists(output_dir)
    
    # 生成各种测试文件
    test_files = {
        "large_action_types.xml": lambda: generate_large_xml_file(os.path.join(output_dir, "large_action_types.xml"), 5000),
        "special_chars.xml": lambda: generate_xml_with_special_characters(os.path.join(output_dir, "special_chars.xml")),
        "empty.xml": lambda: generate_empty_xml_file(os.path.join(output_dir, "empty.xml")),
        "corrupted.xml": lambda: generate_corrupted_xml_file(os.path.join(output_dir, "corrupted.xml")),
        "nested.xml": lambda: generate_nested_xml_file(os.path.join(output_dir, "nested.xml")),
        "unicode.xml": lambda: generate_unicode_xml_file(os.path.join(output_dir, "unicode.xml")),
    }
    
    for file_name, generator in test_files.items():
        try:
            generator()
        except Exception as e:
            print(f"✗ 生成文件 {file_name} 失败: {e}")
    
    # 生成对应的Excel文件
    excel_files = {
        "large_action_types.xlsx": lambda: generate_excel_from_xml(os.path.join(output_dir, "large_action_types.xml"), os.path.join(output_dir, "large_action_types.xlsx")),
        "special_chars.xlsx": lambda: generate_excel_from_xml(os.path.join(output_dir, "special_chars.xml"), os.path.join(output_dir, "special_chars.xlsx")),
    }
    
    for file_name, generator in excel_files.items():
        try:
            generator()
        except Exception as e:
            print(f"✗ 生成Excel文件 {file_name} 失败: {e}")
    
    # 生成README文件
    readme_content = f"""# Bannerlord Mod Editor CLI - 测试数据

此目录包含用于测试CLI工具的各种测试数据文件。

## 文件说明

### XML文件
- **large_action_types.xml**: 包含5000个动作类型的大型XML文件
- **special_chars.xml**: 包含特殊字符的XML文件
- **empty.xml**: 空的XML文件
- **corrupted.xml**: 格式损坏的XML文件
- **nested.xml**: 包含嵌套结构的XML文件
- **unicode.xml**: 包含Unicode字符的XML文件

### Excel文件
- **large_action_types.xlsx**: 从large_action_types.xml生成的Excel文件
- **special_chars.xlsx**: 从special_chars.xml生成的Excel文件

## 生成时间
{datetime.now().strftime('%Y-%m-%d %H:%M:%S')}

## 使用方法
这些文件可以用于测试CLI工具的各种功能，包括：
- 格式识别
- 文件转换
- 错误处理
- 性能测试
"""
    
    with open(os.path.join(output_dir, "README.md"), 'w', encoding='utf-8') as f:
        f.write(readme_content)
    
    print(f"✓ 测试数据目录已生成: {output_dir}")

def generate_nested_xml_file(file_path):
    """生成包含嵌套结构的XML文件"""
    print(f"生成嵌套结构XML文件: {file_path}")
    
    root = ET.Element("combat_parameters")
    root.set("type", "base")
    
    # 添加definitions
    definitions = ET.SubElement(root, "definitions")
    for i in range(5):
        definition = ET.SubElement(definitions, "def")
        definition.set("name", f"def_{i}")
        definition.set("value", f"value_{i}")
    
    # 添加嵌套的combat_parameters
    for i in range(3):
        nested_params = ET.SubElement(root, "combat_parameters")
        nested_params.set("id", f"nested_{i}")
        
        for j in range(3):
            param = ET.SubElement(nested_params, "combat_parameter")
            param.set("name", f"param_{i}_{j}")
            param.set("value", f"val_{i}_{j}")
    
    tree = ET.ElementTree(root)
    tree.write(file_path, encoding="utf-8", xml_declaration=True)
    print(f"✓ 嵌套结构XML文件已生成: {file_path}")

def generate_unicode_xml_file(file_path):
    """生成包含Unicode字符的XML文件"""
    print(f"生成Unicode字符XML文件: {file_path}")
    
    root = ET.Element("action_types")
    
    # 各种Unicode字符
    unicode_names = [
        "测试动作", "動作測試", "アクション", "액션", "Действие",
        "हिंदी", "العربية", "日本語", "한국어", "русский"
    ]
    
    for i, name in enumerate(unicode_names):
        action = ET.SubElement(root, "action")
        action.set("name", name)
        action.set("type", f"type_{i}")
        action.set("usage_direction", f"direction_{i}")
        action.set("action_stage", f"stage_{i}")
    
    tree = ET.ElementTree(root)
    tree.write(file_path, encoding="utf-8", xml_declaration=True)
    print(f"✓ Unicode字符XML文件已生成: {file_path}")

def main():
    """主函数"""
    if len(sys.argv) < 2:
        print("用法: python generate_test_data.py <输出目录>")
        sys.exit(1)
    
    output_dir = sys.argv[1]
    
    print("===============================================")
    print("Bannerlord Mod Editor CLI - 测试数据生成器")
    print("===============================================")
    print()
    
    try:
        generate_test_data_directory(output_dir)
        print()
        print("✓ 所有测试数据已成功生成！")
    except Exception as e:
        print(f"✗ 生成测试数据失败: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()