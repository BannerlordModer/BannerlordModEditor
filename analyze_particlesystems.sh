#!/bin/bash

echo "=== ParticleSystems 问题分析 ==="

# 统计原始XML中的节点数
echo "原始XML节点数:"
grep -o '<[^>]*>' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml | wc -l

# 统计原始XML中的属性数
echo "原始XML属性数:"
grep -o '[a-zA-Z_][a-zA-Z0-9_-]*="[^"]*"' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml | wc -l

# 统计特定元素
echo "特定元素统计:"
echo "curve元素数:"
grep -c '<curve' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml

echo "keys元素数:"
grep -c '<keys>' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml

echo "key元素数:"
grep -c '<key' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml

echo "decal_materials元素数:"
grep -c '<decal_materials>' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml

echo "decal_material元素数:"
grep -c '<decal_material' ../TestDebug/bin/Debug/net9.0/TestData/particle_systems_hardcoded_misc1.xml