using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Nouns;

/// <summary>var 紐낆궗 - 蹂??媛앹껜</summary>
public class VariableNoun : INoun
{
    public string Name => "var";
    public IEnumerable<string> Aliases => new[] { "variable" };

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context)
    {
        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(command.Values.FirstOrDefault()) });
    }
}
