#!/usr/bin/env pwsh

<#
.SYNOPSIS
    同步测试数据从Common.Tests到UI.Tests
.DESCRIPTION
    此脚本用于将Common.Tests/TestData/目录中的必要XML文件同步到UI.Tests/TestData/目录
    确保UI测试有正确的测试数据文件可用
.PARAMETER SourcePath
    源路径，默认为../BannerlordModEditor.Common.Tests/TestData
.PARAMETER DestinationPath
    目标路径，默认为./TestData
.PARAMETER Force
    强制覆盖已存在的文件
.EXAMPLE
    .\Sync-TestData.ps1
.EXAMPLE
    .\Sync-TestData.ps1 -Force
#>

param(
    [string]$SourcePath = "../BannerlordModEditor.Common.Tests/TestData",
    [string]$DestinationPath = "./TestData",
    [switch]$Force
)

# 设置错误处理
$ErrorActionPreference = "Stop"

# 定义需要的文件列表
$requiredFiles = @(
    "attributes.xml",
    "bone_body_types.xml", 
    "skills.xml",
    "module_sounds.xml",
    "crafting_pieces.xml",
    "item_modifiers.xml"
)

Write-Host "开始同步测试数据..." -ForegroundColor Green
Write-Host "源路径: $SourcePath" -ForegroundColor Yellow
Write-Host "目标路径: $DestinationPath" -ForegroundColor Yellow

# 检查源路径是否存在
if (-not (Test-Path $SourcePath)) {
    Write-Error "源路径不存在: $SourcePath"
    exit 1
}

# 创建目标目录（如果不存在）
if (-not (Test-Path $DestinationPath)) {
    Write-Host "创建目标目录: $DestinationPath" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $DestinationPath -Force | Out-Null
}

# 同步文件
$filesCopied = 0
$filesSkipped = 0
$filesNotFound = 0

foreach ($file in $requiredFiles) {
    $sourceFile = Join-Path $SourcePath $file
    $destFile = Join-Path $DestinationPath $file
    
    Write-Host "处理文件: $file" -ForegroundColor Cyan
    
    if (Test-Path $sourceFile) {
        if ((Test-Path $destFile) -and -not $Force) {
            Write-Host "  跳过: 文件已存在 (使用 -Force 参数强制覆盖)" -ForegroundColor Yellow
            $filesSkipped++
        } else {
            try {
                Copy-Item $sourceFile $destFile -Force
                Write-Host "  ✓ 已复制" -ForegroundColor Green
                $filesCopied++
            } catch {
                Write-Error "  复制失败: $_"
                exit 1
            }
        }
    } else {
        Write-Warning "  源文件不存在: $sourceFile"
        $filesNotFound++
    }
}

# 显示摘要
Write-Host ""
Write-Host "同步完成!" -ForegroundColor Green
Write-Host "已复制: $filesCopied 个文件" -ForegroundColor Green
Write-Host "已跳过: $filesSkipped 个文件" -ForegroundColor Yellow
if ($filesNotFound -gt 0) {
    Write-Host "未找到: $filesNotFound 个文件" -ForegroundColor Red
    Write-Warning "某些必需的测试文件不存在，请检查Common.Tests/TestData目录"
}

# 验证结果
Write-Host ""
Write-Host "验证目标目录..." -ForegroundColor Cyan
$existingFiles = Get-ChildItem -Path $DestinationPath -Name "*.xml"
Write-Host "目标目录中的XML文件: $($existingFiles -join ', ')" -ForegroundColor Cyan

# 检查是否有缺失的文件
$missingFiles = $requiredFiles | Where-Object { $_ -notin $existingFiles }
if ($missingFiles) {
    Write-Warning "缺失的文件: $($missingFiles -join ', ')"
    exit 1
}

Write-Host "所有必需的测试文件都已同步完成!" -ForegroundColor Green