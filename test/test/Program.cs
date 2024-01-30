using System.IO;
using RazorEngine;
using RazorEngine.Templating;
using Xunit;

public class ViewTests
{
    [Fact]
    public void AllCshtmlFiles_AreWellFormed()
    {
        string viewsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Online_school/Views");

        var subdirectories = Directory.GetDirectories(viewsFolderPath, "*", SearchOption.TopDirectoryOnly);

        foreach (var subdirectory in subdirectories)
        {
            var cshtmlFiles = Directory.GetFiles(subdirectory, "*.cshtml");

            foreach (var cshtmlFile in cshtmlFiles)
            {
                var fileContent = File.ReadAllText(cshtmlFile);

                try
                {
                    Engine.Razor.RunCompile(fileContent, $"testKey");
                }
                catch (Exception ex)
                {
                    Assert.True(false, $"Compilation failed for file: {cshtmlFile}\nError: {ex.Message}");
                }
            }
        }
    }

}
