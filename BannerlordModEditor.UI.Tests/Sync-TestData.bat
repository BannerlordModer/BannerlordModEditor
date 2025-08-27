@echo off
rem 同步测试数据从Common.Tests到UI.Tests
rem Windows批处理脚本版本

echo 开始同步测试数据...
set SOURCE_PATH=..\BannerlordModEditor.Common.Tests\TestData
set DEST_PATH=.\TestData

rem 检查源路径是否存在
if not exist "%SOURCE_PATH%" (
    echo 错误: 源路径不存在: %SOURCE_PATH%
    pause
    exit /b 1
)

rem 创建目标目录（如果不存在）
if not exist "%DEST_PATH%" (
    echo 创建目标目录: %DEST_PATH%
    mkdir "%DEST_PATH%"
)

rem 定义需要复制的文件列表
set FILES_TO_COPY=^
    attributes.xml^
    bone_body_types.xml^
    skills.xml^
    module_sounds.xml^
    crafting_pieces.xml^
    item_modifiers.xml

rem 复制文件
set files_copied=0
set files_skipped=0
set files_not_found=0

for %%f in (%FILES_TO_COPY%) do (
    set source_file=%SOURCE_PATH%\%%f
    set dest_file=%DEST_PATH%\%%f
    
    echo 处理文件: %%f
    
    if exist "!source_file!" (
        if exist "!dest_file!" (
            if "%~1"=="-force" (
                copy /Y "!source_file!" "!dest_file!" >nul
                echo   ✓ 已覆盖
                set /a files_copied+=1
            ) else (
                echo   跳过: 文件已存在 (使用 -force 参数强制覆盖)
                set /a files_skipped+=1
            )
        ) else (
            copy "!source_file!" "!dest_file!" >nul
            echo   ✓ 已复制
            set /a files_copied+=1
        )
    ) else (
        echo   警告: 源文件不存在: !source_file!
        set /a files_not_found+=1
    )
)

rem 显示摘要
echo.
echo 同步完成!
echo 已复制: %files_copied% 个文件
echo 已跳过: %files_skipped% 个文件
if %files_not_found% gtr 0 (
    echo 未找到: %files_not_found% 个文件
    echo 警告: 某些必需的测试文件不存在，请检查Common.Tests\TestData目录
)

rem 验证结果
echo.
echo 验证目标目录...
if exist "%DEST_PATH%\*.xml" (
    echo 目标目录中的XML文件:
    dir /b "%DEST_PATH%\*.xml"
) else (
    echo 目标目录中没有XML文件
)

pause