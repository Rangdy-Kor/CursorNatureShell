using NatureShell.Core;
using ExecutionContext = NatureShell.Core.ExecutionContext;
using NatureShell.Models;

namespace NatureShell.Engine;

/// <summary>
/// Hub API 구현 - 모든 명령 실행의 중앙 조정자
/// </summary>
public class HubApi : IHubApi
{
    private readonly Dictionary<string, INoun> _nouns = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IVerb> _verbs = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IPreposition> _prepositions = new(StringComparer.OrdinalIgnoreCase);

    public void RegisterNoun(INoun noun)
    {
        _nouns[noun.Name] = noun;
        foreach (var alias in noun.Aliases)
        {
            _nouns[alias] = noun;
        }
    }

    public void RegisterVerb(IVerb verb)
    {
        _verbs[verb.Name] = verb;
        foreach (var alias in verb.Aliases)
        {
            _verbs[alias] = verb;
        }
    }

    public void RegisterPreposition(IPreposition preposition)
    {
        _prepositions[preposition.Name] = preposition;
        foreach (var alias in preposition.Aliases)
        {
            _prepositions[alias] = preposition;
        }
    }

    public INoun? GetNoun(string name) => _nouns.TryGetValue(name, out var noun) ? noun : null;
    public IVerb? GetVerb(string name) => _verbs.TryGetValue(name, out var verb) ? verb : null;
    public IPreposition? GetPreposition(string name) => 
        _prepositions.TryGetValue(name, out var prep) ? prep : null;

    public async Task<IEnumerable<IShellObject>> ExecuteCommandAsync(
        CommandParseResult command,
        ExecutionContext context)
    {
        // 명사가 없고 동사만 있는 경우 (예: echo)
        if (string.IsNullOrEmpty(command.Noun) && !string.IsNullOrEmpty(command.Verb))
        {
            var verbOnly = GetVerb(command.Verb);
            if (verbOnly == null)
                return new[] { ShellObject.FromError(new ArgumentException($"Unknown verb: {command.Verb}")) };

            // 임시 명사로 처리 - tmp 명사가 등록되어 있어야 함
            var tmpNoun = GetNoun("tmp");
            if (tmpNoun == null)
                return new[] { ShellObject.FromError(new ArgumentException("Verb without noun requires 'tmp' noun to be registered")) };
            
            return await verbOnly.ExecuteAsync(tmpNoun, command, this, context, Enumerable.Empty<IShellObject>());
        }

        if (string.IsNullOrEmpty(command.Noun))
            return new[] { ShellObject.FromError(new ArgumentException("Noun or Verb is required")) };

        var noun = GetNoun(command.Noun);
        if (noun == null)
            return new[] { ShellObject.FromError(new ArgumentException($"Unknown noun: {command.Noun}")) };

        if (string.IsNullOrEmpty(command.Verb))
            return await noun.ProcessAsync(command, this, context);

        var verb = GetVerb(command.Verb);
        if (verb == null)
            return new[] { ShellObject.FromError(new ArgumentException($"Unknown verb: {command.Verb}")) };

        // 명사 처리 후 동사 실행
        var nounResults = await noun.ProcessAsync(command, this, context);
        return await verb.ExecuteAsync(noun, command, this, context, nounResults);
    }
}
