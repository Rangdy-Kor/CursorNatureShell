using System.IO;
using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Verbs;

/// <summary>list/ls 동사 - 목록 반환</summary>
public class ListVerb : IVerb
{
    public string Name => "list";
    public IEnumerable<string> Aliases => new[] { "ls" };

    public Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        var results = new List<IShellObject>();
        var recursive = command.Adverb?.Equals("rcs", StringComparison.OrdinalIgnoreCase) == true ||
                       command.Adverb?.Equals("recurse", StringComparison.OrdinalIgnoreCase) == true;

        if (noun.Name == "file" || noun.Name == "fl")
        {
            var path = command.Values.FirstOrDefault() ?? context.Session.CurrentWorkingDirectory;
            path = ResolvePath(path?.ToString() ?? "", context);

            if (Directory.Exists(path))
            {
                var files = recursive 
                    ? Directory.GetFiles(path, "*", SearchOption.AllDirectories)
                    : Directory.GetFiles(path);
                
                results.AddRange(files.Select(f => ShellObject.Success(f)));
            }
            else if (File.Exists(path))
            {
                results.Add(ShellObject.Success(path));
            }
        }
        else if (noun.Name == "dir" || noun.Name == "directory")
        {
            var path = command.Values.FirstOrDefault() ?? context.Session.CurrentWorkingDirectory;
            path = ResolvePath(path?.ToString() ?? "", context);

            if (Directory.Exists(path))
            {
                var dirs = recursive
                    ? Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                    : Directory.GetDirectories(path);
                
                results.AddRange(dirs.Select(d => ShellObject.Success(d)));
            }
        }

        if (!results.Any() && input.Any())
        {
            // 입력이 있으면 입력을 그대로 반환
            return Task.FromResult(input);
        }

        return Task.FromResult<IEnumerable<IShellObject>>(results);
    }

    private string ResolvePath(string path, ExecutionContext context)
    {
        if (string.IsNullOrEmpty(path))
            return context.Session.CurrentWorkingDirectory;
        
        if (Path.IsPathRooted(path))
            return path;
        
        return Path.Combine(context.Session.CurrentWorkingDirectory, path);
    }
}
