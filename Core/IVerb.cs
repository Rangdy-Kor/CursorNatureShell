namespace NatureShell.Core;

/// <summary>
/// 동사 인터페이스 - 명사가 수행할 행동
/// </summary>
public interface IVerb
{
    /// <summary>동사 이름 (예: get, create, list)</summary>
    string Name { get; }
    
    /// <summary>동사 별칭 목록 (예: ls, crt, rd)</summary>
    IEnumerable<string> Aliases { get; }
    
    /// <summary>동사 실행</summary>
    Task<IEnumerable<IShellObject>> ExecuteAsync(
        INoun noun,
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input);
}
