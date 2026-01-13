using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Prepositions;

/// <summary>-in 전치사 - 값 지정</summary>
public class InPreposition : IPreposition
{
    public string Name => "in";
    public IEnumerable<string> Aliases => Array.Empty<string>();

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        // -in은 값 지정용이므로 여기서는 처리하지 않음
        return Task.FromResult(input);
    }
}
