namespace NatureShell.Core;

/// <summary>
/// 실행 컨텍스트 - $ctx 객체를 나타냄
/// </summary>
public class ExecutionContext
{
    public string CurrentWorkingDirectory { get; set; } = Directory.GetCurrentDirectory();
    public string CurrentPath => CurrentWorkingDirectory;
    public ProcessContext Process { get; set; } = new();
    public SessionContext Session { get; set; } = new();
    public RuntimeContext Runtime { get; set; } = new();
    public PipelineContext Pipeline { get; set; } = new();
    public ErrorContext Error { get; set; } = new();
    
    /// <summary>파이프라인 현재 객체 ($_)</summary>
    public object? CurrentPipelineObject => Pipeline.Current;
}

/// <summary>프로세스 컨텍스트 - $ctx.process</summary>
public class ProcessContext
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public bool Async { get; set; }
    public int ThreadId { get; set; }
}

/// <summary>세션 컨텍스트 - $ctx.session</summary>
public class SessionContext
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CurrentWorkingDirectory { get; set; } = Environment.CurrentDirectory;
    public string User { get; set; } = Environment.UserName;
    public Permission Permission { get; set; } = Permission.User;
}

/// <summary>런타임 컨텍스트 - $ctx.runtime</summary>
public class RuntimeContext
{
    public string ShellVersion { get; set; } = "1.0.0";
    public string HubApiVersion { get; set; } = "1.0.0";
    public string OS { get; set; } = Environment.OSVersion.Platform.ToString();
    public string Architecture { get; set; } = Environment.Is64BitProcess ? "x64" : "x86";
}

/// <summary>파이프라인 컨텍스트 - $ctx.pipeline</summary>
public class PipelineContext
{
    public object? Current { get; set; }
    public IEnumerable<object>? Previous { get; set; }
    public int Index { get; set; }
    public bool IsEmpty { get; set; } = true;
}

/// <summary>오류 컨텍스트 - $ctx.error</summary>
public class ErrorContext
{
    public Exception? Last { get; set; }
    public List<Exception> Stack { get; set; } = new();
    public bool Handled { get; set; }
}
