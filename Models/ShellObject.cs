using NatureShell.Core;

namespace NatureShell.Models;

/// <summary>
/// ShellObject 구현체 - 파이프라인을 통해 전달되는 객체
/// </summary>
public class ShellObject : IShellObject
{
    public object? Value { get; }
    public ShellObjectStatus Status { get; }
    public Exception? Error { get; }
    public Type ValueType { get; }
    public bool IsSuccess => Status == ShellObjectStatus.Success && Error == null;

    public ShellObject(object? value, ShellObjectStatus status = ShellObjectStatus.Success, Exception? error = null)
    {
        Value = value;
        Status = status;
        Error = error;
        ValueType = value?.GetType() ?? typeof(object);
    }

    public static ShellObject Success(object? value) => new(value, ShellObjectStatus.Success);
    public static ShellObject FromError(Exception error) => new(null, ShellObjectStatus.Error, error);
    public static ShellObject Empty() => new(null, ShellObjectStatus.Empty);
}
