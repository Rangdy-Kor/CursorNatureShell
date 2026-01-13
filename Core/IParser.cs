namespace NatureShell.Core;

/// <summary>
/// POS 기반 문법 파서 인터페이스
/// 구조: [Permission] [Noun:Adjective] [Verb!Adverb] [Value] [-Preposition] [Value]
/// </summary>
public interface IParser
{
    /// <summary>명령 라인을 파싱하여 CommandParseResult 반환</summary>
    CommandParseResult Parse(string commandLine);
    
    /// <summary>파이프라인(|)을 기준으로 명령 분리</summary>
    IEnumerable<string> SplitPipeline(string commandLine);
}
