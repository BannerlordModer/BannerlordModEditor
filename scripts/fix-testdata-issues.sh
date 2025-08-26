#!/bin/bash

# BannerlordModEditor CLI TestData问题修复脚本
# 用于解决TUI测试TestData复制问题和统一测试项目配置

set -e  # 遇到错误立即退出

echo "=== BannerlordModEditor CLI TestData问题修复脚本 ==="
echo "开始时间: $(date)"
echo ""

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 日志函数
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# 检查必要文件和目录
check_prerequisites() {
    log_info "检查必要文件和目录..."
    
    if [ ! -f "BannerlordModEditor.sln" ]; then
        log_error "未找到解决方案文件 BannerlordModEditor.sln"
        exit 1
    fi
    
    if [ ! -d "BannerlordModEditor.Common.Tests" ]; then
        log_error "未找到 Common.Tests 项目目录"
        exit 1
    fi
    
    if [ ! -d "BannerlordModEditor.Common.Tests/TestData" ]; then
        log_error "未找到 Common.Tests/TestData 目录"
        exit 1
    fi
    
    log_success "必要文件和目录检查完成"
}

# 创建TestData符号链接
create_testdata_links() {
    log_info "创建TestData符号链接..."
    
    # 需要创建符号链接的项目目录
    projects=(
        "BannerlordModEditor.TUI.Tests"
        "BannerlordModEditor.TUI.UATTests"
        "BannerlordModEditor.Cli.UATTests"
    )
    
    for project in "${projects[@]}"; do
        if [ -d "$project" ]; then
            log_info "处理项目: $project"
            
            # 删除现有的TestData目录或符号链接
            if [ -L "$project/TestData" ]; then
                rm "$project/TestData"
                log_info "删除现有符号链接: $project/TestData"
            elif [ -d "$project/TestData" ]; then
                log_warning "项目 $project 已存在TestData目录，将其备份"
                mv "$project/TestData" "$project/TestData.backup.$(date +%Y%m%d_%H%M%S)"
            fi
            
            # 创建符号链接
            ln -s "../../BannerlordModEditor.Common.Tests/TestData" "$project/TestData"
            log_success "创建符号链接: $project/TestData -> ../../BannerlordModEditor.Common.Tests/TestData"
        else
            log_warning "项目目录不存在: $project"
        fi
    done
}

# 修复TUI测试项目配置
fix_tui_test_projects() {
    log_info "修复TUI测试项目配置..."
    
    # BannerlordModEditor.TUI.Tests.csproj
    if [ -f "BannerlordModEditor.TUI.Tests/BannerlordModEditor.TUI.Tests.csproj" ]; then
        log_info "修复 BannerlordModEditor.TUI.Tests.csproj"
        
        # 备份原始文件
        cp "BannerlordModEditor.TUI.Tests/BannerlordModEditor.TUI.Tests.csproj" "BannerlordModEditor.TUI.Tests/BannerlordModEditor.TUI.Tests.csproj.backup"
        
        # 创建新的项目文件
        cat > "BannerlordModEditor.TUI.Tests/BannerlordModEditor.TUI.Tests.csproj" << 'EOF'
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="FluentAssertions" Version="6.12.1" />
    <PackageReference Include="Shouldly" Version="4.2.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\BannerlordModEditor.TUI\BannerlordModEditor.TUI.csproj" />
    <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
  </ItemGroup>

  <ItemGroup>
    <!-- 引用Common.Tests的TestData -->
    <Content Include="..\BannerlordModEditor.Common.Tests\TestData\**\*.*">
      <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
EOF
        
        log_success "BannerlordModEditor.TUI.Tests.csproj 修复完成"
    else
        log_error "未找到 BannerlordModEditor.TUI.Tests.csproj"
    fi
    
    # BannerlordModEditor.TUI.UATTests.csproj
    if [ -f "BannerlordModEditor.TUI.UATTests/BannerlordModEditor.TUI.UATTests.csproj" ]; then
        log_info "修复 BannerlordModEditor.TUI.UATTests.csproj"
        
        # 备份原始文件
        cp "BannerlordModEditor.TUI.UATTests/BannerlordModEditor.TUI.UATTests.csproj" "BannerlordModEditor.TUI.UATTests/BannerlordModEditor.TUI.UATTests.csproj.backup"
        
        # 创建新的项目文件
        cat > "BannerlordModEditor.TUI.UATTests/BannerlordModEditor.TUI.UATTests.csproj" << 'EOF'
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>BannerlordModEditor.TUI.UATTests</RootNamespace>
    
    <!-- 测试项目设置 -->
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    
    <!-- BDD测试特定设置 -->
    <AssemblyTitle>BannerlordModEditor TUI User Acceptance Tests</AssemblyTitle>
    <AssemblyDescription>BDD-style UAT tests for BannerlordModEditor TUI application</AssemblyDescription>
  </PropertyGroup>

  <ItemGroup>
    <!-- xUnit测试框架 -->
    <PackageReference Include="System.IO.Packaging" Version="8.0.1" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    
    <!-- BDD风格测试支持 -->
    <PackageReference Include="FluentAssertions" Version="6.12.1" />
    <PackageReference Include="Shouldly" Version="4.2.1" />
    <PackageReference Include="Moq" Version="4.20.70" />
    
    <!-- 测试工具 -->
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    
    <!-- 文件和系统操作 -->
    <PackageReference Include="System.IO.Abstractions" Version="21.1.4" />
    <PackageReference Include="System.IO.Abstractions.TestingHelpers" Version="21.1.4" />
  </ItemGroup>

  <ItemGroup>
    <!-- 项目引用 -->
    <ProjectReference Include="..\BannerlordModEditor.TUI\BannerlordModEditor.TUI.csproj" />
    <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
  </ItemGroup>

  <ItemGroup>
    <!-- 引用Common.Tests的TestData -->
    <Content Include="..\BannerlordModEditor.Common.Tests\TestData\**\*.*">
      <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
EOF
        
        log_success "BannerlordModEditor.TUI.UATTests.csproj 修复完成"
    else
        log_error "未找到 BannerlordModEditor.TUI.UATTests.csproj"
    fi
}

# 修复UAT测试项目配置
fix_uat_test_projects() {
    log_info "修复UAT测试项目配置..."
    
    # BannerlordModEditor.Cli.UATTests.csproj
    if [ -f "BannerlordModEditor.Cli.UATTests/BannerlordModEditor.Cli.UATTests.csproj" ]; then
        log_info "修复 BannerlordModEditor.Cli.UATTests.csproj"
        
        # 备份原始文件
        cp "BannerlordModEditor.Cli.UATTests/BannerlordModEditor.Cli.UATTests.csproj" "BannerlordModEditor.Cli.UATTests/BannerlordModEditor.Cli.UATTests.csproj.backup"
        
        # 创建新的项目文件
        cat > "BannerlordModEditor.Cli.UATTests/BannerlordModEditor.Cli.UATTests.csproj" << 'EOF'
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.2" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
    <PackageReference Include="FluentAssertions" Version="6.12.1" />
    <PackageReference Include="Shouldly" Version="4.2.1" />
    <PackageReference Include="Moq" Version="4.20.70" />
    <PackageReference Include="System.IO.Abstractions" Version="21.1.4" />
    <PackageReference Include="System.IO.Abstractions.TestingHelpers" Version="21.1.4" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\BannerlordModEditor.Cli\BannerlordModEditor.Cli.csproj" />
    <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
    <Using Include="FluentAssertions" />
    <Using Include="Shouldly" />
  </ItemGroup>

  <ItemGroup>
    <!-- 引用Common.Tests的TestData -->
    <Content Include="..\BannerlordModEditor.Common.Tests\TestData\**\*.*">
      <Link>TestData\%(RecursiveDir)%(Filename)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
EOF
        
        log_success "BannerlordModEditor.Cli.UATTests.csproj 修复完成"
    else
        log_error "未找到 BannerlordModEditor.Cli.UATTests.csproj"
    fi
}

# 重新启用UAT测试项目
enable_uat_project() {
    log_info "重新启用UAT测试项目..."
    
    if [ -f "BannerlordModEditor.sln" ]; then
        # 备份解决方案文件
        cp "BannerlordModEditor.sln" "BannerlordModEditor.sln.backup"
        
        # 移除注释的UAT项目
        sed -i '/^\/\/ Project.*BannerlordModEditor.Cli.UATTests/,/^\/\/ EndProject$/c\
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "BannerlordModEditor.Cli.UATTests", "BannerlordModEditor.Cli.UATTests\BannerlordModEditor.Cli.UATTests.csproj", "{G8F1E3A2-F226-4925-9AD3-E8C0C7721CCG}"\
EndProject' BannerlordModEditor.sln
        
        # 移除注释行
        sed -i '/^\/\/ 暂时注释掉UAT测试项目/d' BannerlordModEditor.sln
        
        log_success "UAT测试项目已重新启用"
    else
        log_error "未找到解决方案文件"
    fi
}

# 验证修复结果
verify_fixes() {
    log_info "验证修复结果..."
    
    # 验证TestData符号链接
    projects=(
        "BannerlordModEditor.TUI.Tests"
        "BannerlordModEditor.TUI.UATTests"
        "BannerlordModEditor.Cli.UATTests"
    )
    
    for project in "${projects[@]}"; do
        if [ -L "$project/TestData" ]; then
            if [ -e "$project/TestData" ]; then
                log_success "$project/TestData 符号链接有效"
            else
                log_error "$project/TestData 符号链接无效"
            fi
        else
            log_warning "$project/TestData 符号链接不存在"
        fi
    done
    
    # 验证项目文件
    log_info "验证项目文件配置..."
    
    # 检查TestData配置
    for project in "${projects[@]}"; do
        if [ -f "$project/$project.csproj" ]; then
            if grep -q "BannerlordModEditor.Common.Tests.TestData" "$project/$project.csproj"; then
                log_success "$project.csproj 包含TestData配置"
            else
                log_warning "$project.csproj 缺少TestData配置"
            fi
        fi
    done
    
    # 尝试构建项目
    log_info "尝试构建解决方案..."
    if dotnet build --configuration Release --verbosity quiet; then
        log_success "解决方案构建成功"
    else
        log_error "解决方案构建失败"
        return 1
    fi
    
    # 尝试运行测试
    log_info "尝试运行测试..."
    if dotnet test --configuration Release --verbosity quiet --no-build; then
        log_success "测试运行成功"
    else
        log_warning "测试运行失败，可能需要进一步调试"
    fi
}

# 创建验证脚本
create_validation_script() {
    log_info "创建验证脚本..."
    
    cat > "scripts/validate-testdata.sh" << 'EOF'
#!/bin/bash

# TestData验证脚本

echo "=== TestData验证脚本 ==="
echo "验证时间: $(date)"
echo ""

# 检查Common.Tests的TestData
if [ ! -d "BannerlordModEditor.Common.Tests/TestData" ]; then
    echo "错误: Common.Tests TestData目录不存在"
    exit 1
fi

echo "Common.Tests TestData文件数量: $(find BannerlordModEditor.Common.Tests/TestData -name "*.xml" | wc -l)"

# 检查测试项目中的TestData文件
projects=(
    "BannerlordModEditor.TUI.Tests"
    "BannerlordModEditor.TUI.UATTests"
    "BannerlordModEditor.Cli.UATTests"
)

for project in "${projects[@]}"; do
    echo ""
    echo "检查项目: $project"
    
    if [ -L "$project/TestData" ]; then
        echo "✓ 符号链接存在"
        if [ -e "$project/TestData" ]; then
            echo "✓ 符号链接有效"
            xml_count=$(find "$project/TestData" -name "*.xml" | wc -l)
            echo "✓ XML文件数量: $xml_count"
        else
            echo "✗ 符号链接无效"
        fi
    elif [ -d "$project/TestData" ]; then
        echo "⚠ TestData目录存在（非符号链接）"
        xml_count=$(find "$project/TestData" -name "*.xml" | wc -l)
        echo "XML文件数量: $xml_count"
    else
        echo "✗ TestData目录不存在"
    fi
done

echo ""
echo "验证完成"
EOF

    chmod +x scripts/validate-testdata.sh
    log_success "验证脚本创建完成: scripts/validate-testdata.sh"
}

# 创建回滚脚本
create_rollback_script() {
    log_info "创建回滚脚本..."
    
    cat > "scripts/rollback-testdata-fixes.sh" << 'EOF'
#!/bin/bash

# TestData修复回滚脚本

echo "=== TestData修复回滚脚本 ==="
echo "回滚时间: $(date)"
echo ""

# 恢复备份的项目文件
projects=(
    "BannerlordModEditor.TUI.Tests"
    "BannerlordModEditor.TUI.UATTests"
    "BannerlordModEditor.Cli.UATTests"
)

for project in "${projects[@]}"; do
    if [ -f "$project/$project.csproj.backup" ]; then
        echo "恢复 $project.csproj"
        mv "$project/$project.csproj.backup" "$project/$project.csproj"
    fi
done

# 恢复解决方案文件
if [ -f "BannerlordModEditor.sln.backup" ]; then
    echo "恢复解决方案文件"
    mv "BannerlordModEditor.sln.backup" "BannerlordModEditor.sln"
fi

# 删除符号链接
for project in "${projects[@]}"; do
    if [ -L "$project/TestData" ]; then
        echo "删除符号链接: $project/TestData"
        rm "$project/TestData"
    fi
done

# 恢复备份的TestData目录
for project in "${projects[@]}"; do
    backup_dirs=$(find "$project" -name "TestData.backup.*" -type d)
    for backup_dir in $backup_dirs; do
        echo "恢复TestData目录: $backup_dir"
        mv "$backup_dir" "$project/TestData"
    done
done

echo "回滚完成"
EOF

    chmod +x scripts/rollback-testdata-fixes.sh
    log_success "回滚脚本创建完成: scripts/rollback-testdata-fixes.sh"
}

# 主执行流程
main() {
    log_info "开始执行TestData问题修复..."
    
    check_prerequisites
    create_testdata_links
    fix_tui_test_projects
    fix_uat_test_projects
    enable_uat_project
    verify_fixes
    create_validation_script
    create_rollback_script
    
    log_success "TestData问题修复完成！"
    echo ""
    echo "后续步骤："
    echo "1. 运行验证脚本: ./scripts/validate-testdata.sh"
    echo "2. 运行测试: dotnet test"
    echo "3. 提交更改: git add . && git commit -m '修复TestData问题和UAT测试配置'"
    echo "4. 如果需要回滚: ./scripts/rollback-testdata-fixes.sh"
}

# 脚本入口点
if [ "${BASH_SOURCE[0]}" == "${0}" ]; then
    main "$@"
fi