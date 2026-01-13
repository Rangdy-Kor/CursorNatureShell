namespace NatureShell.Core;

/// <summary>
/// 전치사 인터페이스 - 값과 값을 연결하여 실행 맥락과 관계를 지정
/// </summary>
public interface IPreposition
{
    /// <summary>전치사 이름 (예: -if, -foreach, -in)</summary>
    string Name { get; }
    
    /// <summary>전치사 별칭 목록 (예: -for, -cat)</summary>
    IEnumerable<string> Aliases { get; }
    
    /// <summary>전치사 처리</summary>
    Task<IEnumerable<IShellObject>> ProcessAsync(
        CommandParseResult command,
        IHubApi hubApi,
        ExecutionContext context,
        IEnumerable<IShellObject> input);
}
