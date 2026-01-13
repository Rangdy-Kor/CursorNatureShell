using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Commands.Verbs;

/// <summary>echo 동사 - 출력</summary>
public class EchoVerb : IVerb
{
    public string Name => "echo";
    public IEnumerable<string> Aliases => Array.Empty<string>();

    public Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        var outputs = new List<IShellObject>();

        // 입력이 있으면 입력 출력
        if (input.Any())
        {
            foreach (var obj in input)
            {
                Console.WriteLine(obj.Value ?? "");
                outputs.Add(ShellObject.Success(obj.Value));
            }
        }
        else if (command.Values.Any())
        {
            // 값이 있으면 값 출력
            foreach (var value in command.Values)
            {
                var resolvedValue = ResolveValue(value, context);
                Console.WriteLine(resolvedValue);
                outputs.Add(ShellObject.Success(resolvedValue));
            }
        }
        else
        {
            Console.WriteLine();
            outputs.Add(ShellObject.Success(null));
        }

        return Task.FromResult<IEnumerable<IShellObject>>(outputs);
    }

    private object? ResolveValue(string value, ExecutionContext context)
    {
        // 변수 해석 ($_ 또는 $변수명)
        if (value == "$_")
            return context.CurrentPipelineObject;
        
        if (value.StartsWith("$"))
            return value; // TODO: 변수 저장소에서 조회
        
        return value;
    }
}
