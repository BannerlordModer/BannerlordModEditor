using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels.Editors;

namespace BannerlordModEditor.UI.Tests;

public class BoneBodyTypeEditorTests
{
    [AvaloniaFact]
    public void BoneBodyTypeEditor_ShouldInitializeWithSampleData()
    {
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        // 验证初始化状态
        Assert.NotEmpty(viewModel.BoneBodyTypes);
        Assert.Equal("head", viewModel.BoneBodyTypes[0].Type);
        Assert.Equal("4", viewModel.BoneBodyTypes[0].Priority);
        Assert.False(viewModel.HasUnsavedChanges);
    }

    [AvaloniaFact]
    public void BoneBodyTypeEditor_ShouldAddNewBoneBodyType()
    {
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var initialCount = viewModel.BoneBodyTypes.Count;
        
        // 添加新骨骼类型
        viewModel.AddBoneBodyTypeCommand.Execute(null);

        Assert.Equal(initialCount + 1, viewModel.BoneBodyTypes.Count);
        Assert.True(viewModel.HasUnsavedChanges);
        Assert.Equal("new_bone_type", viewModel.BoneBodyTypes.Last().Type);
    }

    [AvaloniaFact]
    public void BoneBodyTypeEditor_ShouldRemoveBoneBodyType()
    {
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        var boneTypeToRemove = viewModel.BoneBodyTypes.First();
        var initialCount = viewModel.BoneBodyTypes.Count;
        
        // 删除骨骼类型
        viewModel.RemoveBoneBodyTypeCommand.Execute(boneTypeToRemove);

        Assert.Equal(initialCount - 1, viewModel.BoneBodyTypes.Count);
        Assert.DoesNotContain(boneTypeToRemove, viewModel.BoneBodyTypes);
        Assert.True(viewModel.HasUnsavedChanges);
    }

    [AvaloniaFact]
    public void BoneBodyTypeViewModel_ShouldValidateCorrectly()
    {
        var boneTypeViewModel = new BoneBodyTypeViewModel();

        // 初始状态应该无效（空Type）
        Assert.False(boneTypeViewModel.IsValid);

        // 设置Type但Priority无效
        boneTypeViewModel.Type = "head";
        boneTypeViewModel.Priority = "10"; // 超出范围
        Assert.False(boneTypeViewModel.IsValid);

        // 设置有效Priority
        boneTypeViewModel.Priority = "3";
        Assert.True(boneTypeViewModel.IsValid);

        // 清空Type
        boneTypeViewModel.Type = "";
        Assert.False(boneTypeViewModel.IsValid);
    }

    [AvaloniaFact]
    public void BoneBodyTypeViewModel_ShouldHaveCorrectOptions()
    {
        var boneTypeViewModel = new BoneBodyTypeViewModel();
        
        var typeOptions = boneTypeViewModel.TypeOptions.ToList();
        var priorityOptions = boneTypeViewModel.PriorityOptions.ToList();
        var booleanOptions = boneTypeViewModel.BooleanOptions.ToList();
        
        // 验证类型选项
        Assert.Contains("head", typeOptions);
        Assert.Contains("neck", typeOptions);
        Assert.Contains("arm_left", typeOptions);
        
        // 验证优先级选项
        Assert.Contains("1", priorityOptions);
        Assert.Contains("5", priorityOptions);
        Assert.Equal(5, priorityOptions.Count);
        
        // 验证布尔选项
        Assert.Contains("", booleanOptions);
        Assert.Contains("true", booleanOptions);
        Assert.Contains("false", booleanOptions);
    }

    [AvaloniaFact]
    public void BoneBodyTypeViewModel_ShouldHandleOptionalFields()
    {
        var boneTypeViewModel = new BoneBodyTypeViewModel
        {
            Type = "arm_left",
            Priority = "1",
            ActivateSweep = "true",
            UseSmallerRadiusMultWhileHoldingShield = "",
            DoNotScaleAccordingToAgentScale = "false"
        };

        Assert.True(boneTypeViewModel.IsValid);
        Assert.Equal("true", boneTypeViewModel.ActivateSweep);
        Assert.Equal("", boneTypeViewModel.UseSmallerRadiusMultWhileHoldingShield);
        Assert.Equal("false", boneTypeViewModel.DoNotScaleAccordingToAgentScale);
    }

    [AvaloniaFact]
    public void BoneBodyTypeEditor_ShouldHandleComplexScenario()
    {
        var viewModel = new BoneBodyTypeEditorViewModel();
        var view = new BoneBodyTypeEditorView { DataContext = viewModel };
        
        var window = new Window { Content = view };
        window.Show();

        // 添加多个骨骼类型
        viewModel.AddBoneBodyTypeCommand.Execute(null);
        viewModel.AddBoneBodyTypeCommand.Execute(null);
        
        Assert.Equal(3, viewModel.BoneBodyTypes.Count); // 1 initial + 2 added
        Assert.True(viewModel.HasUnsavedChanges);

        // 配置新添加的骨骼类型
        var newBoneType = viewModel.BoneBodyTypes[1];
        newBoneType.Type = "chest";
        newBoneType.Priority = "3";
        newBoneType.ActivateSweep = "true";

        Assert.True(newBoneType.IsValid);
        
        // 删除一个
        viewModel.RemoveBoneBodyTypeCommand.Execute(newBoneType);
        
        Assert.Equal(2, viewModel.BoneBodyTypes.Count);
        Assert.DoesNotContain(newBoneType, viewModel.BoneBodyTypes);
    }
} 