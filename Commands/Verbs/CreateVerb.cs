using System.IO;
using System.Text;
using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Verbs;

/// <summary>create/crt ?숈궗 - 媛앹껜 ?앹꽦</summary>
public class CreateVerb : IVerb
{
    public string Name => "create";
    public IEnumerable<string> Aliases => new[] { "crt" };

    public async Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        if (noun.Name == "var" || noun.Name == "variable")
        {
            // 蹂???앹꽦: var:int crt $name -in value
            var varName = command.Values.FirstOrDefault()?.ToString();
            if (string.IsNullOrEmpty(varName) || !varName.StartsWith("$"))
                return new[] { ShellObject.FromError(new ArgumentException("Variable name must start with $")) };

            if (command.Prepositions.TryGetValue("in", out var inPrep))
            {
                var value = inPrep.Value ?? inPrep.Block;
                return new[] { ShellObject.Success(new { Name = varName, Value = value }) };
            }
        }
        else if (noun.Name == "file" || noun.Name == "fl")
        {
            // ?뚯씪 ?앹꽦
            var path = command.Values.FirstOrDefault()?.ToString();
            if (string.IsNullOrEmpty(path))
                return new[] { ShellObject.FromError(new ArgumentException("File path is required")) };

            path = ResolvePath(path, context);

            string? content = null;
            if (command.Prepositions.TryGetValue("in", out var inPrep))
            {
                content = inPrep.Value ?? inPrep.Block ?? "";
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? "");
                await File.WriteAllTextAsync(path, content ?? "");
                return new[] { ShellObject.Success(path) };
            }
            catch (Exception ex)
            {
                return new[] { ShellObject.FromError(ex) };
            }
        }
        else if (noun.Name == "dir" || noun.Name == "directory")
        {
            var path = command.Values.FirstOrDefault()?.ToString();
            if (string.IsNullOrEmpty(path))
                return new[] { ShellObject.FromError(new ArgumentException("Directory path is required")) };

            path = ResolvePath(path, context);

            try
            {
                Directory.CreateDirectory(path);
                return new[] { ShellObject.Success(path) };
            }
            catch (Exception ex)
            {
                return new[] { ShellObject.FromError(ex) };
            }
        }

        return new[] { ShellObject.Success(null) };
    }

    private string ResolvePath(string path, ExecutionContext context)
    {
        if (Path.IsPathRooted(path))
            return path;
        
        return Path.Combine(context.Session.CurrentWorkingDirectory, path);
    }
}
