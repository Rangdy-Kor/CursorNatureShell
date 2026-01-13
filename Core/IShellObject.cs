namespace NatureShell.Core;

/// <summary>
/// 파이프라인을 통해 전달되는 쉘 객체
/// 값, 상태, 오류를 모두 포함한다
/// </summary>
public interface IShellObject
{
    /// <summary>객체의 실제 값</summary>
    object? Value { get; }
    
    /// <summary>실행 상태 (성공, 실패, 경고 등)</summary>
    ShellObjectStatus Status { get; }
    
    /// <summary>오류 객체 (있을 경우)</summary>
    Exception? Error { get; }
    
    /// <summary>객체 타입 정보</summary>
    Type ValueType { get; }
    
    /// <summary>성공 객체인지 확인</summary>
    bool IsSuccess { get; }
}

/// <summary>ShellObject 상태</summary>
public enum ShellObjectStatus
{
    Success,
    Error,
    Warning,
    Empty
}
