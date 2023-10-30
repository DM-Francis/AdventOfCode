namespace Day19;

public interface IRule
{
    int? Id { get; }
    MatchResult Match(string message);
}

public class ExactCharacter : IRule
{
    public int? Id { get; init; }
    public char Value { get; init; }

    public MatchResult Match(string message)
    {
        if (string.IsNullOrEmpty(message) || message[0] != Value)
            return new MatchResult(false, null);

        return new MatchResult(true, message[1..]);
    }
}

public class Combined : IRule
{
    public int? Id { get; init; }
    public List<IRule> SubRules { get; init; } = new();

    public MatchResult Match(string message)
    {
        string current = message;
        foreach (var rule in SubRules)
        {
            var matchResult = rule.Match(current);
            if (!matchResult.IsMatch)
                return new MatchResult(false, null);

            current = matchResult.TextAfterMatch ??
                      throw new InvalidOperationException(
                          $"{nameof(matchResult.TextAfterMatch)} should not be nul here.");
        }

        return new MatchResult(true, current);
    }
}

public class Or : IRule
{
    public int? Id { get; init; }
    public List<IRule> SubRules { get; init; } = new();

    public MatchResult Match(string message)
    {
        foreach (var rule in SubRules)
        {
            var matchResult = rule.Match(message);
            if (matchResult.IsMatch)
                return matchResult;
        }

        return new MatchResult(false, null);
    }
}

public class RuleZero : IRule
{
    public int? Id => 0;

    public IRule? StartRule { get; set; }
    public IRule? EndRule { get; set; }
    
    public MatchResult Match(string message)
    {
        // To be a match, a string much match the StartRule M times and then the EndRule N times.
        // It is then a match is M > N and N > 0
        // For example if the StartRule = "a", and the EndRule = "b", then aab, aaabb, aaab, aaaaaabbb are all valid
        // but aaabbb and aabbbb are not valid

        if (StartRule is null || EndRule is null)
            throw new InvalidOperationException("Both StartRule and EndRule must be set before matching");
        
        string current = message;
        int m = 0;
        int n = 0;
        MatchResult result;
        do
        {
            result = StartRule.Match(current);
            if (result.IsMatch)
            {
                m++;
                current = result.TextAfterMatch ?? throw new InvalidOperationException("Text should not be null here");
            }
        } while (result.IsMatch);

        if (m == 0)
            return new MatchResult(false, null);

        do
        {
            result = EndRule.Match(current);
            if (result.IsMatch)
            {
                n++;
                current = result.TextAfterMatch ?? throw new InvalidOperationException("Text should not be null here");
            }
        } while (result.IsMatch);

        if (m > n && n > 0)
            return new MatchResult(true, current);

        return new MatchResult(false, null);
    }
}

public record MatchResult(bool IsMatch, string? TextAfterMatch);