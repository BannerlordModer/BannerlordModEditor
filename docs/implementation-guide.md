# 实施指南

## 概述

本指南详细说明了如何实施修复GitHub Actions UI测试失败问题的解决方案。按照本指南的步骤，您可以建立完整的测试数据管理系统。

## 实施前准备

### 环境要求
- .NET 9.0 SDK
- PowerShell 7.4+
- Git
- GitHub仓库访问权限

### 权限要求
- 仓库写入权限
- CI/CD配置权限
- 文件系统操作权限

## 实施步骤

### 第一步：创建统一的TestData目录结构

#### 1.1 创建主TestData目录
```bash
# 在解决方案根目录创建TestData目录
mkdir -p TestData/XmlFiles
mkdir -p TestData/Configs
mkdir -p TestData/Generated
mkdir -p TestData/Backups
```

#### 1.2 从Common.Tests复制现有测试数据
```bash
# 复制现有的XML测试文件
cp -r BannerlordModEditor.Common.Tests/TestData/*.xml TestData/XmlFiles/

# 复制其他必要的文件
cp -r BannerlordModEditor.Common.Tests/TestData/Layouts TestData/
cp -r BannerlordModEditor.Common.Tests/TestData/SourceFiles TestData/
cp -r BannerlordModEditor.Common.Tests/TestData/AchievementData TestData/
```

#### 1.3 创建配置文件
```bash
# 创建配置目录
mkdir -p scripts/modules

# 创建必要的配置文件
touch TestData/Configs/test-data-config.json
touch TestData/Configs/test-data-mapping.json
touch scripts/test-data-sync.ps1
touch scripts/test-data-manager.ps1
touch scripts/modules/TestDataSync.psm1
touch scripts/modules/TestDataManager.psm1
```

### 第二步：配置项目文件

#### 2.1 更新Common.Tests项目文件
编辑 `BannerlordModEditor.Common.Tests/BannerlordModEditor.Common.Tests.csproj`：

```xml
<!-- 添加在PropertyGroup之后 -->
<PropertyGroup>
  <!-- 现有配置... -->
  
  <!-- 测试数据配置 -->
  <TestDataPath>$(MSBuildProjectDirectory)/../TestData</TestDataPath>
  <EnableTestDataSync>true</EnableTestDataSync>
</PropertyGroup>

<!-- 添加在PackageReference之后 -->
<ItemGroup>
  <!-- 现有包引用... -->
  
  <!-- 新增的测试支持包 -->
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
  <PackageReference Include="Serilog" Version="3.1.1" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
  <PackageReference Include="Polly" Version="8.4.1" />
</ItemGroup>

<!-- 替换现有的Content配置 -->
<ItemGroup>
  <Content Include="..\TestData\**\*.*">
    <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <Visible>false</Visible>
  </Content>
</ItemGroup>

<!-- 添加构建后事件 -->
<Target Name="SyncTestData" AfterTargets="Build">
  <Exec Command="powershell -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)/../scripts/test-data-sync.ps1&quot; -ProjectPath &quot;$(MSBuildProjectDirectory)&quot; -Configuration $(Configuration)" />
</Target>
```

#### 2.2 更新UI.Tests项目文件
编辑 `BannerlordModEditor.UI.Tests/BannerlordModEditor.UI.Tests.csproj`：

```xml
<!-- 添加在PropertyGroup之后 -->
<PropertyGroup>
  <!-- 现有配置... -->
  
  <!-- 测试数据配置 -->
  <TestDataPath>$(MSBuildProjectDirectory)/../TestData</TestDataPath>
  <EnableTestDataSync>true</EnableTestDataSync>
  
  <!-- UI测试特定配置 -->
  <UITestTimeout>120</UITestTimeout>
  <EnableHeadlessMode>true</EnableHeadlessMode>
</PropertyGroup>

<!-- 添加在PackageReference之后 -->
<ItemGroup>
  <!-- 现有包引用... -->
  
  <!-- 新增的测试支持包 -->
  <PackageReference Include="Serilog" Version="3.1.1" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
  <PackageReference Include="Polly" Version="8.4.1" />
  <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks" Version="9.0.0" />
</ItemGroup>

<!-- 添加测试数据文件配置 -->
<ItemGroup>
  <Content Include="..\TestData\**\*.*">
    <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    <Visible>false</Visible>
  </Content>
</ItemGroup>

<!-- 添加构建后事件 -->
<Target Name="SyncTestData" AfterTargets="Build">
  <Exec Command="powershell -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)/../scripts/test-data-sync.ps1&quot; -ProjectPath &quot;$(MSBuildProjectDirectory)&quot; -Configuration $(Configuration) -Force" />
</Target>
```

### 第三步：实现测试数据同步脚本

#### 3.1 创建主同步脚本
创建 `scripts/test-data-sync.ps1`：

```powershell
#!/usr/bin/env pwsh

<#
.SYNOPSIS
    测试数据同步脚本
.DESCRIPTION
    同步测试数据到各个测试项目，确保数据一致性
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
    exit 1
}
finally {
    # 清理资源
    Cleanup-TestDataSync
}
```

#### 3.2 创建同步模块
创建 `scripts/modules/TestDataSync.psm1`：

```powershell
# TestDataSync.psm1 - 测试数据同步模块

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
    $logDir = "$(Split-Path $PSScriptRoot -Parent)/../logs"
    if (-not (Test-Path $logDir)) {
        New-Item -ItemType Directory -Path $logDir -Force | Out-Null
    }
    
    $script:LogPath = "$logDir/$($ScriptName)-$(Get-Date -Format 'yyyyMMdd').log"
    
    Write-Log "初始化 $ScriptName" -Level "INFO"
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

### 第四步：创建配置文件

#### 4.1 创建测试数据配置文件
创建 `TestData/Configs/test-data-config.json`：

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

#### 4.2 创建数据映射配置文件
创建 `TestData/Configs/test-data-mapping.json`：

```json
{
  "fileMappings": {
    "action_types.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "minSize": 1024,
        "maxSize": 1048576
      }
    },
    "combat_parameters.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "minSize": 2048,
        "maxSize": 2097152
      }
    },
    "attributes.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
        "minSize": 512,
        "maxSize": 524288
      }
    },
    "banner_icons.xml": {
      "targetProjects": ["common", "ui"],
      "validation": {
        "required": true,
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

### 第五步：更新GitHub Actions工作流

#### 5.1 优化现有的CI/CD工作流
编辑 `.github/workflows/dotnet-desktop.yml`：

```yaml
name: Build, Test and Deploy

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
  test:
    runs-on: ${{ matrix.os }}
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest]
        configuration: [Debug, Release]
    
    steps:
    - name: Checkout Repository
      uses: actions/checkout@v4
      
    - name: Install .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
        
    - name: Restore Dependencies
      run: dotnet restore
      
    - name: Sync Test Data
      run: scripts/test-data-sync.ps1 -Verify -Force
      
    - name: Validate Test Data
      run: scripts/test-data-manager.ps1 -Action Validate
      
    - name: Build Solution
      run: dotnet build --configuration ${{ matrix.configuration }} --no-restore
      
    - name: Run Unit Tests
      run: dotnet test BannerlordModEditor.Common.Tests --configuration ${{ matrix.configuration }} --no-build --verbosity normal --logger "trx;LogFileName=unit_tests.trx" --collect:"XPlat Code Coverage" --results-directory TestResults
      
    - name: Run UI Tests
      env:
        DOTNET_CLI_TEST_TIMEOUT: 120
        TEST_DATA_PATH: ${{ env.TEST_DATA_PATH }}
      run: |
        # 诊断测试数据状态
        scripts/test-data-manager.ps1 -Action Diagnose
        
        # 运行UI测试
        dotnet test BannerlordModEditor.UI.Tests --configuration ${{ matrix.configuration }} --no-build --verbosity normal --filter "Category!=Integration" --logger "trx;LogFileName=ui_tests.trx" --collect:"XPlat Code Coverage" --results-directory TestResults || {
          echo "UI测试失败，收集诊断信息..."
          find TestData -name "*.xml" | head -5
          echo "TestData 目录大小:"
          du -sh TestData/
          exit 1
        }
      
    - name: Upload Test Results
      if: always()
      uses: actions/upload-artifact@v4
      with:
        name: test-results-${{ matrix.os }}-${{ matrix.configuration }}
        path: |
          TestResults/
          *.trx
          TestData/
```

### 第六步：测试和验证

#### 6.1 本地测试同步功能
```bash
# 在项目根目录执行
./scripts/test-data-sync.ps1 -Verify

# 验证测试数据
./scripts/test-data-manager.ps1 -Action Validate

# 运行诊断
./scripts/test-data-manager.ps1 -Action Diagnose
```

#### 6.2 构建和测试项目
```bash
# 构建解决方案
dotnet build --configuration Release

# 运行Common.Tests
dotnet test BannerlordModEditor.Common.Tests --configuration Release

# 运行UI.Tests
dotnet test BannerlordModEditor.UI.Tests --configuration Release
```

#### 6.3 验证CI/CD流程
1. 提交代码到GitHub仓库
2. 检查GitHub Actions工作流执行情况
3. 验证测试结果和报告

### 第七步：部署和监控

#### 7.1 设置监控
- 配置GitHub Actions通知
- 设置测试失败警报
- 定期检查同步日志

#### 7.2 维护计划
- 定期清理旧的备份文件
- 更新配置文件以适应新的测试需求
- 监控磁盘空间使用情况

## 故障排除

### 常见问题

#### 1. 权限错误
**问题**: 无法创建符号链接或复制文件
**解决方案**:
```bash
# Linux/Mac
chmod +x scripts/*.ps1

# Windows
Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
```

#### 2. 测试数据验证失败
**问题**: 验证脚本报告文件缺失
**解决方案**:
```bash
# 检查文件是否存在
ls -la TestData/XmlFiles/

# 重新同步数据
./scripts/test-data-sync.ps1 -Force -Verify
```

#### 3. GitHub Actions失败
**问题**: CI/CD工作流执行失败
**解决方案**:
1. 检查工作流日志
2. 验证脚本权限
3. 确保测试数据文件在正确的位置

#### 4. UI测试仍然失败
**问题**: UI测试报告数据文件不存在
**解决方案**:
```bash
# 检查输出目录
ls -la BannerlordModEditor.UI.Tests/bin/Release/net9.0/TestData/

# 手动复制测试数据
cp -r TestData/* BannerlordModEditor.UI.Tests/bin/Release/net9.0/TestData/
```

### 调试技巧

#### 1. 启用详细日志
```bash
./scripts/test-data-sync.ps1 -Verbose
```

#### 2. 检查同步历史
```bash
./scripts/test-data-manager.ps1 -Action Report
```

#### 3. 验证文件完整性
```bash
# 检查文件哈希
find TestData -type f -exec md5sum {} \;
```

## 最佳实践

### 1. 版本控制
- 将配置文件纳入版本控制
- 使用.gitignore排除临时文件
- 定期备份重要的测试数据

### 2. 安全考虑
- 不要在配置文件中存储敏感信息
- 使用环境变量存储密钥
- 定期更新依赖包

### 3. 性能优化
- 使用符号链接减少磁盘空间使用
- 定期清理旧的日志文件
- 监控构建时间

### 4. 团队协作
- 在README中说明测试数据管理流程
- 提供详细的错误处理指南
- 定期进行团队培训

## 总结

通过按照本实施指南的步骤，您可以建立完整的测试数据管理系统，有效解决GitHub Actions UI测试失败的问题。关键成功因素包括：

1. **统一管理**: 所有测试数据集中存储
2. **自动化**: 通过脚本实现自动化同步
3. **验证机制**: 确保数据完整性
4. **错误处理**: 提供详细的诊断信息
5. **持续监控**: 及时发现和解决问题

实施后，您将拥有一个可靠、可维护的测试数据管理系统，显著提高测试的稳定性和开发效率。