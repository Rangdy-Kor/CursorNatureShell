namespace NatureShell.Core;

/// <summary>
/// NatureShell 명령 인터페이스
/// 모든 명령은 Hub API를 통해 실행된다
/// </summary>
public interface ICommand
{
    /// <summary>명령 실행 (Hub API를 통해)</summary>
    Task<IEnumerable<IShellObject>> ExecuteAsync(CommandParseResult command, IHubApi hubApi, ExecutionContext context);
}
