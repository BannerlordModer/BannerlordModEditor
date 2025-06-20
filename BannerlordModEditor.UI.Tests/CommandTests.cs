using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels.Editors;

namespace BannerlordModEditor.UI.Tests;

public class CommandTests
{
    [AvaloniaFact]
    public void AttributeEditor_Commands_ShouldWork()
    {
        // 创建属性编辑器
        var viewModel = new AttributeEditorViewModel();
        var view = new AttributeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        // 验证初始状态
        var initialCount = viewModel.Attributes.Count;
        Assert.False(viewModel.HasUnsavedChanges);
        
        // 测试添加命令是否存在且可执行
        Assert.NotNull(viewModel.AddAttributeCommand);
        Assert.True(viewModel.AddAttributeCommand.CanExecute(null));
        
        // 执行添加命令
        viewModel.AddAttributeCommand.Execute(null);
        
        // 验证状态变化
        Assert.Equal(initialCount + 1, viewModel.Attributes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
        
        // 测试删除命令
        var attributeToRemove = viewModel.Attributes.Last();
        Assert.NotNull(viewModel.RemoveAttributeCommand);
        Assert.True(viewModel.RemoveAttributeCommand.CanExecute(attributeToRemove));
        
        viewModel.RemoveAttributeCommand.Execute(attributeToRemove);
        Assert.Equal(initialCount, viewModel.Attributes.Count);
        
        // 测试加载命令
        Assert.NotNull(viewModel.LoadFileCommand);
        Assert.True(viewModel.LoadFileCommand.CanExecute(null));
        
        // 测试保存命令
        Assert.NotNull(viewModel.SaveFileCommand);
        Assert.True(viewModel.SaveFileCommand.CanExecute(null));
    }
    
    [AvaloniaFact]
    public void BoneBodyTypeEditor_Commands_ShouldWork()
    {
        // 创建骨骼编辑器
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        // 验证初始状态
        var initialCount = viewModel.BoneBodyTypes.Count;
        Assert.False(viewModel.HasUnsavedChanges);
        
        // 测试添加命令是否存在且可执行
        Assert.NotNull(viewModel.AddBoneBodyTypeCommand);
        Assert.True(viewModel.AddBoneBodyTypeCommand.CanExecute(null));
        
        // 执行添加命令
        viewModel.AddBoneBodyTypeCommand.Execute(null);
        
        // 验证状态变化
        Assert.Equal(initialCount + 1, viewModel.BoneBodyTypes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
        
        // 测试删除命令
        var boneTypeToRemove = viewModel.BoneBodyTypes.Last();
        Assert.NotNull(viewModel.RemoveBoneBodyTypeCommand);
        Assert.True(viewModel.RemoveBoneBodyTypeCommand.CanExecute(boneTypeToRemove));
        
        viewModel.RemoveBoneBodyTypeCommand.Execute(boneTypeToRemove);
        Assert.Equal(initialCount, viewModel.BoneBodyTypes.Count);
        
        // 测试加载命令
        Assert.NotNull(viewModel.LoadFileCommand);
        Assert.True(viewModel.LoadFileCommand.CanExecute(null));
        
        // 测试保存命令
        Assert.NotNull(viewModel.SaveFileCommand);
        Assert.True(viewModel.SaveFileCommand.CanExecute(null));
    }
    
    [AvaloniaFact]
    public void AttributeEditor_LoadFile_ShouldLoadTestData()
    {
        var viewModel = new AttributeEditorViewModel();
        var view = new AttributeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var initialCount = viewModel.Attributes.Count;
        
        // 执行加载命令
        viewModel.LoadFileCommand.Execute(null);
        
        // 如果TestData/attributes.xml存在，应该加载数据
        if (File.Exists(@"TestData\attributes.xml"))
        {
            Assert.True(viewModel.Attributes.Count > initialCount || viewModel.Attributes.Count >= 1);
            Assert.Equal(@"TestData\attributes.xml", viewModel.FilePath);
            Assert.False(viewModel.HasUnsavedChanges);
        }
    }
    
    [AvaloniaFact]
    public void BoneBodyTypeEditor_LoadFile_ShouldLoadTestData()
    {
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var initialCount = viewModel.BoneBodyTypes.Count;
        
        // 执行加载命令
        viewModel.LoadFileCommand.Execute(null);
        
        // 如果TestData/bone_body_types.xml存在，应该加载数据
        if (File.Exists(@"TestData\bone_body_types.xml"))
        {
            Assert.True(viewModel.BoneBodyTypes.Count > initialCount || viewModel.BoneBodyTypes.Count >= 1);
            Assert.Equal(@"TestData\bone_body_types.xml", viewModel.FilePath);
            Assert.False(viewModel.HasUnsavedChanges);
        }
    }
} 