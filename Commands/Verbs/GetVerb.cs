using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Verbs;

/// <summary>get 동사 - 객체 반환</summary>
public class GetVerb : IVerb
{
    public string Name => "get";
    public IEnumerable<string> Aliases => Array.Empty<string>();

    public Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        if (noun.Name == "sys" || noun.Name == "system")
        {
            // 시스템 정보 조회
            var adj = command.Adjective?.ToLower();
            var result = adj switch
            {
                "mem" or "memory" => new[] { ShellObject.Success(GC.GetTotalMemory(false) / 1024 / 1024) }, // MB
                "cpu" => new[] { ShellObject.Success(Environment.ProcessorCount) },
                "date" => new[] { ShellObject.Success(DateTime.Now) },
                _ => new[] { ShellObject.Success(context.Runtime) }
            };
            return Task.FromResult<IEnumerable<IShellObject>>(result);
        }

        // 입력이 있으면 입력 반환
        if (input.Any())
            return Task.FromResult(input);

        return Task.FromResult<IEnumerable<IShellObject>>(new[] { ShellObject.Success(command.Values.FirstOrDefault()) });
    }
}
