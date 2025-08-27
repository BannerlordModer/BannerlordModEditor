# 技术栈和配置方案

## 概述

本文档详细说明了修复GitHub Actions UI测试失败问题所使用的技术栈选择和配置方案。基于对现有项目结构的分析，我们选择了一套成熟、可靠且易于维护的技术组合。

## 技术栈选择

### 核心技术栈

| 技术类别 | 选择 | 版本 | 理由 |
|---------|------|------|------|
| **运行时** | .NET 9.0 | 9.0.100 | 项目现有技术栈，提供最新特性和性能优化 |
| **构建工具** | MSBuild | 17.0+ | .NET原生构建系统，与项目完美集成 |
| **测试框架** | xUnit | 2.5.3 | 项目现有测试框架，稳定可靠 |
| **脚本语言** | PowerShell | 7.4+ | 跨平台支持，强大的文件系统操作能力 |
| **CI/CD平台** | GitHub Actions | 最新 | 项目现有CI/CD平台，无需额外配置 |

### 文件操作技术栈

| 技术类别 | 选择 | 版本 | 理由 |
|---------|------|------|------|
| **文件同步** | PowerShell + Robocopy | 7.4+ | 跨平台文件同步，支持符号链接 |
| **文件监控** | System.IO.FileSystemWatcher | .NET 9.0 | 原生文件系统监控 |
| **路径处理** | System.IO.Path | .NET 9.0 | 跨平台路径处理 |
| **压缩备份** | System.IO.Compression | .NET 9.0 | 原生压缩支持 |

### 配置管理技术栈

| 技术类别 | 选择 | 版本 | 理由 |
|---------|------|------|------|
| **配置格式** | JSON | 标准 | 人类可读，易于版本控制 |
| **配置解析** | System.Text.Json | .NET 9.0 | 高性能JSON处理 |
| **配置验证** | Data Annotations | .NET 9.0 | 内置验证特性 |
| **环境变量** | .NET Configuration | .NET 9.0 | 多环境配置支持 |

### 监控和诊断技术栈

| 技术类别 | 选择 | 版本 | 理由 |
|---------|------|------|------|
| **日志记录** | Serilog | 3.1.1 | 结构化日志，多种输出格式 |
| **性能监控** | System.Diagnostics | .NET 9.0 | 原生性能计数器 |
| **错误处理** | Polly | 8.4.1 | 弹性模式库，重试和断路器 |
| **健康检查** | Microsoft.Extensions.Diagnostics.HealthChecks | 9.0.0 | 标准化健康检查框架 |

## 项目配置

### 项目文件配置

#### 1. BannerlordModEditor.Common.Tests.csproj 优化
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    
    <!-- 测试数据配置 -->
    <TestDataPath>$(MSBuildProjectDirectory)/../TestData</TestDataPath>
    <EnableTestDataSync>true</EnableTestDataSync>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    
    <!-- 新增的测试支持包 -->
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="Polly" Version="8.4.1" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
    <Using Include="BannerlordModEditor.Common.Services" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
  </ItemGroup>

  <!-- 测试数据文件配置 -->
  <ItemGroup>
    <Content Include="..\TestData\**\*.*">
      <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      <Visible>false</Visible>
    </Content>
  </ItemGroup>

  <ItemGroup>
    <None Update="xunit.runner.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  <!-- 构建后事件：同步测试数据 -->
  <Target Name="SyncTestData" AfterTargets="Build">
    <Exec Command="powershell -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)/../scripts/test-data-sync.ps1&quot; -ProjectPath &quot;$(MSBuildProjectDirectory)&quot; -Configuration $(Configuration)" />
  </Target>

</Project>
```

#### 2. BannerlordModEditor.UI.Tests.csproj 优化
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    
    <!-- 测试数据配置 -->
    <TestDataPath>$(MSBuildProjectDirectory)/../TestData</TestDataPath>
    <EnableTestDataSync>true</EnableTestDataSync>
    
    <!-- UI测试特定配置 -->
    <UITestTimeout>120</UITestTimeout>
    <EnableHeadlessMode>true</EnableHeadlessMode>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia.Headless.XUnit" Version="11.3.0" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.3.0" />
    <PackageReference Include="Avalonia.FreeDesktop" Version="11.3.0" Condition="'$([MSBuild]::IsOSPlatform(Linux))' == 'true'" />
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
    <PackageReference Include="Moq" Version="4.20.70" />
    <PackageReference Include="System.Reactive" Version="6.0.1" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    
    <!-- 新增的测试支持包 -->
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="Polly" Version="8.4.1" />
    <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\BannerlordModEditor.UI\BannerlordModEditor.UI.csproj" />
    <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
  </ItemGroup>

  <!-- 测试数据文件配置 - 与Common.Tests共享 -->
  <ItemGroup>
    <Content Include="..\TestData\**\*.*">
      <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      <Visible>false</Visible>
    </Content>
  </ItemGroup>

  <!-- 构建后事件：同步测试数据 -->
  <Target Name="SyncTestData" AfterTargets="Build">
    <Exec Command="powershell -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)/../scripts/test-data-sync.ps1&quot; -ProjectPath &quot;$(MSBuildProjectDirectory)&quot; -Configuration $(Configuration) -Force" />
  </Target>

</Project>
```

### 测试数据同步脚本

#### 1. test-data-sync.ps1 (主要同步脚本)
```powershell
#!/usr/bin/env pwsh

<#
.SYNOPSIS
    测试数据同步脚本
.DESCRIPTION
    同步测试数据到各个测试项目，确保数据一致性
.PARAMETER SourcePath
    源测试数据路径
.PARAMETER ProjectPath
    目标项目路径
.PARAMETER Configuration
    构建配置（Debug/Release）
.PARAMETER Force
    是否强制同步
.PARAMETER Verify
    是否验证同步结果
.PARAMETER UseSymbolicLinks
    是否使用符号链接
.EXAMPLE
    .\test-data-sync.ps1 -SourcePath "TestData" -ProjectPath "BannerlordModEditor.UI.Tests" -Configuration "Release"
#>

param(
    [string]$SourcePath = "$(Split-Path $PSScriptRoot -Parent)/TestData",
    [string]$ProjectPath = "",
    [string]$Configuration = "Release",
    [switch]$Force,
    [switch]$Verify,
    [switch]$UseSymbolicLinks = $true
)

# 导入必要的模块
Import-Module "$PSScriptRoot/modules/TestDataSync.psm1" -Force

# 初始化日志
Initialize-TestDataSync -ScriptName "test-data-sync" -Configuration $Configuration

try {
    Write-Log "开始测试数据同步..." -Level "INFO"
    
    # 验证源路径
    if (-not (Test-Path $SourcePath)) {
        throw "源测试数据路径不存在: $SourcePath"
    }
    
    Write-Log "源测试数据路径: $SourcePath" -Level "INFO"
    
    # 如果没有指定项目路径，同步到所有测试项目
    if ([string]::IsNullOrEmpty($ProjectPath)) {
        $testProjects = @(
            "$(Split-Path $PSScriptRoot -Parent)/BannerlordModEditor.Common.Tests",
            "$(Split-Path $PSScriptRoot -Parent)/BannerlordModEditor.UI.Tests"
        )
        
        Write-Log "同步到所有测试项目: $($testProjects -join ', ')" -Level "INFO"
        
        foreach ($project in $testProjects) {
            Sync-TestDataToProject -SourcePath $SourcePath -ProjectPath $project -Configuration $Configuration -Force:$Force -Verify:$Verify -UseSymbolicLinks:$UseSymbolicLinks
        }
    }
    else {
        Write-Log "同步到指定项目: $ProjectPath" -Level "INFO"
        Sync-TestDataToProject -SourcePath $SourcePath -ProjectPath $ProjectPath -Configuration $Configuration -Force:$Force -Verify:$Verify -UseSymbolicLinks:$UseSymbolicLinks
    }
    
    Write-Log "测试数据同步完成" -Level "INFO"
    
    # 生成同步报告
    $report = Generate-SyncReport
    Save-SyncReport -Report $report -OutputPath "$(Split-Path $PSScriptRoot -Parent)/reports/test-data-sync-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
    
    exit 0
}
catch {
    Write-Log "测试数据同步失败: $($_.Exception.Message)" -Level "ERROR"
    Write-Log "堆栈跟踪: $($_.ScriptStackTrace)" -Level "ERROR"
    
    # 发送错误通知
    Send-ErrorNotification -Error $_.Exception -Context @{
        SourcePath = $SourcePath
        ProjectPath = $ProjectPath
        Configuration = $Configuration
    }
    
    exit 1
}
finally {
    # 清理资源
    Cleanup-TestDataSync
}
```

#### 2. TestDataManager.ps1 (测试数据管理脚本)
```powershell
#!/usr/bin/env pwsh

<#
.SYNOPSIS
    测试数据管理脚本
.DESCRIPTION
    提供测试数据的验证、清理、备份等管理功能
.PARAMETER Action
    执行的操作 (Validate/Cleanup/Backup/Restore/Report)
.PARAMETER TestDataPath
    测试数据路径
.PARAMETER BackupPath
    备份路径
.PARAMETER OutputPath
    输出路径
.EXAMPLE
    .\test-data-manager.ps1 -Action Validate -TestDataPath "TestData"
#>

param(
    [ValidateSet("Validate", "Cleanup", "Backup", "Restore", "Report", "Diagnose")]
    [string]$Action = "Validate",
    [string]$TestDataPath = "$(Split-Path $PSScriptRoot -Parent)/TestData",
    [string]$BackupPath = "",
    [string]$OutputPath = ""
)

# 导入必要的模块
Import-Module "$PSScriptRoot/modules/TestDataManager.psm1" -Force

# 初始化日志
Initialize-TestDataManager -ScriptName "test-data-manager" -Action $Action

try {
    Write-Log "开始测试数据管理操作: $Action" -Level "INFO"
    
    switch ($Action) {
        "Validate" {
            $result = Validate-TestData -TestDataPath $TestDataPath
            if ($result.IsValid) {
                Write-Log "测试数据验证成功" -Level "INFO"
            }
            else {
                Write-Log "测试数据验证失败" -Level "ERROR"
                foreach ($error in $result.Errors) {
                    Write-Log "错误: $error" -Level "ERROR"
                }
                exit 1
            }
        }
        
        "Cleanup" {
            $result = Cleanup-TestData -TestDataPath $TestDataPath
            Write-Log "清理完成，删除了 $($result.DeletedFiles.Count) 个文件" -Level "INFO"
        }
        
        "Backup" {
            if ([string]::IsNullOrEmpty($BackupPath)) {
                $BackupPath = "$(Split-Path $PSScriptRoot -Parent)/backups/test-data-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
            }
            $result = Backup-TestData -TestDataPath $TestDataPath -BackupPath $BackupPath
            Write-Log "备份完成: $($result.BackupPath)" -Level "INFO"
        }
        
        "Restore" {
            if ([string]::IsNullOrEmpty($BackupPath)) {
                throw "必须指定备份路径"
            }
            $result = Restore-TestData -BackupPath $BackupPath -TestDataPath $TestDataPath
            Write-Log "恢复完成" -Level "INFO"
        }
        
        "Report" {
            $report = Generate-TestDataReport -TestDataPath $TestDataPath
            if ([string]::IsNullOrEmpty($OutputPath)) {
                $OutputPath = "$(Split-Path $PSScriptRoot -Parent)/reports/test-data-report-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
            }
            Save-TestDataReport -Report $report -OutputPath $OutputPath
            Write-Log "报告已生成: $OutputPath" -Level "INFO"
        }
        
        "Diagnose" {
            $diagnosis = Diagnose-TestData -TestDataPath $TestDataPath
            if ([string]::IsNullOrEmpty($OutputPath)) {
                $OutputPath = "$(Split-Path $PSScriptRoot -Parent)/reports/test-data-diagnosis-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
            }
            Save-DiagnosisReport -Diagnosis $diagnosis -OutputPath $OutputPath
            Write-Log "诊断报告已生成: $OutputPath" -Level "INFO"
        }
    }
    
    exit 0
}
catch {
    Write-Log "测试数据管理操作失败: $($_.Exception.Message)" -Level "ERROR"
    Write-Log "堆栈跟踪: $($_.ScriptStackTrace)" -Level "ERROR"
    exit 1
}
finally {
    # 清理资源
    Cleanup-TestDataManager
}
```

### 配置文件

#### 1. test-data-config.json (测试数据配置)
```json
{
  "version": "1.0.0",
  "testDataPath": "TestData",
  "syncSettings": {
    "enabled": true,
    "useSymbolicLinks": true,
    "syncPatterns": [
      "*.xml",
      "*.json",
      "*.txt",
      "*.md"
    ],
    "excludePatterns": [
      "*.tmp",
      "*.log",
      "*.bak"
    ],
    "maxRetryAttempts": 3,
    "timeoutSeconds": 60,
    "verifyAfterSync": true
  },
  "validationSettings": {
    "strictValidation": true,
    "requiredFiles": [
      "action_types.xml",
      "combat_parameters.xml",
      "attributes.xml",
      "banner_icons.xml"
    ],
    "maxFileSize": 10485760,
    "validateXmlSchema": true,
    "validateFileIntegrity": true
  },
  "backupSettings": {
    "enabled": true,
    "backupPath": "backups",
    "maxBackups": 10,
    "compressBackups": true
  },
  "monitoringSettings": {
    "enabled": true,
    "checkInterval": 300,
    "alertOnFailure": true,
    "logLevel": "INFO"
  },
  "projectSettings": {
    "commonTests": {
      "path": "BannerlordModEditor.Common.Tests",
      "testDataSubPath": "TestData",
      "additionalFiles": []
    },
    "uiTests": {
      "path": "BannerlordModEditor.UI.Tests",
      "testDataSubPath": "TestData",
      "additionalFiles": [
        "ui-test-data.xml",
        "test-configurations.json"
      ]
    }
  }
}
```

#### 2. test-data-mapping.json (数据映射配置)
```json
{
  "fileMappings": {
    "action_types.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "schema": "action-types.xsd",
        "minSize": 1024,
        "maxSize": 1048576
      }
    },
    "combat_parameters.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "schema": "combat-parameters.xsd",
        "minSize": 2048,
        "maxSize": 2097152
      }
    },
    "attributes.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "schema": "attributes.xsd",
        "minSize": 512,
        "maxSize": 524288
      }
    },
    "ui-test-data.xml": {
      "targetProjects": ["ui"],
      "validation": {
        "required": false,
        "schema": "ui-test-data.xsd",
        "minSize": 256,
        "maxSize": 1048576
      }
    }
  },
  "directoryMappings": {
    "XmlFiles/": {
      "targetProjects": ["common", "ui"],
      "syncMode": "mirror"
    },
    "Configs/": {
      "targetProjects": ["common", "ui"],
      "syncMode": "one-way"
    },
    "Generated/": {
      "targetProjects": ["ui"],
      "syncMode": "one-way",
      "regenerate": true
    }
  }
}
```

### CI/CD 配置优化

#### 1. 优化的 GitHub Actions 工作流
```yaml
name: Enhanced UI Test Suite

on:
  push:
    branches: [ "**" ]
  pull_request:
    branches: [ master ]

env:
  DOTNET_VERSION: '9.0.x'
  TEST_DATA_PATH: 'TestData'
  SYNC_ENABLED: true
  VALIDATION_ENABLED: true

jobs:
  # 1. 测试数据准备和验证
  test-data-setup:
    runs-on: ubuntu-latest
    outputs:
      test-data-hash: ${{ steps.hash.outputs.hash }}
      validation-result: ${{ steps.validate.outputs.result }}
    
    steps:
    - name: 检出代码
      uses: actions/checkout@v4
      
    - name: 设置 .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
        
    - name: 恢复依赖
      run: dotnet restore
      
    - name: 同步测试数据
      run: scripts/test-data-sync.ps1 -Verify -Force
      
    - name: 验证测试数据
      id: validate
      run: scripts/test-data-manager.ps1 -Action Validate
      continue-on-error: true
      
    - name: 生成测试数据哈希
      id: hash
      run: |
        hash=$(find TestData -type f -exec md5sum {} \; | sort | md5sum | cut -d' ' -f1)
        echo "hash=$hash" >> $GITHUB_OUTPUT
        
    - name: 上传测试数据
      uses: actions/upload-artifact@v4
      with:
        name: test-data-${{ steps.hash.outputs.hash }}
        path: TestData/
        retention-days: 7

  # 2. Common.Tests 执行
  common-tests:
    runs-on: ${{ matrix.os }}
    needs: test-data-setup
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest]
        configuration: [Debug, Release]
    
    steps:
    - name: 检出代码
      uses: actions/checkout@v4
      
    - name: 设置 .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
        
    - name: 下载测试数据
      uses: actions/download-artifact@v4
      with:
        name: test-data-${{ needs.test-data-setup.outputs.test-data-hash }}
        path: TestData/
        
    - name: 构建项目
      run: dotnet build --configuration ${{ matrix.configuration }} --no-restore
      
    - name: 运行 Common.Tests
      run: |
        dotnet test BannerlordModEditor.Common.Tests \
          --configuration ${{ matrix.configuration }} \
          --no-build \
          --verbosity normal \
          --collect:"XPlat Code Coverage" \
          --results-directory TestResults \
          --logger "trx;LogFileName=common_tests_${{ matrix.os }}_${{ matrix.configuration }}.trx"
          
    - name: 上传测试结果
      uses: actions/upload-artifact@v4
      if: always()
      with:
        name: common-test-results-${{ matrix.os }}-${{ matrix.configuration }}
        path: |
          TestResults/
          *.trx

  # 3. UI.Tests 执行
  ui-tests:
    runs-on: ${{ matrix.os }}
    needs: test-data-setup
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest]
        configuration: [Debug, Release]
    
    steps:
    - name: 检出代码
      uses: actions/checkout@v4
      
    - name: 设置 .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
        
    - name: 下载测试数据
      uses: actions/download-artifact@v4
      with:
        name: test-data-${{ needs.test-data-setup.outputs.test-data-hash }}
        path: TestData/
        
    - name: 验证测试数据可用性
      run: |
        if [ ! -d "TestData" ]; then
          echo "错误: TestData 目录不存在"
          exit 1
        fi
        echo "TestData 目录内容:"
        ls -la TestData/
        
    - name: 构建项目
      run: dotnet build --configuration ${{ matrix.configuration }} --no-restore
      
    - name: 运行 UI.Tests
      env:
        DOTNET_CLI_TEST_TIMEOUT: 120
        TEST_DATA_PATH: ${{ env.TEST_DATA_PATH }}
      run: |
        # 诊断测试数据状态
        scripts/test-data-manager.ps1 -Action Diagnose
        
        # 运行UI测试
        dotnet test BannerlordModEditor.UI.Tests \
          --configuration ${{ matrix.configuration }} \
          --no-build \
          --verbosity normal \
          --filter "Category!=Integration" \
          --collect:"XPlat Code Coverage" \
          --results-directory TestResults \
          --logger "trx;LogFileName=ui_tests_${{ matrix.os }}_${{ matrix.configuration }}.trx" || {
            echo "UI测试失败，收集诊断信息..."
            find TestData -name "*.xml" | head -5
            echo "TestData 目录大小:"
            du -sh TestData/
            exit 1
          }
          
    - name: 上传测试结果
      uses: actions/upload-artifact@v4
      if: always()
      with:
        name: ui-test-results-${{ matrix.os }}-${{ matrix.configuration }}
        path: |
          TestResults/
          *.trx
          TestData/

  # 4. 测试结果汇总
  test-summary:
    runs-on: ubuntu-latest
    needs: [common-tests, ui-tests]
    if: always()
    
    steps:
    - name: 下载所有测试结果
      uses: actions/download-artifact@v4
      
    - name: 生成测试汇总报告
      run: |
        echo "# 测试执行汇总报告" > test-summary.md
        echo "" >> test-summary.md
        echo "## 执行时间: $(date)" >> test-summary.md
        echo "" >> test-summary.md
        
        # 汇总 Common.Tests 结果
        if [ -d "common-test-results-*" ]; then
          echo "### Common.Tests 结果" >> test-summary.md
          echo "\`\`\`" >> test-summary.md
          find common-test-results-* -name "*.trx" | wc -l
          echo "\`\`\`" >> test-summary.md
          echo "" >> test-summary.md
        fi
        
        # 汇总 UI.Tests 结果
        if [ -d "ui-test-results-*" ]; then
          echo "### UI.Tests 结果" >> test-summary.md
          echo "\`\`\`" >> test-summary.md
          find ui-test-results-* -name "*.trx" | wc -l
          echo "\`\`\`" >> test-summary.md
          echo "" >> test-summary.md
        fi
        
        echo "## 测试数据状态" >> test-summary.md
        echo "- 数据验证结果: ${{ needs.test-data-setup.outputs.validation-result }}" >> test-summary.md
        echo "- 数据哈希: ${{ needs.test-data-setup.outputs.test-data-hash }}" >> test-summary.md
        
    - name: 上传汇总报告
      uses: actions/upload-artifact@v4
      with:
        name: test-summary-report
        path: test-summary.md
```

### PowerShell 模块

#### 1. TestDataSync.psm1 (同步模块)
```powershell
# TestDataManager.psm1 - 测试数据管理模块

# 全局变量
$script:LogPath = ""
$script:ScriptName = ""
$script:Configuration = ""
$script:SyncHistory = @()

function Initialize-TestDataSync {
    param(
        [string]$ScriptName,
        [string]$Configuration = "Release"
    )
    
    $script:ScriptName = $ScriptName
    $script:Configuration = $Configuration
    
    # 创建日志目录
    $logDir = "$(Split-Path $PSScriptRoot -Parent)/logs"
    if (-not (Test-Path $logDir)) {
        New-Item -ItemType Directory -Path $logDir -Force | Out-Null
    }
    
    $script:LogPath = "$logDir/$($ScriptName)-$(Get-Date -Format 'yyyyMMdd').log"
    
    Write-Log "初始化 $ScriptName" -Level "INFO"
    Write-Log "配置: $Configuration" -Level "INFO"
}

function Write-Log {
    param(
        [string]$Message,
        [string]$Level = "INFO"
    )
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] [$Level] $Message"
    
    # 写入日志文件
    if ($script:LogPath) {
        Add-Content -Path $script:LogPath -Value $logMessage
    }
    
    # 输出到控制台
    switch ($Level) {
        "ERROR" { Write-Error $logMessage }
        "WARNING" { Write-Warning $logMessage }
        default { Write-Host $logMessage }
    }
}

function Sync-TestDataToProject {
    param(
        [string]$SourcePath,
        [string]$ProjectPath,
        [string]$Configuration,
        [switch]$Force,
        [switch]$Verify,
        [switch]$UseSymbolicLinks
    )
    
    Write-Log "同步测试数据到项目: $ProjectPath" -Level "INFO"
    
    $targetPath = "$ProjectPath/TestData"
    
    # 检查目标路径是否存在
    if (-not (Test-Path $ProjectPath)) {
        throw "项目路径不存在: $ProjectPath"
    }
    
    # 创建目标目录
    if (-not (Test-Path $targetPath)) {
        New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
        Write-Log "创建目标目录: $targetPath" -Level "INFO"
    }
    
    # 清空目标目录（如果强制同步）
    if ($Force -and (Test-Path $targetPath)) {
        Remove-Item -Path "$targetPath/*" -Recurse -Force
        Write-Log "清空目标目录: $targetPath" -Level "INFO"
    }
    
    # 同步文件
    $syncResult = @{
        SourcePath = $SourcePath
        TargetPath = $targetPath
        SyncedFiles = @()
        FailedFiles = @()
        StartTime = Get-Date
        EndTime = $null
        Success = $false
    }
    
    try {
        if ($UseSymbolicLinks -and $env:OS -ne "Windows_NT") {
            # 使用符号链接
            Create-SymbolicLinks -SourcePath $SourcePath -TargetPath $targetPath
        } else {
            # 复制文件
            Copy-TestDataFiles -SourcePath $SourcePath -TargetPath $targetPath -Force:$Force
        }
        
        # 验证同步结果
        if ($Verify) {
            $validationResult = Test-SyncResult -SourcePath $SourcePath -TargetPath $targetPath
            if (-not $validationResult.Success) {
                throw "同步验证失败: $($validationResult.ErrorMessage)"
            }
        }
        
        $syncResult.Success = $true
        $syncResult.EndTime = Get-Date
        
        Write-Log "同步成功完成" -Level "INFO"
    }
    catch {
        $syncResult.Success = $false
        $syncResult.EndTime = Get-Date
        $syncResult.ErrorMessage = $_.Exception.Message
        
        Write-Log "同步失败: $($_.Exception.Message)" -Level "ERROR"
        throw
    }
    finally {
        # 记录同步历史
        $script:SyncHistory += $syncResult
    }
}

function Create-SymbolicLinks {
    param(
        [string]$SourcePath,
        [string]$TargetPath
    )
    
    Write-Log "创建符号链接: $SourcePath -> $TargetPath" -Level "INFO"
    
    # 获取源目录中的所有文件
    $files = Get-ChildItem -Path $SourcePath -Recurse -File
    
    foreach ($file in $files) {
        $relativePath = $file.FullName.Substring($SourcePath.Length).TrimStart('\')
        $targetFile = Join-Path $TargetPath $relativePath
        
        # 创建目标目录
        $targetDir = Split-Path $targetFile -Parent
        if (-not (Test-Path $targetDir)) {
            New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        }
        
        # 创建符号链接
        try {
            New-Item -ItemType SymbolicLink -Path $targetFile -Target $file.FullName -Force | Out-Null
            Write-Log "创建符号链接: $targetFile" -Level "DEBUG"
        }
        catch {
            Write-Log "创建符号链接失败: $targetFile, 尝试复制文件" -Level "WARNING"
            Copy-Item -Path $file.FullName -Destination $targetFile -Force
        }
    }
}

function Copy-TestDataFiles {
    param(
        [string]$SourcePath,
        [string]$TargetPath,
        [switch]$Force
    )
    
    Write-Log "复制测试数据文件: $SourcePath -> $TargetPath" -Level "INFO"
    
    # 复制所有文件
    Copy-Item -Path "$SourcePath/*" -Destination $TargetPath -Recurse -Force:$Force
    
    Write-Log "文件复制完成" -Level "INFO"
}

function Test-SyncResult {
    param(
        [string]$SourcePath,
        [string]$TargetPath
    )
    
    Write-Log "验证同步结果: $SourcePath -> $TargetPath" -Level "INFO"
    
    # 检查关键文件是否存在
    $requiredFiles = @("action_types.xml", "combat_parameters.xml", "attributes.xml")
    
    foreach ($file in $requiredFiles) {
        $sourceFile = Join-Path $SourcePath $file
        $targetFile = Join-Path $TargetPath $file
        
        if (-not (Test-Path $targetFile)) {
            return @{
                Success = $false
                ErrorMessage = "缺少必需的文件: $file"
            }
        }
        
        # 比较文件大小
        $sourceSize = (Get-Item $sourceFile).Length
        $targetSize = (Get-Item $targetFile).Length
        
        if ($sourceSize -ne $targetSize) {
            return @{
                Success = $false
                ErrorMessage = "文件大小不匹配: $file (源: $sourceSize, 目标: $targetSize)"
            }
        }
    }
    
    return @{
        Success = $true
        ErrorMessage = ""
    }
}

function Generate-SyncReport {
    $report = @{
        Timestamp = Get-Date
        ScriptName = $script:ScriptName
        Configuration = $script:Configuration
        SyncHistory = $script:SyncHistory
        Statistics = @{
            TotalSyncs = $script:SyncHistory.Count
            SuccessfulSyncs = ($script:SyncHistory | Where-Object { $_.Success }).Count
            FailedSyncs = ($script:SyncHistory | Where-Object { -not $_.Success }).Count
        }
    }
    
    return $report
}

function Save-SyncReport {
    param(
        $Report,
        [string]$OutputPath
    )
    
    $reportDir = Split-Path $OutputPath -Parent
    if (-not (Test-Path $reportDir)) {
        New-Item -ItemType Directory -Path $reportDir -Force | Out-Null
    }
    
    $Report | ConvertTo-Json -Depth 10 | Set-Content -Path $OutputPath
    Write-Log "同步报告已保存: $OutputPath" -Level "INFO"
}

function Cleanup-TestDataSync {
    Write-Log "清理 $script:ScriptName" -Level "INFO"
    
    # 清理历史记录（只保留最近100条）
    if ($script:SyncHistory.Count -gt 100) {
        $script:SyncHistory = $script:SyncHistory[-100..-1]
    }
}

# 导出函数
Export-ModuleMember -Function @(
    'Initialize-TestDataSync',
    'Write-Log',
    'Sync-TestDataToProject',
    'Generate-SyncReport',
    'Save-SyncReport',
    'Cleanup-TestDataSync'
)
```

## 实施计划

### 第一阶段：基础架构搭建（1-2天）
1. **创建统一的TestData目录结构**
2. **实现基础的数据同步脚本**
3. **更新项目文件配置**

### 第二阶段：自动化实现（2-3天）
1. **实现CI/CD流程集成**
2. **添加数据验证机制**
3. **实现错误处理和诊断**

### 第三阶段：优化和测试（1-2天）
1. **性能优化**
2. **监控和报告**
3. **文档完善**

## 监控和维护

### 关键监控指标
- 测试数据同步成功率
- 测试执行成功率
- 数据完整性状态
- 错误率和恢复时间

### 维护策略
- 定期数据备份
- 自动化健康检查
- 版本控制和回滚机制
- 持续的性能监控

## 总结

本技术栈和配置方案提供了完整的解决方案来修复GitHub Actions UI测试失败问题。通过选择成熟的技术组合和合理的配置，可以确保：

1. **可靠性**: 使用.NET 9.0和PowerShell提供稳定的技术基础
2. **自动化**: 通过脚本和CI/CD集成实现完全自动化
3. **跨平台**: 支持Windows、Linux和MacOS
4. **可维护性**: 模块化设计和完善的日志记录
5. **可扩展性**: 易于添加新功能和测试项目

通过实施此方案，可以显著提高测试的可靠性和开发效率。