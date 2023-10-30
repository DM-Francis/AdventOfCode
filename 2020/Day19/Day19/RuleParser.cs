namespace Day19;

public class RuleParser
{
    // private readonly Dictionary<int, IRule> _createdRules = new();
    private readonly Dictionary<int, string> _rawRules = new();

    public RuleParser(List<string> rawRules)
    {
        foreach (var rule in rawRules)
        {
            var split = rule.Split(": ");
            int id = int.Parse(split[0]);
            _rawRules[id] = split[1];
        }
    }
    
    public IRule ParseRule(string rawRule)
    {
        var split = rawRule.Split(": ");
        int id = int.Parse(split[0]);
        string ruleSection = split[1];

        return ParseRuleWithId(id, ruleSection, false);
    }

    private IRule ParseRuleWithId(int id, string ruleSection, bool isSubRule)
    {
        if (ruleSection.Contains('"'))
            return new ExactCharacter { Id = isSubRule ? null : id, Value = ruleSection[1] };

        if (ruleSection.Contains('|'))
            return ParseOrRule(id, ruleSection, isSubRule);

        return ParseCombined(id, ruleSection, isSubRule);
    }

    private IRule ParseOrRule(int id, string ruleSection, bool isSubRule)
    {
        var parts = ruleSection.Split(" | ");
        var subRules = new List<IRule>();
        foreach (var part in parts)
        {
            subRules.Add(ParseRuleWithId(id, part, true));
        }

        var newRule = new Or { Id = isSubRule ? null : id, SubRules = subRules };
        // _createdRules[id] = newRule;
        return newRule;
    }

    private IRule ParseCombined(int? id, string ruleSection, bool isSubRule)
    {
        var partIds = ruleSection.Split(' ').Select(int.Parse).ToList();
        var subRules = new List<IRule?>();
        
        foreach (int partId in partIds)
        {
             if (partId == id)
                subRules.Add(null); // Placeholder
             // else if (_createdRules.TryGetValue(partId, out var rule))
             //     subRules.Add(rule);
             else
             {
                 var newRule = ParseRuleWithId(partId, _rawRules[partId], false);
                 // _createdRules[partId] = newRule;
                 subRules.Add(newRule);
             }
        }

        if (subRules.Count == 1)
            return subRules[0] ??
                   throw new InvalidOperationException("Cannot have only a single subRule that is self-referential");

        var combined = new Combined { Id = isSubRule ? null : id, SubRules = subRules };
        int selfIndex = subRules.FindIndex(x => x is null);
        if (selfIndex >= 0)
            subRules[selfIndex] = combined;

        // _createdRules[id] = combined;
        return combined;
    }
}