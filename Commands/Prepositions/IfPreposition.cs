using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Prepositions;

/// <summary>-if/-else 전치사 - 조건부 실행</summary>
public class IfPreposition : IPreposition
{
    public string Name => "if";
    public IEnumerable<string> Aliases => Array.Empty<string>();

    public Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        // -if는 전치사가 아니라 조건식과 함께 사용됨
        // 실제 구현은 더 복잡한 파싱이 필요
        return Task.FromResult(input);
    }
}
