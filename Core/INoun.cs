namespace NatureShell.Core;

/// <summary>
/// 명사 인터페이스 - 명령이 작용하는 대상 객체
/// </summary>
public interface INoun
{
    /// <summary>명사 이름 (예: file, var, sys)</summary>
    string Name { get; }
    
    /// <summary>명사 별칭 목록 (예: fl, fd)</summary>
    IEnumerable<string> Aliases { get; }
    
    /// <summary>명사 처리</summary>
    Task<IEnumerable<IShellObject>> ProcessAsync(CommandParseResult command, IHubApi hubApi, ExecutionContext context);
}
