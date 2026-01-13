using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Engine;

/// <summary>
/// 파이프라인 오케스트레이터 구현
/// </summary>
public class Pipeline : IPipeline
{
    private readonly IParser _parser;

    public Pipeline(IParser parser)
    {
        _parser = parser;
    }

    public async Task<IEnumerable<IShellObject>> ExecuteAsync(
        IEnumerable<string> pipelineCommands,
        IHubApi hubApi,
        ExecutionContext context)
    {
        var input = Enumerable.Empty<IShellObject>();
        int index = 0;

        foreach (var commandLine in pipelineCommands)
        {
            context.Pipeline.Index = index++;
            context.Pipeline.Previous = input.Select(obj => obj.Value).ToList<object>();
            context.Pipeline.IsEmpty = !input.Any();

            var command = _parser.Parse(commandLine);
            
            // 첫 번째 명령이 전치사로 시작하는 경우 (예: -foreach)
            if (command.Noun == null && command.Verb == null && command.Prepositions.Count > 0)
            {
                var prep = command.Prepositions.First();
                var preposition = hubApi.GetPreposition(prep.Key);
                if (preposition != null)
                {
                    input = await preposition.ProcessAsync(command, hubApi, context, input);
                }
                continue;
            }

            // 일반 명령 실행
            var results = await hubApi.ExecuteCommandAsync(command, context);
            
            // 파이프라인 컨텍스트 업데이트
            var resultList = results.ToList();
            if (resultList.Count > 0)
            {
                context.Pipeline.Current = resultList[0].Value;
            }

            input = resultList;
        }

        return input;
    }
}
