using System;
using System.IO;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BannerlordModEditor.UI.Tests.EditorFactoryTests
{
    /// <summary>
    /// 测试编辑器工厂的功能
    /// </summary>
    public class EditorFactoryTests
    {
        [Fact]
        public void EditorFactory_ShouldCreateAllRegisteredEditors()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // 注册编辑器工厂
            services.AddSingleton<IEditorFactory, EditorFactory>();
            
            // 注册所有编辑器ViewModel
            services.AddTransient<AttributeEditorViewModel>();
            services.AddTransient<SkillEditorViewModel>();
            services.AddTransient<BoneBodyTypeEditorViewModel>();
            services.AddTransient<CraftingPieceEditorViewModel>();
            services.AddTransient<ItemModifierEditorViewModel>();
            
            // 注册Common层服务
            services.AddTransient<IFileDiscoveryService, FileDiscoveryService>();
            
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetRequiredService<IEditorFactory>();
            
            // Act & Assert
            Assert.NotNull(factory);
            
            // 测试创建各个编辑器
            var attributeEditor = factory.CreateEditorViewModel("AttributeEditor", "spattributes.xml");
            Assert.NotNull(attributeEditor);
            Assert.IsType<AttributeEditorViewModel>(attributeEditor);
            
            var skillEditor = factory.CreateEditorViewModel("SkillEditor", "skills.xml");
            Assert.NotNull(skillEditor);
            Assert.IsType<SkillEditorViewModel>(skillEditor);
            
            var boneBodyTypeEditor = factory.CreateEditorViewModel("BoneBodyTypeEditor", "bone_body_types.xml");
            Assert.NotNull(boneBodyTypeEditor);
            Assert.IsType<BoneBodyTypeEditorViewModel>(boneBodyTypeEditor);
            
            var craftingPieceEditor = factory.CreateEditorViewModel("CraftingPieceEditor", "crafting_pieces.xml");
            Assert.NotNull(craftingPieceEditor);
            Assert.IsType<CraftingPieceEditorViewModel>(craftingPieceEditor);
            
            var itemModifierEditor = factory.CreateEditorViewModel("ItemModifierEditor", "item_modifiers.xml");
            Assert.NotNull(itemModifierEditor);
            Assert.IsType<ItemModifierEditorViewModel>(itemModifierEditor);
        }
        
        [Fact]
        public void EditorFactory_ShouldThrowExceptionForUnknownEditor()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IEditorFactory, EditorFactory>();
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetRequiredService<IEditorFactory>();
            
            // Act & Assert
            var unknownEditor = factory.CreateEditorViewModel("UnknownEditor", "unknown_file.xml");
            Assert.Null(unknownEditor);
        }
        
        [Fact]
        public void EditorFactory_ShouldReturnCorrectEditorTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IEditorFactory, EditorFactory>();
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetRequiredService<IEditorFactory>();
            
            // Act & Assert
            var editorTypes = factory.GetRegisteredEditorTypes();
            Assert.NotNull(editorTypes);
            Assert.Contains("AttributeEditor", editorTypes);
            Assert.Contains("SkillEditor", editorTypes);
            Assert.Contains("BoneBodyTypeEditor", editorTypes);
            Assert.Contains("CraftingPieceEditor", editorTypes);
            Assert.Contains("ItemModifierEditor", editorTypes);
        }
    }
}