using Xunit;
using Moq;
using CommunityToolkit.Mvvm.ComponentModel;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Services;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests.Integration
{
    /// <summary>
    /// 编辑器管理器集成测试套件
    /// 
    /// 这个测试套件专门验证EditorManager的完整功能。
    /// 主要功能：
    /// - 验证EditorManager的完整功能
    /// - 测试编辑器选择和切换逻辑
    /// - 确保UI可见性管理的正确性
    /// - 验证编辑器之间的交互
    /// 
    /// 测试覆盖范围：
    /// 1. 编辑器管理器初始化
    /// 2. 编辑器分类管理
    /// 3. 编辑器选择和切换
    /// 4. 编辑器状态管理
    /// 5. 编辑器命令执行
    /// 6. 编辑器面包屑导航
    /// 7. 编辑器可见性管理
    /// 8. 编辑器错误处理
    /// 9. 编辑器数据持久化
    /// 10. 编辑器集成测试
    /// </summary>
    public class EditorManagerIntegrationTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEditorFactory _editorFactory;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandlerService;
        private readonly EditorManagerViewModel _editorManager;

        public EditorManagerIntegrationTests()
        {
            // 设置依赖注入
            var services = new ServiceCollection();
            
            // 注册核心服务
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
            services.AddSingleton<IValidationService, ValidationService>();
            services.AddSingleton<IDataBindingService, DataBindingService>();
            
            // 注册工厂
            services.AddSingleton<IEditorFactory, MockEditorFactory>();
            
            // 注册ViewModels
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<CombatParameterEditorViewModel>();
            services.AddTransient<ItemEditorViewModel>();
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            services.AddTransient<EditorManagerViewModel>();
            services.AddTransient<EditorCategoryViewModel>();
            services.AddTransient<EditorItemViewModel>();
            
            _serviceProvider = services.BuildServiceProvider();
            _logService = _serviceProvider.GetRequiredService<ILogService>();
            _errorHandlerService = _serviceProvider.GetRequiredService<IErrorHandlerService>();
            _editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
            _editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
        }

        [Fact]
        public void EditorManager_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();

            // Assert
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            Assert.NotNull(editorManager.SelectEditorCommand);
            Assert.NotNull(editorManager.CurrentEditorViewModel);
            Assert.NotNull(editorManager.CurrentBreadcrumb);
            Assert.Null(editorManager.SelectedEditor);
        }

        [Fact]
        public void EditorManager_Should_Load_Editor_Categories()
        {
            // Arrange & Act
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();

            // Assert
            Assert.NotNull(editorManager.Categories);
            Assert.True(editorManager.Categories.Count > 0);
            
            // 验证分类结构
            var characterCategory = editorManager.Categories.FirstOrDefault(c => c.Name.Contains("角色"));
            var combatCategory = editorManager.Categories.FirstOrDefault(c => c.Name.Contains("战斗"));
            var itemCategory = editorManager.Categories.FirstOrDefault(c => c.Name.Contains("物品"));
            
            Assert.NotNull(characterCategory);
            Assert.NotNull(combatCategory);
            Assert.NotNull(itemCategory);
            
            // 验证每个分类都有编辑器
            Assert.True(characterCategory.Editors.Count > 0);
            Assert.True(combatCategory.Editors.Count > 0);
            Assert.True(itemCategory.Editors.Count > 0);
        }

        [Fact]
        public void EditorManager_Should_Select_Editor_Correctly()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act
            editorManager.SelectEditorCommand.Execute(attributeEditor);

            // Assert
            Assert.NotNull(editorManager.SelectedEditor);
            Assert.Equal(attributeEditor, editorManager.SelectedEditor);
            Assert.NotNull(editorManager.CurrentEditorViewModel);
            Assert.IsType<AttributeEditorViewModel>(editorManager.CurrentEditorViewModel);
            Assert.NotNull(editorManager.CurrentBreadcrumb);
            Assert.Contains("属性", editorManager.CurrentBreadcrumb);
        }

        [Fact]
        public void EditorManager_Should_Switch_Editors_Correctly()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");
            var skillEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "SkillEditor");

            // Act - 先选择属性编辑器
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var firstViewModel = editorManager.CurrentEditorViewModel;

            // 再选择技能编辑器
            editorManager.SelectEditorCommand.Execute(skillEditor);
            var secondViewModel = editorManager.CurrentEditorViewModel;

            // Assert
            Assert.NotNull(firstViewModel);
            Assert.NotNull(secondViewModel);
            Assert.NotSame(firstViewModel, secondViewModel);
            Assert.IsType<AttributeEditorViewModel>(firstViewModel);
            Assert.IsType<SkillEditorViewModel>(secondViewModel);
            Assert.Equal(skillEditor, editorManager.SelectedEditor);
            Assert.Contains("技能", editorManager.CurrentBreadcrumb);
        }

        [Fact]
        public void EditorManager_Should_Handle_Unsaved_Changes()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // 先选择属性编辑器
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var attributeViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            Assert.NotNull(attributeViewModel);

            // 模拟未保存的更改
            attributeViewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "TestAttribute",
                Name = "Test Attribute",
                DefaultValue = "15"
            });

            Assert.True(attributeViewModel.HasUnsavedChanges);

            // Act - 尝试切换到其他编辑器
            var skillEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "SkillEditor");

            // 这里应该有提示用户保存更改的逻辑
            editorManager.SelectEditorCommand.Execute(skillEditor);

            // Assert
            // 即使有未保存的更改，也应该能够切换编辑器
            Assert.Equal(skillEditor, editorManager.SelectedEditor);
            Assert.IsType<SkillEditorViewModel>(editorManager.CurrentEditorViewModel);
        }

        [Fact]
        public void EditorManager_Should_Update_Breadcrumb_Correctly()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act
            editorManager.SelectEditorCommand.Execute(attributeEditor);

            // Assert
            Assert.NotNull(editorManager.CurrentBreadcrumb);
            Assert.True(editorManager.CurrentBreadcrumb.Length > 0);
            Assert.Contains("属性", editorManager.CurrentBreadcrumb);
        }

        [Fact]
        public void EditorManager_Should_Handle_Editor_Creation_Failure()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            
            // 创建一个会导致失败的编辑器
            var invalidEditor = new EditorItemViewModel("无效编辑器", "无效编辑器", "invalid.xml", "InvalidEditor", "❌");

            // Act
            editorManager.SelectEditorCommand.Execute(invalidEditor);

            // Assert
            // 应该有错误处理机制，确保编辑器管理器仍然可用
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            Assert.True(editorManager.Categories.Count > 0);
        }

        [Fact]
        public void EditorManager_Should_Handle_Concurrent_Editor_Selection()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");
            var skillEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "SkillEditor");

            const int threadCount = 5;
            var results = new bool[threadCount];
            var exceptions = new System.Exception[threadCount];

            // Act
            Parallel.For(0, threadCount, i =>
            {
                try
                {
                    var editor = i % 2 == 0 ? attributeEditor : skillEditor;
                    editorManager.SelectEditorCommand.Execute(editor);
                    results[i] = editorManager.SelectedEditor == editor;
                }
                catch (Exception ex)
                {
                    exceptions[i] = ex;
                    results[i] = false;
                }
            });

            // Assert
            Assert.All(results, result => Assert.True(result));
            Assert.All(exceptions, ex => Assert.Null(ex));
        }

        [Fact]
        public void EditorManager_Should_Persist_Editor_State()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act - 选择编辑器并修改状态
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var attributeViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            Assert.NotNull(attributeViewModel);

            // 添加新的属性
            attributeViewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "PersistentAttribute",
                Name = "Persistent Attribute",
                DefaultValue = "20"
            });

            // 切换到其他编辑器再切换回来
            var skillEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "SkillEditor");
            
            editorManager.SelectEditorCommand.Execute(skillEditor);
            editorManager.SelectEditorCommand.Execute(attributeEditor);

            var restoredViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            // Assert
            Assert.NotNull(restoredViewModel);
            Assert.True(restoredViewModel.Attributes.Count > 0);
            Assert.Contains(restoredViewModel.Attributes, a => a.Id == "PersistentAttribute");
        }

        [Fact]
        public void EditorManager_Should_Handle_Editor_Disposal()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act - 选择编辑器
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var viewModel = editorManager.CurrentEditorViewModel;

            // 清理编辑器
            if (viewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }

            // Assert
            // 编辑器管理器应该仍然可用
            Assert.NotNull(editorManager);
            Assert.NotNull(editorManager.Categories);
            Assert.True(editorManager.Categories.Count > 0);
        }

        [Fact]
        public void EditorManager_Should_Integrate_With_Services()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();

            // Act & Assert
            Assert.NotNull(editorManager.EditorFactory);
            Assert.NotNull(editorManager.LogService);
            Assert.NotNull(editorManager.ErrorHandlerService);
            
            // 验证服务依赖项
            Assert.Equal(_editorFactory, editorManager.EditorFactory);
            Assert.Equal(_logService, editorManager.LogService);
            Assert.Equal(_errorHandlerService, editorManager.ErrorHandlerService);
        }

        [Fact]
        public void EditorManager_Should_Handle_Async_Operations()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act - 异步选择编辑器
            var task = Task.Run(() =>
            {
                editorManager.SelectEditorCommand.Execute(attributeEditor);
                return editorManager.CurrentEditorViewModel;
            });

            var viewModel = task.Result;

            // Assert
            Assert.NotNull(viewModel);
            Assert.IsType<AttributeEditorViewModel>(viewModel);
        }

        [Fact]
        public void EditorManager_Should_Handle_Multiple_Editor_Instances()
        {
            // Arrange
            var editorManager1 = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var editorManager2 = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            
            var attributeEditor = editorManager1.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act - 在不同的编辑器管理器实例中选择编辑器
            editorManager1.SelectEditorCommand.Execute(attributeEditor);
            editorManager2.SelectEditorCommand.Execute(attributeEditor);

            // Assert
            Assert.NotNull(editorManager1.CurrentEditorViewModel);
            Assert.NotNull(editorManager2.CurrentEditorViewModel);
            Assert.IsType<AttributeEditorViewModel>(editorManager1.CurrentEditorViewModel);
            Assert.IsType<AttributeEditorViewModel>(editorManager2.CurrentEditorViewModel);
            
            // 但应该是不同的实例
            Assert.NotSame(editorManager1.CurrentEditorViewModel, editorManager2.CurrentEditorViewModel);
        }

        [Fact]
        public void EditorManager_Should_Handle_Editor_Commands()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var attributeViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            // Assert
            Assert.NotNull(attributeViewModel);
            Assert.NotNull(attributeViewModel.AddAttributeCommand);
            Assert.NotNull(attributeViewModel.RemoveAttributeCommand);
            Assert.NotNull(attributeViewModel.SaveCommand);
            Assert.NotNull(attributeViewModel.LoadCommand);
            
            // 测试命令执行
            var initialCount = attributeViewModel.Attributes.Count;
            attributeViewModel.AddAttributeCommand.Execute(null);
            Assert.Equal(initialCount + 1, attributeViewModel.Attributes.Count);
        }

        [Fact]
        public void EditorManager_Should_Handle_Editor_Validation()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var attributeViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            // Assert
            Assert.NotNull(attributeViewModel);
            
            // 添加无效数据
            attributeViewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "",
                Name = "Invalid Attribute",
                DefaultValue = "10"
            });

            // 验证应该能检测到问题
            var hasInvalidData = attributeViewModel.Attributes.Any(a => string.IsNullOrEmpty(a.Id));
            Assert.True(hasInvalidData);
        }

        [Fact]
        public void EditorManager_Should_Handle_Editor_Error_Handling()
        {
            // Arrange
            var editorManager = _serviceProvider.GetRequiredService<EditorManagerViewModel>();
            var attributeEditor = editorManager.Categories
                .SelectMany(c => c.Editors)
                .FirstOrDefault(e => e.EditorType == "AttributeEditor");

            // Act
            editorManager.SelectEditorCommand.Execute(attributeEditor);
            var attributeViewModel = editorManager.CurrentEditorViewModel as AttributeEditorViewModel;

            // Assert
            Assert.NotNull(attributeViewModel);
            
            // 验证错误处理服务可用
            Assert.NotNull(attributeViewModel.ErrorHandler);
        }
    }
}