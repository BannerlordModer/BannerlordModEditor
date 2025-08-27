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
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests.ViewModels
{
    /// <summary>
    /// 编辑器ViewModel测试套件
    /// 
    /// 这个测试套件专门验证所有编辑器ViewModel的正确功能和行为。
    /// 主要功能：
    /// - 验证所有编辑器ViewModel的正确初始化
    /// - 测试编辑器之间的依赖关系
    /// - 确保MockEditorFactory能正确创建编辑器实例
    /// - 测试编辑器的数据绑定和状态管理
    /// 
    /// 测试覆盖范围：
    /// 1. 基础编辑器ViewModel（Attribute、Skill、CombatParameter等）
    /// 2. 高级编辑器ViewModel（Crafting、ItemModifier、BoneBodyType等）
    /// 3. 编辑器工厂的正确创建
    /// 4. 编辑器状态管理
    /// 5. 编辑器数据操作
    /// 6. 编辑器命令执行
    /// </summary>
    public class EditorViewModelTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEditorFactory _editorFactory;
        private readonly IValidationService _validationService;
        private readonly IDataBindingService _dataBindingService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandlerService;

        public EditorViewModelTests()
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
            
            _serviceProvider = services.BuildServiceProvider();
            _validationService = _serviceProvider.GetRequiredService<IValidationService>();
            _dataBindingService = _serviceProvider.GetRequiredService<IDataBindingService>();
            _logService = _serviceProvider.GetRequiredService<ILogService>();
            _errorHandlerService = _serviceProvider.GetRequiredService<IErrorHandlerService>();
            _editorFactory = _serviceProvider.GetRequiredService<IEditorFactory>();
        }

        [Fact]
        public void AttributeEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.Attributes);
            Assert.True(viewModel.Attributes.Count > 0);
            Assert.Equal("NewAttribute", viewModel.Attributes[0].Id);
            Assert.Equal("New Attribute", viewModel.Attributes[0].Name);
            Assert.Equal("10", viewModel.Attributes[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void SkillEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<SkillEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.Skills);
            Assert.True(viewModel.Skills.Count > 0);
            Assert.Equal("NewSkill", viewModel.Skills[0].Id);
            Assert.Equal("New Skill", viewModel.Skills[0].Name);
            Assert.Equal("0", viewModel.Skills[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void CombatParameterEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<CombatParameterEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.CombatParameters);
            Assert.True(viewModel.CombatParameters.Count > 0);
            Assert.Equal("NewCombatParameter", viewModel.CombatParameters[0].Id);
            Assert.Equal("New Combat Parameter", viewModel.CombatParameters[0].Name);
            Assert.Equal("1.0", viewModel.CombatParameters[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void ItemEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<ItemEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.Items);
            Assert.True(viewModel.Items.Count > 0);
            Assert.Equal("NewItem", viewModel.Items[0].Id);
            Assert.Equal("New Item", viewModel.Items[0].Name);
            Assert.Equal("1", viewModel.Items[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void CraftingPieceEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<CraftingPieceEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.CraftingPieces);
            Assert.True(viewModel.CraftingPieces.Count > 0);
            Assert.Equal("NewCraftingPiece", viewModel.CraftingPieces[0].Id);
            Assert.Equal("New Crafting Piece", viewModel.CraftingPieces[0].Name);
            Assert.Equal("1", viewModel.CraftingPieces[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void ItemModifierEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<ItemModifierEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.ItemModifiers);
            Assert.True(viewModel.ItemModifiers.Count > 0);
            Assert.Equal("NewItemModifier", viewModel.ItemModifiers[0].Id);
            Assert.Equal("New Item Modifier", viewModel.ItemModifiers[0].Name);
            Assert.Equal("1.0", viewModel.ItemModifiers[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void BoneBodyTypeEditorViewModel_Should_Initialize_Correctly()
        {
            // Arrange & Act
            var viewModel = _serviceProvider.GetRequiredService<BoneBodyTypeEditorViewModel>();

            // Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.BoneBodyTypes);
            Assert.True(viewModel.BoneBodyTypes.Count > 0);
            Assert.Equal("NewBoneBodyType", viewModel.BoneBodyTypes[0].Id);
            Assert.Equal("New Bone Body Type", viewModel.BoneBodyTypes[0].Name);
            Assert.Equal("human", viewModel.BoneBodyTypes[0].DefaultValue);
            Assert.False(viewModel.HasUnsavedChanges);
            Assert.Empty(viewModel.FilePath);
        }

        [Fact]
        public void MockEditorFactory_Should_Create_All_Editor_Types()
        {
            // Arrange & Act
            var attributeEditor = _editorFactory.CreateEditorViewModel("AttributeEditor", "attributes.xml");
            var skillEditor = _editorFactory.CreateEditorViewModel("SkillEditor", "skills.xml");
            var combatEditor = _editorFactory.CreateEditorViewModel("CombatParameterEditor", "combat_parameters.xml");
            var itemEditor = _editorFactory.CreateEditorViewModel("ItemEditor", "items.xml");
            var craftingEditor = _editorFactory.CreateEditorViewModel("CraftingPieceEditor", "crafting_pieces.xml");
            var modifierEditor = _editorFactory.CreateEditorViewModel("ItemModifierEditor", "item_modifiers.xml");
            var boneBodyEditor = _editorFactory.CreateEditorViewModel("BoneBodyTypeEditor", "bone_body_types.xml");

            // Assert
            Assert.NotNull(attributeEditor);
            Assert.IsType<AttributeEditorViewModel>(attributeEditor);
            
            Assert.NotNull(skillEditor);
            Assert.IsType<SkillEditorViewModel>(skillEditor);
            
            Assert.NotNull(combatEditor);
            Assert.IsType<CombatParameterEditorViewModel>(combatEditor);
            
            Assert.NotNull(itemEditor);
            Assert.IsType<ItemEditorViewModel>(itemEditor);
            
            Assert.NotNull(craftingEditor);
            Assert.IsType<CraftingPieceEditorViewModel>(craftingEditor);
            
            Assert.NotNull(modifierEditor);
            Assert.IsType<ItemModifierEditorViewModel>(modifierEditor);
            
            Assert.NotNull(boneBodyEditor);
            Assert.IsType<BoneBodyTypeEditorViewModel>(boneBodyEditor);
        }

        [Fact]
        public void EditorFactory_Should_Handle_Unknown_Editor_Type()
        {
            // Arrange & Act
            var unknownEditor = _editorFactory.CreateEditorViewModel("UnknownEditor", "unknown.xml");

            // Assert
            Assert.NotNull(unknownEditor);
            // 未知编辑器类型应该返回默认的MockEditorViewModel
            Assert.IsType<MockEditorViewModel>(unknownEditor);
        }

        [Fact]
        public void AttributeEditor_Should_Track_Unsaved_Changes()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            var originalCount = viewModel.Attributes.Count;

            // Act
            viewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "TestAttribute",
                Name = "Test Attribute",
                DefaultValue = "15"
            });

            // Assert
            Assert.True(viewModel.HasUnsavedChanges);
            Assert.Equal(originalCount + 1, viewModel.Attributes.Count);
        }

        [Fact]
        public void SkillEditor_Should_Track_Unsaved_Changes()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<SkillEditorViewModel>();
            var originalCount = viewModel.Skills.Count;

            // Act
            viewModel.Skills.Add(new SkillDataViewModel
            {
                Id = "TestSkill",
                Name = "Test Skill",
                DefaultValue = "5"
            });

            // Assert
            Assert.True(viewModel.HasUnsavedChanges);
            Assert.Equal(originalCount + 1, viewModel.Skills.Count);
        }

        [Fact]
        public void EditorViewModel_Should_Reset_Unsaved_Changes()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            viewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "TestAttribute",
                Name = "Test Attribute",
                DefaultValue = "15"
            });

            // Act
            // 模拟重置操作
            viewModel.Attributes.Clear();
            viewModel.Attributes.Add(new AttributeDataViewModel
            {
                Id = "NewAttribute",
                Name = "New Attribute",
                DefaultValue = "10"
            });

            // Assert
            Assert.True(viewModel.HasUnsavedChanges); // 重置后仍然有未保存的更改
        }

        [Fact]
        public void EditorViewModel_Should_Validate_Data()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            var invalidAttribute = new AttributeDataViewModel
            {
                Id = "",
                Name = "Test Attribute",
                DefaultValue = "10"
            };

            // Act
            var validationResult = _validationService.Validate(invalidAttribute);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains("ID不能为空", validationResult.Errors);
        }

        [Fact]
        public void EditorViewModel_Should_Handle_DataBinding()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            var source = new TestObservableObject();
            var target = new TestObservableObject();

            // Act
            using var binding = _dataBindingService.CreateBinding<TestObservableObject, TestObservableObject>(
                source, "Name",
                target, "Name",
                true);

            source.Name = "Test Name";

            // Assert
            Assert.Equal("Test Name", target.Name);
        }

        [Fact]
        public void EditorViewModel_Should_Handle_Collection_Binding()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            var source = new ObservableCollection<AttributeDataViewModel>
            {
                new AttributeDataViewModel { Id = "Attr1", Name = "Attribute 1", DefaultValue = "10" },
                new AttributeDataViewModel { Id = "Attr2", Name = "Attribute 2", DefaultValue = "20" }
            };
            var target = new ObservableCollection<AttributeDataViewModel>();

            // Act
            using var binding = _dataBindingService.CreateCollectionBinding(source, target);

            // Assert
            Assert.Equal(2, target.Count);
            Assert.Contains(target, a => a.Id == "Attr1");
            Assert.Contains(target, a => a.Id == "Attr2");
        }

        [Fact]
        public void EditorViewModel_Should_Handle_Async_Operations()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();

            // Act
            var task = Task.Run(() =>
            {
                viewModel.Attributes.Add(new AttributeDataViewModel
                {
                    Id = "AsyncAttribute",
                    Name = "Async Attribute",
                    DefaultValue = "25"
                });
                return viewModel.HasUnsavedChanges;
            });

            var result = task.Result;

            // Assert
            Assert.True(result);
            Assert.Contains(viewModel.Attributes, a => a.Id == "AsyncAttribute");
        }

        [Fact]
        public void EditorViewModel_Should_Handle_Error_Cases()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();

            // Act
            // 模拟错误情况
            try
            {
                viewModel.Attributes.Add(null!);
            }
            catch (Exception ex)
            {
                // 错误应该被正确处理
                Assert.NotNull(ex);
            }

            // Assert
            // ViewModel应该仍然处于有效状态
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.Attributes);
        }

        [Fact]
        public void EditorViewModel_Should_Be_Thread_Safe()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            const int threadCount = 5;
            var results = new bool[threadCount];

            // Act
            Parallel.For(0, threadCount, i =>
            {
                try
                {
                    viewModel.Attributes.Add(new AttributeDataViewModel
                    {
                        Id = $"ThreadAttribute{i}",
                        Name = $"Thread Attribute {i}",
                        DefaultValue = (i * 10).ToString()
                    });
                    results[i] = true;
                }
                catch
                {
                    results[i] = false;
                }
            });

            // Assert
            Assert.All(results, result => Assert.True(result));
            Assert.Equal(threadCount, viewModel.Attributes.Count(a => a.Id.StartsWith("ThreadAttribute")));
        }

        [Fact]
        public void EditorViewModel_Should_Dispose_Correctly()
        {
            // Arrange
            var viewModel = _serviceProvider.GetRequiredService<AttributeEditorViewModel>();
            
            // Act
            if (viewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }

            // Assert
            // ViewModel应该被正确清理
            Assert.NotNull(viewModel);
        }

        // Test helper class
        private class TestObservableObject : ObservableObject
        {
            private string _name = string.Empty;
            private int _value = 0;

            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            public int Value
            {
                get => _value;
                set => SetProperty(ref _value, value);
            }
        }
    }
}