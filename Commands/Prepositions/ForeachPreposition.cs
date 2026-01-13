using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Engine;
using NatureShell.Models;

namespace NatureShell.Commands.Prepositions;

/// <summary>-foreach/-for ?꾩튂??- 諛섎났 ?ㅽ뻾</summary>
public class ForeachPreposition : IPreposition
{
    public string Name => "foreach";
    public IEnumerable<string> Aliases => new[] { "for" };

    public async Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        var results = new List<IShellObject>();
        var originalContext = context;

        // -foreach { ... } 釉붾줉 泥섎━
        if (command.Prepositions.TryGetValue("foreach", out var foreachPrep) ||
            command.Prepositions.TryGetValue("for", out foreachPrep))
        {
            var block = foreachPrep.Block;
            if (string.IsNullOrEmpty(block))
                return new[] { ShellObject.FromError(new ArgumentException("Foreach block is required")) };

            var parser = new Parser();
            var blockCommands = parser.SplitPipeline(block);

            foreach (var obj in input)
            {
                // 媛??낅젰 媛앹껜?????而⑦뀓?ㅽ듃 ?낅뜲?댄듃
                var loopContext = new ExecutionContext
                {
                    Process = originalContext.Process,
                    Session = originalContext.Session,
                    Runtime = originalContext.Runtime,
                    Pipeline = new PipelineContext
                    {
                        Current = obj.Value,
                        Index = originalContext.Pipeline.Index,
                        IsEmpty = false
                    },
                    Error = originalContext.Error
                };

                var pipeline = new Pipeline(parser);
                var blockResults = await pipeline.ExecuteAsync(blockCommands, hubApi, loopContext);
                results.AddRange(blockResults);
            }
        }

        return results;
    }
}
