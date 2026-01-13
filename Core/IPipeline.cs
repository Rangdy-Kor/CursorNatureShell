namespace NatureShell.Core;

/// <summary>
/// 파이프라인 오케스트레이터 인터페이스
/// </summary>
public interface IPipeline
{
    /// <summary>파이프라인 실행</summary>
    Task<IEnumerable<IShellObject>> ExecuteAsync(
        IEnumerable<string> pipelineCommands,
        IHubApi hubApi,
        ExecutionContext context);
}
