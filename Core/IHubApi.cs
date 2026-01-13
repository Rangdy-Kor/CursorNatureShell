namespace NatureShell.Core;

/// <summary>
/// Hub API - 모든 명령 실행의 중앙 조정자 (Mediator Pattern)
/// </summary>
public interface IHubApi
{
    /// <summary>명령 실행</summary>
    Task<IEnumerable<IShellObject>> ExecuteCommandAsync(CommandParseResult command, ExecutionContext context);
    
    /// <summary>명사 등록</summary>
    void RegisterNoun(INoun noun);
    
    /// <summary>동사 등록</summary>
    void RegisterVerb(IVerb verb);
    
    /// <summary>전치사 등록</summary>
    void RegisterPreposition(IPreposition preposition);
    
    /// <summary>명사 조회</summary>
    INoun? GetNoun(string name);
    
    /// <summary>동사 조회</summary>
    IVerb? GetVerb(string name);
    
    /// <summary>전치사 조회</summary>
    IPreposition? GetPreposition(string name);
}
