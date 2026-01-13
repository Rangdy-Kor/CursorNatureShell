namespace NatureShell.Core;

/// <summary>
/// 파서가 생성하는 명령 파싱 결과
/// 구조: [Permission] [Noun:Adjective] [Verb!Adverb] [Value] [-Preposition] [Value]
/// </summary>
public class CommandParseResult
{
    public Permission Permission { get; set; } = Permission.User;
    public string? Noun { get; set; }
    public string? Adjective { get; set; }
    public string? Verb { get; set; }
    public string? Adverb { get; set; }
    public List<string> Values { get; set; } = new();
    public Dictionary<string, PrepositionBlock> Prepositions { get; set; } = new();
    public string OriginalCommand { get; set; } = string.Empty;
}

/// <summary>전치사 블록 (예: -if { ... } -else { ... })</summary>
public class PrepositionBlock
{
    public string Name { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Block { get; set; }  // 중괄호 내부 코드
}

/// <summary>명령 실행 권한</summary>
public enum Permission
{
    User,   // 일반 권한 (기본값)
    Admin,  // 관리자 권한
    Root    // 전체 시스템 제어 권한
}
