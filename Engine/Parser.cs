using System.Text;
using System.Text.RegularExpressions;
using NatureShell.Core;
using NatureShell.Models;

namespace NatureShell.Engine;

/// <summary>
/// POS 기반 문법 파서 구현
/// 구조: [Permission] [Noun:Adjective] [Verb!Adverb] [Value] [-Preposition] [Value]
/// </summary>
public class Parser : IParser
{
    public CommandParseResult Parse(string commandLine)
    {
        var result = new CommandParseResult { OriginalCommand = commandLine };
        
        // 주석 제거
        commandLine = RemoveComments(commandLine).Trim();
        if (string.IsNullOrWhiteSpace(commandLine))
            return result;

        // 파이프라인 분리 (이 메서드는 단일 명령만 처리)
        var parts = Tokenize(commandLine);
        if (parts.Count == 0)
            return result;

        int index = 0;

        // Permission 파싱 (선택적, 맨 앞)
        if (index < parts.Count && IsPermission(parts[index]))
        {
            result.Permission = ParsePermission(parts[index]);
            index++;
        }

        // Noun:Adjective 파싱
        if (index < parts.Count)
        {
            var nounPart = parts[index];
            if (nounPart.Contains(':'))
            {
                var nounParts = nounPart.Split(':', 2);
                result.Noun = nounParts[0];
                result.Adjective = nounParts[1];
            }
            else
            {
                result.Noun = nounPart;
            }
            index++;
        }

        // Verb!Adverb 파싱
        if (index < parts.Count)
        {
            var verbPart = parts[index];
            if (verbPart.Contains('!'))
            {
                var verbParts = verbPart.Split('!', 2);
                result.Verb = verbParts[0];
                result.Adverb = verbParts[1];
            }
            else
            {
                result.Verb = verbPart;
            }
            index++;
        }

        // Values 및 Prepositions 파싱
        while (index < parts.Count)
        {
            var part = parts[index];
            
            if (part.StartsWith("-"))
            {
                // Preposition
                var prepName = part.TrimStart('-');
                index++;
                
                // 전치사 블록 파싱 (중괄호 또는 값)
                string? value = null;
                string? block = null;
                
                if (index < parts.Count)
                {
                    var nextPart = parts[index];
                    if (nextPart.StartsWith("{"))
                    {
                        // 블록 파싱
                        block = ParseBlock(parts, ref index);
                    }
                    else
                    {
                        value = nextPart;
                        index++;
                    }
                }
                
                result.Prepositions[prepName] = new PrepositionBlock
                {
                    Name = prepName,
                    Value = value,
                    Block = block
                };
            }
            else
            {
                // Value
                result.Values.Add(part);
                index++;
            }
        }

        return result;
    }

    public IEnumerable<string> SplitPipeline(string commandLine)
    {
        // 파이프(|)로 분리하되, 중괄호 내부의 파이프는 무시
        var parts = new List<string>();
        var current = new StringBuilder();
        int braceDepth = 0;
        
        for (int i = 0; i < commandLine.Length; i++)
        {
            char c = commandLine[i];
            
            if (c == '{')
                braceDepth++;
            else if (c == '}')
                braceDepth--;
            else if (c == '|' && braceDepth == 0)
            {
                parts.Add(current.ToString().Trim());
                current.Clear();
                continue;
            }
            
            current.Append(c);
        }
        
        if (current.Length > 0)
            parts.Add(current.ToString().Trim());
        
        return parts;
    }

    private string RemoveComments(string commandLine)
    {
        var lines = commandLine.Split('\n');
        var result = new StringBuilder();
        
        foreach (var line in lines)
        {
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("//") || trimmed.StartsWith("##") || 
                trimmed.StartsWith("cmt", StringComparison.OrdinalIgnoreCase))
                continue;
            
            result.AppendLine(line);
        }
        
        return result.ToString().TrimEnd();
    }

    private List<string> Tokenize(string commandLine)
    {
        var tokens = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;
        char quoteChar = '"';
        int braceDepth = 0;
        
        for (int i = 0; i < commandLine.Length; i++)
        {
            char c = commandLine[i];
            
            if (!inQuotes && (c == '"' || c == '\''))
            {
                inQuotes = true;
                quoteChar = c;
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                continue;
            }
            
            if (inQuotes && c == quoteChar)
            {
                inQuotes = false;
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                continue;
            }
            
            if (inQuotes)
            {
                current.Append(c);
                continue;
            }
            
            if (c == '{')
            {
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                braceDepth++;
                current.Append(c);
                continue;
            }
            
            if (c == '}')
            {
                current.Append(c);
                braceDepth--;
                if (braceDepth == 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                continue;
            }
            
            if (braceDepth > 0)
            {
                current.Append(c);
                continue;
            }
            
            if (char.IsWhiteSpace(c))
            {
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
                continue;
            }
            
            current.Append(c);
        }
        
        if (current.Length > 0)
            tokens.Add(current.ToString());
        
        return tokens;
    }

    private string ParseBlock(List<string> parts, ref int index)
    {
        if (index >= parts.Count)
            return string.Empty;
        
        var block = parts[index];
        if (!block.StartsWith("{"))
            return string.Empty;
        
        // 중괄호 제거
        block = block.TrimStart('{').TrimEnd('}');
        index++;
        return block;
    }

    private bool IsPermission(string part)
    {
        return part.Equals("root", StringComparison.OrdinalIgnoreCase) ||
               part.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
               part.Equals("user", StringComparison.OrdinalIgnoreCase);
    }

    private Permission ParsePermission(string part)
    {
        return part.ToLower() switch
        {
            "root" => Permission.Root,
            "admin" => Permission.Admin,
            "user" => Permission.User,
            _ => Permission.User
        };
    }
}
