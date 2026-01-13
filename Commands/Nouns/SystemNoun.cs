using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Nouns;

/// <summary>sys 紐낆궗 - ?쒖뒪??媛앹껜</summary>
public class SystemNoun : INoun
{
    public string Name => "sys";
    public IEnumerable<string> Aliases => new[] { "system" };

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context)
    {
        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(command.Values.FirstOrDefault()) });
    }
}
