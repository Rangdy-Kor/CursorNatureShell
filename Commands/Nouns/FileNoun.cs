using System.IO;
using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Nouns;

/// <summary>file 명사 - 파일 객체</summary>
public class FileNoun : INoun
{
    public string Name => "file";
    public IEnumerable<string> Aliases => new[] { "fl" };

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context)
    {
        // 명사만으로는 처리하지 않음, 동사가 필요
        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(command.Values.FirstOrDefault()) });
    }
}
