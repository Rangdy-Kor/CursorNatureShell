using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Engine;
using NatureShell.Models;
using NatureShell.Commands.Nouns;
using NatureShell.Commands.Verbs;
using NatureShell.Commands.Prepositions;

namespace NatureShell;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("NatureShell v1.0.0 - POS-based Object-Oriented Shell");
        Console.WriteLine("Type 'exit' or 'quit' to exit.\n");

        // HubApi 및 파서 초기화
        var hubApi = new HubApi();
        var parser = new Parser();
        var pipeline = new Pipeline(parser);

        // 명사/동사/전치사 등록
        RegisterCommands(hubApi);

        // 실행 컨텍스트 초기화
        var context = new ExecutionContext
        {
            Process = new ProcessContext
            {
                Id = Environment.ProcessId,
                ThreadId = Environment.CurrentManagedThreadId,
                Async = false
            },
            Session = new SessionContext
            {
                CurrentWorkingDirectory = Environment.CurrentDirectory
            },
            Runtime = new RuntimeContext()
        };

        // REPL 루프
        while (true)
        {
            try
            {
                Console.Write("NatureShell> ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                input = input.Trim();
                
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                // 파이프라인 분리 및 실행
                var pipelineCommands = parser.SplitPipeline(input).ToList();
                var results = await pipeline.ExecuteAsync(pipelineCommands, hubApi, context);

                // 에러 처리
                var errorResults = results.Where(r => !r.IsSuccess).ToList();
                if (errorResults.Any())
                {
                    foreach (var error in errorResults)
                    {
                        if (error.Error != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Error: {error.Error.Message}");
                            Console.ResetColor();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fatal Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        Console.WriteLine("Goodbye!");
    }

    static void RegisterCommands(IHubApi hubApi)
    {
        // 명사 등록
        hubApi.RegisterNoun(new FileNoun());
        hubApi.RegisterNoun(new VariableNoun());
        hubApi.RegisterNoun(new SystemNoun());
        hubApi.RegisterNoun(new DirectoryNoun());
        hubApi.RegisterNoun(new TemporaryNoun());

        // 동사 등록
        hubApi.RegisterVerb(new ListVerb());
        hubApi.RegisterVerb(new CreateVerb());
        hubApi.RegisterVerb(new EchoVerb());
        hubApi.RegisterVerb(new GetVerb());

        // 전치사 등록
        hubApi.RegisterPreposition(new ForeachPreposition());
        hubApi.RegisterPreposition(new IfPreposition());
        hubApi.RegisterPreposition(new InPreposition());
    }
}
