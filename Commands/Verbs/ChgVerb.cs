using NatureShell.Core;
using System.IO;
using System.Linq;
using ExecutionContext = NatureShell.Core.ExecutionContext;
namespace NatureShell.Commands.Verbs;

public class ChgVerb : IVerb
{
    public string Name => "change";
    public IEnumerable<string> Aliases => new[] { "chg", "cd" };

    public async Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input)
    {
        // 1. CommandParseResult의 'Values' 속성에서 이동할 경로 추출
        var targetPath = command.Values.FirstOrDefault();

        if (string.IsNullOrEmpty(targetPath))
        {
            return input;
        }

        try
        {
            // 2. ExecutionContext의 'CurrentWorkingDirectory' 속성 사용
            string currentDir = context.CurrentWorkingDirectory;
            string newPath = Path.GetFullPath(Path.Combine(currentDir, targetPath));

            if (Directory.Exists(newPath))
            {
                // 3. 쉘 컨텍스트 상태 업데이트
                context.CurrentWorkingDirectory = newPath;

                // 4. 세션 컨텍스트 내의 작업 디렉토리도 함께 업데이트
                context.Session.CurrentWorkingDirectory = newPath;

                // 5. 실제 OS 프로세스의 디렉토리 동기화
                Directory.SetCurrentDirectory(newPath);

                Console.WriteLine($"[Context] Path changed: {newPath}");
            }
            else
            {
                Console.WriteLine($"[Error] Directory not found: {newPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Invalid path: {ex.Message}");
        }

        // 비동기 인터페이스 사양을 준수하기 위해 Task 반환
        return await Task.FromResult(input);
    }
}