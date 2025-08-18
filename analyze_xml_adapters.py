#!/usr/bin/env python3
"""
分析XML文件DO/DTO适配状态
"""

import os
import re
from pathlib import Path

def extract_test_names_from_current_tests():
    """从current_tests.txt提取测试文件名"""
    current_tests_path = "/root/WorkSpace/CSharp/BannerlordModEditor/current_tests.txt"
    with open(current_tests_path, 'r') as f:
        lines = f.readlines()
    
    test_names = []
    for line in lines:
        line = line.strip()
        if line and not line.startswith('#'):
            test_names.append(line)
    
    return test_names

def get_existing_test_files():
    """获取已存在的测试文件"""
    test_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common.Tests"
    test_files = []
    
    for file in os.listdir(test_dir):
        if file.endswith("XmlTests.cs"):
            # 提取文件名中的XML名称部分
            name = file.replace("XmlTests.cs", "")
            test_files.append(name)
    
    return sorted(test_files)

def get_data_models():
    """获取Data模型"""
    data_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Models/Data"
    data_models = []
    
    for file in os.listdir(data_dir):
        if file.endswith(".cs"):
            # 移除.cs扩展名，转换为标准名称
            name = file.replace(".cs", "")
            # 处理一些特殊情况
            name = name.replace("Model", "")
            name = name.replace("Data", "")
            name = name.replace("Xml", "")
            data_models.append(name)
    
    return sorted(data_models)

def get_do_models():
    """获取DO模型"""
    do_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Models/DO"
    do_models = []
    
    for root, dirs, files in os.walk(do_dir):
        for file in files:
            if file.endswith("DO.cs"):
                # 提取DO模型名称
                name = file.replace("DO.cs", "")
                do_models.append(name)
    
    return sorted(do_models)

def get_dto_models():
    """获取DTO模型"""
    dto_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Models/DTO"
    dto_models = []
    
    for root, dirs, files in os.walk(dto_dir):
        for file in files:
            if file.endswith("DTO.cs"):
                # 提取DTO模型名称
                name = file.replace("DTO.cs", "")
                dto_models.append(name)
    
    return sorted(dto_models)

def get_mappers():
    """获取Mapper文件"""
    mapper_dir = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common/Mappers"
    mappers = []
    
    for file in os.listdir(mapper_dir):
        if file.endswith("Mapper.cs"):
            # 提取Mapper名称
            name = file.replace("Mapper.cs", "")
            mappers.append(name)
    
    return sorted(mappers)

def normalize_name(name):
    """标准化名称以便比较"""
    # 移除常见的后缀
    name = name.replace("Model", "")
    name = name.replace("Data", "")
    name = name.replace("Xml", "")
    return name

def main():
    print("=== Bannerlord XML文件DO/DTO适配状态分析 ===\n")
    
    # 获取所有数据
    current_tests = extract_test_names_from_current_tests()
    existing_tests = get_existing_test_files()
    data_models = get_data_models()
    do_models = get_do_models()
    dto_models = get_dto_models()
    mappers = get_mappers()
    
    print(f"current_tests.txt中的文件总数: {len(current_tests)}")
    print(f"已存在的测试文件数: {len(existing_tests)}")
    print(f"Data模型数: {len(data_models)}")
    print(f"DO模型数: {len(do_models)}")
    print(f"DTO模型数: {len(dto_models)}")
    print(f"Mapper文件数: {len(mappers)}")
    print()
    
    # 1. 找出current_tests.txt中有但没有测试的文件
    missing_tests = set(current_tests) - set(existing_tests)
    print("=== 缺失测试的文件 ===")
    if missing_tests:
        for test in sorted(missing_tests):
            print(f"- {test}")
    else:
        print("所有current_tests.txt中的文件都有对应的测试")
    print()
    
    # 2. 找出有Data模型但没有DO/DTO适配的文件
    data_only = set(data_models) - set(do_models)
    print("=== 有Data模型但没有DO/DTO适配的文件 ===")
    if data_only:
        for model in sorted(data_only):
            print(f"- {model}")
    else:
        print("所有Data模型都有对应的DO模型")
    print()
    
    # 3. 找出有DO模型但没有DTO模型的文件
    do_only = set(do_models) - set(dto_models)
    print("=== 有DO模型但没有DTO模型的文件 ===")
    if do_only:
        for model in sorted(do_only):
            print(f"- {model}")
    else:
        print("所有DO模型都有对应的DTO模型")
    print()
    
    # 4. 找出有DO/DTO但没有Mapper的文件
    has_do_dto = set(do_models) & set(dto_models)
    no_mapper = has_do_dto - set(mappers)
    print("=== 有DO/DTO但没有Mapper的文件 ===")
    if no_mapper:
        for model in sorted(no_mapper):
            print(f"- {model}")
    else:
        print("所有有DO/DTO的文件都有对应的Mapper")
    print()
    
    # 5. 找出完整的DO/DTO适配文件
    complete_adapters = set(do_models) & set(dto_models) & set(mappers)
    print("=== 已完成DO/DTO适配的文件 ===")
    if complete_adapters:
        for model in sorted(complete_adapters):
            print(f"- {model}")
    else:
        print("没有完整的DO/DTO适配文件")
    print()
    
    # 6. 优先级建议
    print("=== 建议的适配优先级 ===")
    
    # 高优先级：有Data模型但没有DO/DTO的
    high_priority = sorted(set(data_models) - set(do_models))
    if high_priority:
        print("高优先级 (有Data模型但缺少DO/DTO):")
        for model in high_priority[:10]:  # 显示前10个
            print(f"  - {model}")
        if len(high_priority) > 10:
            print(f"  ... 还有 {len(high_priority) - 10} 个")
    
    # 中优先级：有DO但缺少DTO或Mapper的
    medium_priority = sorted((set(do_models) - set(dto_models)) | (set(do_models) & set(dto_models) - set(mappers)))
    if medium_priority:
        print("中优先级 (部分完成DO/DTO适配):")
        for model in medium_priority[:5]:  # 显示前5个
            print(f"  - {model}")
        if len(medium_priority) > 5:
            print(f"  ... 还有 {len(medium_priority) - 5} 个")
    
    # 低优先级：缺失测试的
    low_priority = sorted(missing_tests)
    if low_priority:
        print("低优先级 (缺失测试):")
        for model in low_priority[:5]:  # 显示前5个
            print(f"  - {model}")
        if len(low_priority) > 5:
            print(f"  ... 还有 {len(low_priority) - 5} 个")

if __name__ == "__main__":
    main()