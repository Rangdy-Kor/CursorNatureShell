using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Nouns;

/// <summary>tmp 紐낆궗 - ?꾩떆 媛앹껜 (紐낆궗媛 ?꾩슂?섏? ?딆? 紐낅졊??</summary>
public class TemporaryNoun : INoun
{
    public string Name => "tmp";
    public IEnumerable<string> Aliases => Array.Empty<string>();

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context)
    {
        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(null) });
    }
}
