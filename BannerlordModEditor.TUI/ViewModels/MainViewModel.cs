using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Terminal.Gui;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.Models;

namespace BannerlordModEditor.TUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFormatConversionService _conversionService;
        private string _sourceFilePath = string.Empty;
        private string _targetFilePath = string.Empty;
        private string _statusMessage = "就绪";
        private bool _isBusy = false;
        private FileFormatInfo? _sourceFileInfo;
        private ConversionDirection _conversionDirection = ConversionDirection.ExcelToXml;
        private ConversionOptions _conversionOptions = new ConversionOptions();
        private XmlTypeInfo? _xmlTypeInfo;

        public string SourceFilePath
        {
            get => _sourceFilePath;
            set => SetProperty(ref _sourceFilePath, value);
        }

        public string TargetFilePath
        {
            get => _targetFilePath;
            set => SetProperty(ref _targetFilePath, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public FileFormatInfo? SourceFileInfo
        {
            get => _sourceFileInfo;
            set => SetProperty(ref _sourceFileInfo, value);
        }

        public ConversionDirection ConversionDirection
        {
            get => _conversionDirection;
            set => SetProperty(ref _conversionDirection, value);
        }

        public ConversionOptions ConversionOptions
        {
            get => _conversionOptions;
            set => SetProperty(ref _conversionOptions, value);
        }

        public XmlTypeInfo? XmlTypeInfo
        {
            get => _xmlTypeInfo;
            set => SetProperty(ref _xmlTypeInfo, value);
        }

        public Command BrowseSourceFileCommand { get; }
        public Command BrowseTargetFileCommand { get; }
        public Command ConvertCommand { get; }
        public Command AnalyzeSourceFileCommand { get; }
        public Command ShowOptionsCommand { get; }
        public Command ShowXmlTypeInfoCommand { get; }
        public Command ClearCommand { get; }
        public Command ExitCommand { get; }

        public MainViewModel(IFormatConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));

            BrowseSourceFileCommand = new Command(async () => await BrowseSourceFileAsync());
            BrowseTargetFileCommand = new Command(async () => await BrowseTargetFileAsync());
            ConvertCommand = new Command(async () => await ConvertAsync(), () => CanConvert());
            AnalyzeSourceFileCommand = new Command(async () => await AnalyzeSourceFileAsync(), () => !string.IsNullOrEmpty(SourceFilePath));
            ShowOptionsCommand = new Command(ShowOptions);
            ShowXmlTypeInfoCommand = new Command(ShowXmlTypeInfo, () => XmlTypeInfo != null);
            ClearCommand = new Command(Clear);
            ExitCommand = new Command(() => Application.RequestStop());

            // 根据转换方向自动设置默认文件扩展名
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ConversionDirection))
                {
                    UpdateDefaultFileExtensions();
                }
                else if (e.PropertyName == nameof(SourceFilePath))
                {
                    AnalyzeSourceFileCommand.NotifyCanExecuteChanged();
                }
            };
        }

        private async Task BrowseSourceFileAsync()
        {
            try
            {
                if (IsBusy)
                {
                    StatusMessage = "系统繁忙，请稍后再试";
                    return;
                }

                IsBusy = true;
                StatusMessage = "正在打开文件选择对话框...";

                var dialog = new OpenDialog("选择源文件", "请选择要转换的文件")
                {
                    AllowsMultipleSelection = false,
                    DirectoryPath = Environment.CurrentDirectory
                };

                // 根据转换方向设置文件过滤器
                if (ConversionDirection == ConversionDirection.ExcelToXml)
                {
                    dialog.AllowedFileTypes = new[] { ".xlsx", ".xls" };
                }
                else
                {
                    dialog.AllowedFileTypes = new[] { ".xml" };
                }

                Application.Run(dialog);

                if (dialog.FilePath != null && !dialog.Canceled)
                {
                    SourceFilePath = dialog.FilePath.ToString();
                    StatusMessage = "已选择源文件，正在分析...";
                    await AnalyzeSourceFileAsync();
                }
                else
                {
                    StatusMessage = "已取消文件选择";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                StatusMessage = $"文件访问权限不足: {ex.Message}";
            }
            catch (IOException ex)
            {
                StatusMessage = $"文件I/O错误: {ex.Message}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"选择文件失败: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task BrowseTargetFileAsync()
        {
            try
            {
                if (IsBusy)
                {
                    StatusMessage = "系统繁忙，请稍后再试";
                    return;
                }

                IsBusy = true;
                StatusMessage = "正在打开保存对话框...";

                var dialog = new SaveDialog("保存目标文件", "请选择保存位置")
                {
                    DirectoryPath = Path.GetDirectoryName(!string.IsNullOrEmpty(TargetFilePath) ? TargetFilePath : SourceFilePath) ?? Environment.CurrentDirectory
                };

                // 设置文件过滤
                if (ConversionDirection == ConversionDirection.ExcelToXml)
                {
                    dialog.AllowedFileTypes = new[] { ".xml" };
                }
                else
                {
                    dialog.AllowedFileTypes = new[] { ".xlsx" };
                }

                Application.Run(dialog);

                if (dialog.FilePath != null && !dialog.Canceled)
                {
                    TargetFilePath = dialog.FilePath.ToString();
                    StatusMessage = "已选择保存位置";
                }
                else
                {
                    StatusMessage = "已取消保存位置选择";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                StatusMessage = $"文件访问权限不足: {ex.Message}";
            }
            catch (IOException ex)
            {
                StatusMessage = $"文件I/O错误: {ex.Message}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"选择保存位置失败: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ConvertAsync()
        {
            if (!CanConvert()) return;

            IsBusy = true;
            StatusMessage = "正在转换...";

            try
            {
                // 验证输入文件是否存在
                if (!File.Exists(SourceFilePath))
                {
                    StatusMessage = $"源文件不存在: {SourceFilePath}";
                    return;
                }

                // 检查目标目录是否可写
                var targetDir = Path.GetDirectoryName(TargetFilePath);
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                {
                    try
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    catch (Exception ex)
                    {
                        StatusMessage = $"无法创建目标目录: {ex.Message}";
                        return;
                    }
                }

                ConversionResult result;

                if (ConversionDirection == ConversionDirection.ExcelToXml)
                {
                    result = await _conversionService.ExcelToXmlAsync(SourceFilePath, TargetFilePath, ConversionOptions);
                }
                else
                {
                    result = await _conversionService.XmlToExcelAsync(SourceFilePath, TargetFilePath, ConversionOptions);
                }

                if (result.Success)
                {
                    StatusMessage = $"转换成功! {result.Message}";
                    ShowConversionResult(result);
                }
                else
                {
                    StatusMessage = $"转换失败: {result.Message}";
                    ShowConversionErrors(result);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                StatusMessage = $"文件访问权限不足: {ex.Message}";
            }
            catch (IOException ex)
            {
                StatusMessage = $"文件I/O错误: {ex.Message}";
            }
            catch (System.Xml.XmlException ex)
            {
                StatusMessage = $"XML处理错误: {ex.Message}";
            }
            catch (OutOfMemoryException ex)
            {
                StatusMessage = $"内存不足，文件可能过大: {ex.Message}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"转换过程中发生错误: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AnalyzeSourceFileAsync()
        {
            if (string.IsNullOrEmpty(SourceFilePath)) return;

            StatusMessage = "正在分析文件...";
            IsBusy = true;

            try
            {
                // 验证文件是否存在
                if (!File.Exists(SourceFilePath))
                {
                    StatusMessage = $"文件不存在: {SourceFilePath}";
                    return;
                }

                // 验证文件扩展名
                var extension = Path.GetExtension(SourceFilePath).ToLowerInvariant();
                if (extension != ".xlsx" && extension != ".xls" && extension != ".xml")
                {
                    StatusMessage = $"不支持的文件格式: {extension}";
                    return;
                }

                SourceFileInfo = await _conversionService.DetectFileFormatAsync(SourceFilePath);
                
                if (SourceFileInfo?.IsSupported == true)
                {
                    StatusMessage = $"文件格式: {SourceFileInfo.FormatDescription}";
                    
                    // 如果是XML文件，检测XML类型
                    if (SourceFileInfo.FormatType == FileFormatType.Xml)
                    {
                        try
                        {
                            XmlTypeInfo = await _conversionService.DetectXmlTypeAsync(SourceFilePath);
                            if (XmlTypeInfo?.IsSupported == true)
                            {
                                var messageParts = new List<string>
                                {
                                    $"XML类型: {XmlTypeInfo.DisplayName} ({XmlTypeInfo.XmlType})"
                                };
                                
                                if (XmlTypeInfo.IsAdapted)
                                {
                                    messageParts.Add("已适配");
                                }
                                else
                                {
                                    messageParts.Add("未适配");
                                }
                                
                                if (XmlTypeInfo.EstimatedRecordCount.HasValue)
                                {
                                    messageParts.Add($"预计记录数: {XmlTypeInfo.EstimatedRecordCount}");
                                }
                                
                                if (XmlTypeInfo.FileSize > 0)
                                {
                                    var sizeInKB = XmlTypeInfo.FileSize / 1024.0;
                                    var sizeDisplay = sizeInKB > 1024 
                                        ? $"{sizeInKB / 1024:F1}MB" 
                                        : $"{sizeInKB:F1}KB";
                                    messageParts.Add($"文件大小: {sizeDisplay}");
                                }
                                
                                if (XmlTypeInfo.SupportedOperations.Count > 0)
                                {
                                    var operations = string.Join(", ", XmlTypeInfo.SupportedOperations);
                                    messageParts.Add($"支持操作: {operations}");
                                }
                                
                                StatusMessage = string.Join(", ", messageParts);
                            }
                            else
                            {
                                StatusMessage = "XML文件格式不支持";
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusMessage += $" (XML类型检测失败: {ex.Message})";
                        }
                    }
                    
                    // 自动设置目标文件路径
                    if (string.IsNullOrEmpty(TargetFilePath))
                    {
                        try
                        {
                            var targetExt = ConversionDirection == ConversionDirection.ExcelToXml ? ".xml" : ".xlsx";
                            TargetFilePath = Path.ChangeExtension(SourceFilePath, targetExt);
                        }
                        catch (Exception ex)
                        {
                            StatusMessage = $"设置目标文件路径失败: {ex.Message}";
                            return;
                        }
                    }

                    // 自动切换转换方向
                    if (SourceFileInfo.FormatType == FileFormatType.Excel && ConversionDirection != ConversionDirection.ExcelToXml)
                    {
                        ConversionDirection = ConversionDirection.ExcelToXml;
                    }
                    else if (SourceFileInfo.FormatType == FileFormatType.Xml && ConversionDirection != ConversionDirection.XmlToExcel)
                    {
                        ConversionDirection = ConversionDirection.XmlToExcel;
                    }
                }
                else
                {
                    StatusMessage = $"不支持的文件格式: {SourceFileInfo?.FormatDescription}";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                StatusMessage = $"文件访问权限不足: {ex.Message}";
            }
            catch (IOException ex)
            {
                StatusMessage = $"文件I/O错误: {ex.Message}";
            }
            catch (System.Xml.XmlException ex)
            {
                StatusMessage = $"XML处理错误: {ex.Message}";
            }
            catch (OutOfMemoryException ex)
            {
                StatusMessage = $"内存不足，文件可能过大: {ex.Message}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"文件分析失败: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowOptions()
        {
            var optionsDialog = new OptionsDialog(ConversionOptions);
            Application.Run(optionsDialog);

            if (!optionsDialog.Canceled)
            {
                ConversionOptions = optionsDialog.GetOptions();
            }
        }

        private void ShowXmlTypeInfo()
        {
            if (XmlTypeInfo == null) return;

            var message = new List<string>
            {
                $"XML类型详细信息",
                "",
                $"类型名称: {XmlTypeInfo.XmlType}",
                $"显示名称: {XmlTypeInfo.DisplayName}",
                $"模型类型: {XmlTypeInfo.ModelType}",
                $"命名空间: {XmlTypeInfo.Namespace}",
                $"适配状态: {(XmlTypeInfo.IsAdapted ? "已适配" : "未适配")}",
                $"支持状态: {(XmlTypeInfo.IsSupported ? "支持" : "不支持")}"
            };

            if (XmlTypeInfo.EstimatedRecordCount.HasValue)
            {
                message.Add($"预计记录数: {XmlTypeInfo.EstimatedRecordCount}");
            }

            if (XmlTypeInfo.FileSize > 0)
            {
                var sizeInKB = XmlTypeInfo.FileSize / 1024.0;
                var sizeDisplay = sizeInKB > 1024 
                    ? $"{sizeInKB / 1024:F1}MB" 
                    : $"{sizeInKB:F1}KB";
                message.Add($"文件大小: {sizeDisplay}");
            }

            if (XmlTypeInfo.LastModified != default)
            {
                message.Add($"最后修改: {XmlTypeInfo.LastModified:yyyy-MM-dd HH:mm:ss}");
            }

            if (XmlTypeInfo.SupportedOperations.Count > 0)
            {
                message.Add($"支持的操作: {string.Join(", ", XmlTypeInfo.SupportedOperations)}");
            }

            message.Add("");
            message.Add("按任意键关闭...");

            var dialog = new Dialog()
            {
                Title = "XML类型信息",
                Width = 60,
                Height = message.Count + 2
            };

            var text = new TextView()
            {
                X = 1,
                Y = 1,
                Width = 58,
                Height = message.Count,
                Text = string.Join("\n", message),
                ReadOnly = true,
                WordWrap = true
            };

            dialog.Add(text);
            Application.Run(dialog);
        }

        private void Clear()
        {
            SourceFilePath = string.Empty;
            TargetFilePath = string.Empty;
            SourceFileInfo = null;
            XmlTypeInfo = null;
            StatusMessage = "就绪";
        }

        private bool CanConvert()
        {
            return !IsBusy && 
                   !string.IsNullOrEmpty(SourceFilePath) && 
                   !string.IsNullOrEmpty(TargetFilePath) &&
                   File.Exists(SourceFilePath) &&
                   SourceFileInfo?.IsSupported == true;
        }

        private void UpdateDefaultFileExtensions()
        {
            if (!string.IsNullOrEmpty(SourceFilePath))
            {
                var targetExt = ConversionDirection == ConversionDirection.ExcelToXml ? ".xml" : ".xlsx";
                if (string.IsNullOrEmpty(TargetFilePath) || Path.GetExtension(TargetFilePath) != targetExt)
                {
                    TargetFilePath = Path.ChangeExtension(SourceFilePath, targetExt);
                }
            }
        }

        private void ShowConversionResult(ConversionResult result)
        {
            var message = $"转换成功!\n\n" +
                         $"处理记录数: {result.RecordsProcessed}\n" +
                         $"耗时: {result.Duration.TotalSeconds:F2} 秒\n" +
                         $"输出文件: {result.OutputPath}";

            if (result.Warnings.Count > 0)
            {
                message += $"\n\n警告 ({result.Warnings.Count} 个):";
                foreach (var warning in result.Warnings.Take(5))
                {
                    message += $"\n- {warning}";
                }
                if (result.Warnings.Count > 5)
                {
                    message += $"\n... 还有 {result.Warnings.Count - 5} 个警告";
                }
            }

            MessageBox.Query("转换完成", message, "确定");
        }

        private void ShowConversionErrors(ConversionResult result)
        {
            var message = $"转换失败!\n\n原因: {result.Message}";

            if (result.Errors.Count > 0)
            {
                message += $"\n\n错误 ({result.Errors.Count} 个):";
                foreach (var error in result.Errors.Take(5))
                {
                    message += $"\n- {error}";
                }
                if (result.Errors.Count > 5)
                {
                    message += $"\n... 还有 {result.Errors.Count - 5} 个错误";
                }
            }

            MessageBox.ErrorQuery("转换失败", message, "确定");
        }
    }

    public class OptionsDialog : Dialog
    {
        private readonly ConversionOptions _options;
        private readonly CheckBox _includeSchemaValidation;
        private readonly CheckBox _preserveFormatting;
        private readonly CheckBox _createBackup;
        private readonly TextField _worksheetName;
        private readonly TextField _rootElementName;
        private readonly TextField _rowElementName;
        private readonly CheckBox _flattenNestedElements;
        private readonly TextField _nestedElementSeparator;

        public OptionsDialog(ConversionOptions options)
        {
            _options = options;

            Title = "转换选项";
            Width = Dim.Percent(80);
            Height = Dim.Percent(80);

            // 创建控件
            _includeSchemaValidation = new CheckBox("包含架构验证")
            {
                X = 1,
                Y = 1,
                Checked = options.IncludeSchemaValidation
            };

            _preserveFormatting = new CheckBox("保留格式")
            {
                X = 1,
                Y = 2,
                Checked = options.PreserveFormatting
            };

            _createBackup = new CheckBox("创建备份")
            {
                X = 1,
                Y = 3,
                Checked = options.CreateBackup
            };

            var worksheetNameLabel = new Label("工作表名称:")
            {
                X = 1,
                Y = 5
            };

            _worksheetName = new TextField("")
            {
                X = Pos.Right(worksheetNameLabel) + 1,
                Y = 5,
                Width = Dim.Fill() - 1,
                Text = options.WorksheetName ?? ""
            };

            var rootElementNameLabel = new Label("根元素名称:")
            {
                X = 1,
                Y = 7
            };

            _rootElementName = new TextField("")
            {
                X = Pos.Right(rootElementNameLabel) + 1,
                Y = 7,
                Width = Dim.Fill() - 1,
                Text = options.RootElementName ?? ""
            };

            var rowElementNameLabel = new Label("行元素名称:")
            {
                X = 1,
                Y = 9
            };

            _rowElementName = new TextField("")
            {
                X = Pos.Right(rowElementNameLabel) + 1,
                Y = 9,
                Width = Dim.Fill() - 1,
                Text = options.RowElementName ?? ""
            };

            _flattenNestedElements = new CheckBox("扁平化嵌套元素")
            {
                X = 1,
                Y = 11,
                Checked = options.FlattenNestedElements
            };

            var nestedSeparatorLabel = new Label("嵌套元素分隔符:")
            {
                X = 1,
                Y = 13
            };

            _nestedElementSeparator = new TextField("")
            {
                X = Pos.Right(nestedSeparatorLabel) + 1,
                Y = 13,
                Width = 10,
                Text = options.NestedElementSeparator ?? "_"
            };

            // 按钮
            var okButton = new Button("确定")
            {
                X = Pos.Center() - 10,
                Y = Pos.Bottom(this) - 3
            };
            okButton.Clicked += () => { Application.RequestStop(); };

            var cancelButton = new Button("取消")
            {
                X = Pos.Center() + 2,
                Y = Pos.Bottom(this) - 3
            };
            cancelButton.Clicked += () => { Canceled = true; Application.RequestStop(); };

            // 添加控件到对话框
            Add(
                _includeSchemaValidation,
                _preserveFormatting,
                _createBackup,
                worksheetNameLabel,
                _worksheetName,
                rootElementNameLabel,
                _rootElementName,
                rowElementNameLabel,
                _rowElementName,
                _flattenNestedElements,
                nestedSeparatorLabel,
                _nestedElementSeparator,
                okButton,
                cancelButton
            );
        }

        public ConversionOptions GetOptions()
        {
            return new ConversionOptions
            {
                IncludeSchemaValidation = _includeSchemaValidation.Checked == true,
                PreserveFormatting = _preserveFormatting.Checked == true,
                CreateBackup = _createBackup.Checked == true,
                WorksheetName = string.IsNullOrEmpty(_worksheetName.Text.ToString()) ? null : _worksheetName.Text.ToString(),
                RootElementName = string.IsNullOrEmpty(_rootElementName.Text.ToString()) ? null : _rootElementName.Text.ToString(),
                RowElementName = string.IsNullOrEmpty(_rowElementName.Text.ToString()) ? null : _rowElementName.Text.ToString(),
                FlattenNestedElements = _flattenNestedElements.Checked == true,
                NestedElementSeparator = string.IsNullOrEmpty(_nestedElementSeparator.Text.ToString()) ? "_" : _nestedElementSeparator.Text.ToString()
            };
        }

        public bool Canceled { get; private set; }
    }
}