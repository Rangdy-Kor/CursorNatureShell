namespace NatureShell.Models;

/// <summary>변수 저장소 - 스크립트 변수 관리</summary>
public class VariableStore
{
    private readonly Dictionary<string, object?> _variables = new();

    public void Set(string name, object? value)
    {
        if (string.IsNullOrEmpty(name) || !name.StartsWith("$"))
            throw new ArgumentException("Variable name must start with $", nameof(name));
        
        _variables[name] = value;
    }

    public object? Get(string name)
    {
        return _variables.TryGetValue(name, out var value) ? value : null;
    }

    public bool Exists(string name) => _variables.ContainsKey(name);

    public void Remove(string name) => _variables.Remove(name);

    public void Clear() => _variables.Clear();
}
