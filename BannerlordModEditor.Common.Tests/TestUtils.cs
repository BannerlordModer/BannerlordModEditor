using System;
using System.IO;

namespace BannerlordModEditor.Common.Tests;

public static class TestUtils
{
    public static string GetSolutionRoot()
    {
        var currentDirectory = AppContext.BaseDirectory;
        while (currentDirectory != null)
        {
            if (Directory.GetFiles(currentDirectory, "*.sln").Length > 0)
            {
                return currentDirectory;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        throw new DirectoryNotFoundException("Could not find the solution root directory.");
    }
} 