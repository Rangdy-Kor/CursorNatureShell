using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Nouns;

/// <summary>dir 紐낆궗 - ?붾젆?좊━ 媛앹껜</summary>
public class DirectoryNoun : INoun
{
    public string Name => "dir";
    public IEnumerable<string> Aliases => new[] { "directory" };

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context)
    {
        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(command.Values.FirstOrDefault()) });
    }
}
