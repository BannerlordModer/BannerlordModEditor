#!/usr/bin/env python3
"""
更详细的XML文件复杂度分析和优先级评估
"""

import os
import re
from pathlib import Path

def get_data_models_with_complexity():
    """获取Data模型并评估复杂度"""
    data_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Models/Data"
    models = []
    
    for file in os.listdir(data_dir):
        if file.endswith(".cs"):
            # 移除.cs扩展名
            name = file.replace(".cs", "")
            # 处理一些特殊情况
            clean_name = name.replace("Model", "").replace("Data", "").replace("Xml", "")
            
            # 读取文件内容评估复杂度
            file_path = os.path.join(data_dir, file)
            complexity = analyze_complexity(file_path)
            
            models.append({
                'name': clean_name,
                'original_name': name,
                'file_path': file_path,
                'complexity': complexity
            })
    
    return models

def analyze_complexity(file_path):
    """分析文件复杂度"""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        complexity_score = 0
        
        # 计算类数量
        class_count = len(re.findall(r'class\s+\w+', content))
        complexity_score += class_count * 2
        
        # 计算属性数量
        property_count = len(re.findall(r'public\s+\w+.*?\{.*?\}', content, re.DOTALL))
        complexity_score += property_count
        
        # 计算列表/集合属性
        list_count = len(re.findall(r'List<\w+>', content))
        complexity_score += list_count * 3
        
        # 计算嵌套类
        nested_count = len(re.findall(r'public\s+class\s+\w+.*?\{.*?\}', content, re.DOTALL))
        complexity_score += nested_count * 2
        
        # 计算XmlElement/XmlAttribute数量
        xml_element_count = len(re.findall(r'\[XmlElement\(', content))
        xml_attribute_count = len(re.findall(r'\[XmlAttribute\(', content))
        complexity_score += (xml_element_count + xml_attribute_count)
        
        # 计算ShouldSerialize方法数量
        should_serialize_count = len(re.findall(r'ShouldSerialize\w+', content))
        complexity_score += should_serialize_count
        
        # 计算私有字段数量（存在复杂逻辑的标志）
        private_field_count = len(re.findall(r'private\s+\w+.*?_.*?;', content))
        complexity_score += private_field_count * 2
        
        return {
            'score': complexity_score,
            'class_count': class_count,
            'property_count': property_count,
            'list_count': list_count,
            'nested_count': nested_count,
            'xml_element_count': xml_element_count,
            'xml_attribute_count': xml_attribute_count,
            'should_serialize_count': should_serialize_count,
            'private_field_count': private_field_count
        }
    except Exception as e:
        return {'score': 0, 'error': str(e)}

def get_existing_do_models():
    """获取已存在的DO模型"""
    do_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Models/DO"
    do_models = set()
    
    for root, dirs, files in os.walk(do_dir):
        for file in files:
            if file.endswith("DO.cs"):
                name = file.replace("DO.cs", "")
                do_models.add(name)
    
    return do_models

def main():
    print("=== Bannerlord XML文件复杂度分析和优先级评估 ===\n")
    
    # 获取所有数据
    data_models = get_data_models_with_complexity()
    existing_do_models = get_existing_do_models()
    
    # 筛选出还没有DO适配的模型
    missing_do_models = [m for m in data_models if m['name'] not in existing_do_models]
    
    # 按复杂度排序
    missing_do_models.sort(key=lambda x: x['complexity']['score'], reverse=True)
    
    print(f"总Data模型数: {len(data_models)}")
    print(f"已有DO适配的模型数: {len(existing_do_models)}")
    print(f"缺少DO适配的模型数: {len(missing_do_models)}")
    print()
    
    # 分级显示优先级
    print("=== 高优先级 (复杂度 > 20) ===")
    high_priority = [m for m in missing_do_models if m['complexity']['score'] > 20]
    for model in high_priority[:10]:
        c = model['complexity']
        print(f"- {model['name']} (复杂度: {c['score']})")
        print(f"  类数: {c['class_count']}, 属性数: {c['property_count']}, 列表数: {c['list_count']}")
        print(f"  XML元素: {c['xml_element_count']}, XML属性: {c['xml_attribute_count']}")
        print()
    
    print("=== 中优先级 (复杂度 10-20) ===")
    medium_priority = [m for m in missing_do_models if 10 <= m['complexity']['score'] <= 20]
    for model in medium_priority[:10]:
        c = model['complexity']
        print(f"- {model['name']} (复杂度: {c['score']})")
        print(f"  类数: {c['class_count']}, 属性数: {c['property_count']}, 列表数: {c['list_count']}")
        print()
    
    print("=== 低优先级 (复杂度 < 10) ===")
    low_priority = [m for m in missing_do_models if m['complexity']['score'] < 10]
    for model in low_priority[:10]:
        c = model['complexity']
        print(f"- {model['name']} (复杂度: {c['score']})")
        print(f"  类数: {c['class_count']}, 属性数: {c['property_count']}")
        print()
    
    # 推荐下一步处理的文件
    print("=== 推荐下一步处理的文件 (前5个高优先级) ===")
    for i, model in enumerate(high_priority[:5]):
        print(f"{i+1}. {model['name']}")
        print(f"   文件: {model['file_path']}")
        print(f"   复杂度: {model['complexity']['score']}")
        print(f"   建议处理原因: 高复杂度，包含多个类和嵌套结构")
        print()

if __name__ == "__main__":
    main()