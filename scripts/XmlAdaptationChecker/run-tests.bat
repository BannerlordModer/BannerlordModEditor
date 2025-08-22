@echo off
REM XML适配状态检查工具测试运行脚本 (Windows版本)
REM 此脚本运行所有测试并生成覆盖率报告

echo ==========================================
echo XML适配状态检查工具测试套件
echo ==========================================

REM 获取脚本所在目录
set SCRIPT_DIR=%~dp0
set PROJECT_DIR=%~dp0..

REM 颜色定义 (Windows 10+)
set "RED=[91m"
set "GREEN=[92m"
set "YELLOW=[93m"
set "BLUE=[94m"
set "NC=[0m"

REM 日志函数
:log_info
echo %BLUE%[INFO]%NC% %~1
goto :eof

:log_success
echo %GREEN%[SUCCESS]%NC% %~1
goto :eof

:log_warning
echo %YELLOW%[WARNING]%NC% %~1
goto :eof

:log_error
echo %RED%[ERROR]%NC% %~1
goto :eof

REM 检查依赖
call :check_dependencies
if %ERRORLEVEL% neq 0 exit /b 1

REM 构建项目
call :build_project
if %ERRORLEVEL% neq 0 exit /b 1

REM 运行测试
if "%1"=="" (
    call :run_all_tests
) else if "%1"=="-u" (
    call :run_unit_tests
) else if "%1"=="-i" (
    call :run_integration_tests
) else if "%1"=="-p" (
    call :run_performance_tests
) else if "%1"=="-c" (
    call :generate_coverage_report
) else if "%1"=="-b" (
    call :build_only
) else if "%1"=="--clean" (
    call :cleanup_test_files
) else if "%1"=="-h" (
    call :show_help
    exit /b 0
) else (
    call :run_all_tests
)

REM 显示测试结果摘要
call :show_test_summary

REM 显示结束信息
echo.
echo 测试完成时间: %date% %time%
call :log_success "测试套件执行完成！"
exit /b 0

REM 检查依赖函数
:check_dependencies
call :log_info "检查依赖..."

REM 检查.NET SDK
where dotnet >nul 2>&1
if %ERRORLEVEL% neq 0 (
    call :log_error "未找到.NET SDK，请先安装.NET 9.0 SDK"
    exit /b 1
)

REM 检查dotnet版本
for /f "tokens=*" %%v in ('dotnet --version ^| findstr /r "^[0-9][.][0-9]"') do set DOTNET_VERSION=%%v
if not "%DOTNET_VERSION%"=="9.0" (
    call :log_warning "当前.NET版本为%DOTNET_VERSION%，建议使用.NET 9.0"
)

REM 检查是否安装了测试工具
dotnet tool list --global | findstr "coverlet" >nul 2>&1
if %ERRORLEVEL% neq 0 (
    call :log_info "安装coverlet代码覆盖率工具..."
    dotnet tool install --global coverlet.console
)

call :log_success "依赖检查完成"
exit /b 0

REM 构建项目函数
:build_project
call :log_info "构建项目..."

cd /d "%PROJECT_DIR%"

REM 恢复依赖
call :log_info "恢复NuGet包..."
dotnet restore
if %ERRORLEVEL% neq 0 exit /b 1

REM 构建项目
call :log_info "编译项目..."
dotnet build --configuration Release --no-restore
if %ERRORLEVEL% neq 0 exit /b 1

call :log_success "项目构建完成"
exit /b 0

REM 运行单元测试函数
:run_unit_tests
call :log_info "运行单元测试..."

cd /d "%PROJECT_DIR%"

REM 创建测试结果目录
if not exist "test-results" mkdir test-results

REM 运行单元测试
call :log_info "执行单元测试..."
dotnet test ^
    --configuration Release ^
    --no-build ^
    --logger "trx;LogFileName=test-results/unit-tests.trx" ^
    --logger "console;verbosity=minimal" ^
    --collect:"XPlat Code Coverage" ^
    --results-directory test-results ^
    --filter "TestCategory!=Integration^&TestCategory!=Performance"

if %ERRORLEVEL% neq 0 (
    call :log_error "单元测试失败"
    exit /b 1
)

call :log_success "单元测试完成"
exit /b 0

REM 运行集成测试函数
:run_integration_tests
call :log_info "运行集成测试..."

cd /d "%PROJECT_DIR%"

REM 运行集成测试
call :log_info "执行集成测试..."
dotnet test ^
    --configuration Release ^
    --no-build ^
    --logger "trx;LogFileName=test-results/integration-tests.trx" ^
    --logger "console;verbosity=minimal" ^
    --filter "TestCategory=Integration"

if %ERRORLEVEL% neq 0 (
    call :log_error "集成测试失败"
    exit /b 1
)

call :log_success "集成测试完成"
exit /b 0

REM 运行性能测试函数
:run_performance_tests
call :log_info "运行性能测试..."

cd /d "%PROJECT_DIR%"

REM 运行性能测试
call :log_info "执行性能测试..."
dotnet test ^
    --configuration Release ^
    --no-build ^
    --logger "trx;LogFileName=test-results/performance-tests.trx" ^
    --logger "console;verbosity=minimal" ^
    --filter "TestCategory=Performance"

if %ERRORLEVEL% neq 0 (
    call :log_error "性能测试失败"
    exit /b 1
)

call :log_success "性能测试完成"
exit /b 0

REM 生成覆盖率报告函数
:generate_coverage_report
call :log_info "生成代码覆盖率报告..."

cd /d "%PROJECT_DIR%"

REM 检查是否有覆盖率文件
if exist "test-results\coverage.xml" (
    call :log_info "生成HTML覆盖率报告..."
    
    REM 生成HTML报告（如果有reportgenerator工具）
    where reportgenerator >nul 2>&1
    if %ERRORLEVEL% equ 0 (
        reportgenerator ^
            -reports:test-results\coverage.xml ^
            -targetdir:test-results\coverage-report ^
            -reporttypes:HtmlInline_AzurePipelines
        call :log_success "HTML覆盖率报告已生成到 test-results\coverage-report\"
    ) else (
        call :log_warning "未找到reportgenerator工具，跳过HTML报告生成"
    )
) else (
    call :log_warning "未找到覆盖率数据文件"
)

exit /b 0

REM 运行所有测试函数
:run_all_tests
call :log_info "运行所有测试..."

cd /d "%PROJECT_DIR%"

REM 创建测试结果目录
if not exist "test-results" mkdir test-results

REM 运行所有测试
call :log_info "执行所有测试..."
dotnet test ^
    --configuration Release ^
    --no-build ^
    --logger "trx;LogFileName=test-results/all-tests.trx" ^
    --logger "console;verbosity=minimal" ^
    --collect:"XPlat Code Coverage" ^
    --results-directory test-results

if %ERRORLEVEL% neq 0 (
    call :log_error "部分测试失败"
    exit /b 1
)

call :log_success "所有测试完成"
exit /b 0

REM 显示测试结果摘要函数
:show_test_summary
call :log_info "测试结果摘要"

cd /d "%PROJECT_DIR%"

if exist "test-results" (
    echo 测试结果文件:
    dir /b test-results\*.trx 2>nul | findstr . >nul
    if %ERRORLEVEL% equ 0 (
        for /f %%f in ('dir /b test-results\*.trx') do echo   - test-results\%%f
    )
    
    echo.
    echo 覆盖率文件:
    dir /b test-results\coverage* 2>nul | findstr . >nul
    if %ERRORLEVEL% equ 0 (
        for /f %%f in ('dir /b test-results\coverage*') do echo   - test-results\%%f
    )
    
    echo.
    echo 报告目录:
    echo   - test-results\
)

exit /b 0

REM 清理测试文件函数
:cleanup_test_files
call :log_info "清理测试文件..."

cd /d "%PROJECT_DIR%"

REM 保留重要的测试结果，清理临时文件
if exist "test-results" (
    del /q test-results\*.tmp 2>nul
    del /q test-results\*.log 2>nul
)

call :log_success "清理完成"
exit /b 0

REM 只构建函数
:build_only
call :log_success "项目构建完成"
exit /b 0

REM 显示帮助信息函数
:show_help
echo 用法: %~nx0 [选项]
echo.
echo 选项:
echo   -h, --help          显示帮助信息
echo   -u, --unit          只运行单元测试
echo   -i, --integration   只运行集成测试
echo   -p, --performance   只运行性能测试
echo   -a, --all           运行所有测试（默认）
echo   -c, --coverage      生成覆盖率报告
echo   -b, --build         只构建项目
echo   --clean             清理测试文件
echo.
echo 示例:
echo   %~nx0                运行所有测试
echo   %~nx0 -u            只运行单元测试
echo   %~nx0 -i -c         运行集成测试并生成覆盖率报告
exit /b 0