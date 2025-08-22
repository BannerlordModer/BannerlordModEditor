using System;
using Terminal.Gui;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.TUI.Services;

namespace BannerlordModEditor.TUI.Views
{
    public class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        
        // UI 控件
        private RadioGroup _conversionDirectionGroup = null!;
        private Button _browseSourceButton = null!;
        private Button _browseTargetButton = null!;
        private Button _analyzeButton = null!;
        private Button _convertButton = null!;
        private Button _optionsButton = null!;
        private Button _clearButton = null!;
        private Button _exitButton = null!;
        private TextField _sourceFileField = null!;
        private TextField _targetFileField = null!;
        private Label _statusLabel = null!;
        private ListView _fileInfoListView = null!;
        private ListView _messageListView = null!;
        private ProgressBar _progressBar = null!;

        public MainWindow(MainViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            
            Title = "骑马与砍杀2模组编辑器 - Excel/XML转换工具";
            X = 0;
            Y = 0;
            Width = Dim.Fill();
            Height = Dim.Fill();

            InitializeComponents();
            BindToViewModel();
            SetupEventHandlers();
        }

        private void InitializeComponents()
        {
            // 转换方向选择
            var directionLabel = new Label("转换方向 (F1):")
            {
                X = 1,
                Y = 1
            };

            _conversionDirectionGroup = new RadioGroup(new NStack.ustring[] { "Excel → XML", "XML → Excel" })
            {
                X = 1,
                Y = 2,
                Width = 25,
                Height = 2,
                SelectedItem = (int)_viewModel.ConversionDirection
            };

            // 源文件选择
            var sourceFileLabel = new Label("源文件 (F2):")
            {
                X = 1,
                Y = 5
            };

            _sourceFileField = new TextField("")
            {
                X = 1,
                Y = 6,
                Width = Dim.Fill() - 20,
                Height = 1
            };

            _browseSourceButton = new Button("浏览...")
            {
                X = Pos.Right(_sourceFileField) + 1,
                Y = 6,
                Width = 12
            };

            _analyzeButton = new Button("分析 (F3)")
            {
                X = Pos.Right(_browseSourceButton) + 1,
                Y = 6,
                Width = 12
            };

            // 目标文件选择
            var targetFileLabel = new Label("目标文件 (F4):")
            {
                X = 1,
                Y = 8
            };

            _targetFileField = new TextField("")
            {
                X = 1,
                Y = 9,
                Width = Dim.Fill() - 20,
                Height = 1
            };

            _browseTargetButton = new Button("浏览...")
            {
                X = Pos.Right(_targetFileField) + 1,
                Y = 9,
                Width = 12
            };

            // 文件信息显示
            var fileInfoLabel = new Label("文件信息:")
            {
                X = 1,
                Y = 11
            };

            _fileInfoListView = new ListView(new List<string>())
            {
                X = 1,
                Y = 12,
                Width = Dim.Fill() - 1,
                Height = 6,
                Visible = false
            };

            // 按钮组
            var buttonFrame = new FrameView("操作")
            {
                X = 1,
                Y = 19,
                Width = Dim.Fill() - 1,
                Height = 3
            };

            _convertButton = new Button("转换 (F5)")
            {
                X = 1,
                Y = 0,
                Width = 12
            };

            _optionsButton = new Button("选项 (F6)")
            {
                X = Pos.Right(_convertButton) + 1,
                Y = 0,
                Width = 12
            };

            _clearButton = new Button("清空 (F7)")
            {
                X = Pos.Right(_optionsButton) + 1,
                Y = 0,
                Width = 12
            };

            _exitButton = new Button("退出 (F8)")
            {
                X = Pos.Right(_clearButton) + 1,
                Y = 0,
                Width = 12
            };

            buttonFrame.Add(_convertButton, _optionsButton, _clearButton, _exitButton);

            // 状态栏
            var statusFrame = new FrameView("状态")
            {
                X = 1,
                Y = 23,
                Width = Dim.Fill() - 1,
                Height = 3
            };

            _statusLabel = new Label("就绪")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 30,
                Height = 1
            };

            _progressBar = new ProgressBar()
            {
                X = Pos.Right(_statusLabel) + 1,
                Y = 1,
                Width = 25,
                Height = 1,
                Visible = false,
                Fraction = 0.0f
            };

            statusFrame.Add(_statusLabel, _progressBar);

            // 消息列表
            var messageFrame = new FrameView("消息")
            {
                X = 1,
                Y = 27,
                Width = Dim.Fill() - 1,
                Height = Dim.Fill() - 28
            };

            _messageListView = new ListView(new List<string>())
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 2,
                Height = Dim.Fill() - 2,
                Visible = false
            };

            messageFrame.Add(_messageListView);

            // 添加所有控件到窗口
            Add(
                directionLabel,
                _conversionDirectionGroup,
                sourceFileLabel,
                _sourceFileField,
                _browseSourceButton,
                _analyzeButton,
                targetFileLabel,
                _targetFileField,
                _browseTargetButton,
                fileInfoLabel,
                _fileInfoListView,
                buttonFrame,
                statusFrame,
                messageFrame
            );
        }

        private void BindToViewModel()
        {
            // 绑定转换方向
            _conversionDirectionGroup.SelectedItemChanged += (args) =>
            {
                _viewModel.ConversionDirection = (ConversionDirection)_conversionDirectionGroup.SelectedItem;
            };

            // 绑定文件路径
            _sourceFileField.Text = _viewModel.SourceFilePath;
            _targetFileField.Text = _viewModel.TargetFilePath;

            // 绑定状态消息
            _statusLabel.Text = _viewModel.StatusMessage;

            // 监听ViewModel属性变化
            _viewModel.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(MainViewModel.SourceFilePath):
                        _sourceFileField.Text = _viewModel.SourceFilePath;
                        break;
                    case nameof(MainViewModel.TargetFilePath):
                        _targetFileField.Text = _viewModel.TargetFilePath;
                        break;
                    case nameof(MainViewModel.StatusMessage):
                        _statusLabel.Text = _viewModel.StatusMessage;
                        AddMessage($"状态: {_viewModel.StatusMessage}");
                        break;
                    case nameof(MainViewModel.SourceFileInfo):
                        UpdateFileInfoDisplay();
                        break;
                    case nameof(MainViewModel.IsBusy):
                        UpdateControlStates();
                        break;
                }
            };
        }

        private void SetupEventHandlers()
        {
            // 按钮事件
            _browseSourceButton.Clicked += async () => { await _viewModel.BrowseSourceFileCommand.ExecuteAsync(); };
            _browseTargetButton.Clicked += async () => { await _viewModel.BrowseTargetFileCommand.ExecuteAsync(); };
            _analyzeButton.Clicked += async () => { await _viewModel.AnalyzeSourceFileCommand.ExecuteAsync(); };
            _convertButton.Clicked += async () => { await _viewModel.ConvertCommand.ExecuteAsync(); };
            _optionsButton.Clicked += () => _viewModel.ShowOptionsCommand.Execute();
            _clearButton.Clicked += () => _viewModel.ClearCommand.Execute();
            _exitButton.Clicked += () => _viewModel.ExitCommand.Execute();

            // 文本框变化事件
            _sourceFileField.TextChanged += (args) =>
            {
                _viewModel.SourceFilePath = _sourceFileField.Text.ToString();
            };

            _targetFileField.TextChanged += (args) =>
            {
                _viewModel.TargetFilePath = _targetFileField.Text.ToString();
            };

            // 键盘快捷键
            this.KeyDown += (e) =>
            {
                switch (e.KeyEvent.Key)
                {
                    case Key.F1:
                        // 切换转换方向
                        _conversionDirectionGroup.SelectedItem = _conversionDirectionGroup.SelectedItem == 0 ? 1 : 0;
                        e.Handled = true;
                        break;
                    case Key.F2:
                        _viewModel.BrowseSourceFileCommand.ExecuteAsync();
                        e.Handled = true;
                        break;
                    case Key.F3:
                        _viewModel.AnalyzeSourceFileCommand.ExecuteAsync();
                        e.Handled = true;
                        break;
                    case Key.F4:
                        _viewModel.BrowseTargetFileCommand.ExecuteAsync();
                        e.Handled = true;
                        break;
                    case Key.F5:
                        _viewModel.ConvertCommand.ExecuteAsync();
                        e.Handled = true;
                        break;
                    case Key.F6:
                        _viewModel.ShowOptionsCommand.Execute();
                        e.Handled = true;
                        break;
                    case Key.F7:
                        _viewModel.ClearCommand.Execute();
                        e.Handled = true;
                        break;
                    case Key.F8:
                    case Key.Esc:
                        _viewModel.ExitCommand.Execute();
                        e.Handled = true;
                        break;
                }
            };

            // 定期更新命令状态
            Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(100), (MainLoop loop) =>
            {
                _viewModel.ConvertCommand.NotifyCanExecuteChanged();
                _viewModel.AnalyzeSourceFileCommand.NotifyCanExecuteChanged();
                return true;
            });
        }

        private void UpdateFileInfoDisplay()
        {
            if (_viewModel.SourceFileInfo != null)
            {
                var fileInfo = _viewModel.SourceFileInfo;
                var infoLines = new List<string>
                {
                    $"格式类型: {fileInfo.FormatType}",
                    $"是否支持: {fileInfo.IsSupported}",
                    $"描述: {fileInfo.FormatDescription}"
                };

                if (fileInfo.ColumnNames.Count > 0)
                {
                    infoLines.Add($"列数: {fileInfo.ColumnNames.Count}");
                    infoLines.Add($"行数: {fileInfo.RowCount}");
                    
                    if (fileInfo.ColumnNames.Count <= 10)
                    {
                        infoLines.Add("列名: " + string.Join(", ", fileInfo.ColumnNames));
                    }
                    else
                    {
                        infoLines.Add("列名: " + string.Join(", ", fileInfo.ColumnNames.Take(10)) + "...");
                    }
                }

                _fileInfoListView.SetSource(infoLines);
                _fileInfoListView.Visible = true;
                
                AddMessage($"文件分析完成: {fileInfo.FormatDescription}");
            }
            else
            {
                _fileInfoListView.Visible = false;
            }
        }

        private void UpdateControlStates()
        {
            var isBusy = _viewModel.IsBusy;
            
            _sourceFileField.Enabled = !isBusy;
            _targetFileField.Enabled = !isBusy;
            _browseSourceButton.Enabled = !isBusy;
            _browseTargetButton.Enabled = !isBusy;
            _analyzeButton.Enabled = !isBusy;
            _convertButton.Enabled = !isBusy && _viewModel.ConvertCommand.CanExecute();
            _optionsButton.Enabled = !isBusy;
            _clearButton.Enabled = !isBusy;
            _exitButton.Enabled = !isBusy;
            _conversionDirectionGroup.Enabled = !isBusy;

            // 更新按钮文本和进度条
            if (isBusy)
            {
                _convertButton.Text = "转换中...";
                _progressBar.Visible = true;
                _progressBar.Fraction = 0.5f; // 模拟进度
            }
            else
            {
                _convertButton.Text = "转换 (F5)";
                _progressBar.Visible = false;
                _progressBar.Fraction = 0.0f;
            }
        }

        private void AddMessage(string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var fullMessage = $"[{timestamp}] {message}";
            
            var currentSource = new List<string>();
            if (_messageListView.Source != null)
            {
                foreach (var item in _messageListView.Source.ToList())
                {
                    currentSource.Add(item.ToString());
                }
            }
            currentSource.Add(fullMessage);
            
            // 保持最多100条消息
            if (currentSource.Count > 100)
            {
                currentSource = currentSource.Skip(currentSource.Count - 100).ToList();
            }
            
            _messageListView.SetSource(currentSource);
            _messageListView.Visible = true;
            _messageListView.MoveEnd();
        }

        public override void Redraw(Rect bounds)
        {
            base.Redraw(bounds);
            UpdateControlStates();
        }
    }
}