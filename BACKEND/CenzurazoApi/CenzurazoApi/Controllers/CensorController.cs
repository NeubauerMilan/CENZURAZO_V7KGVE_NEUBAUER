using CenzurazoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CenzurazoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CensorController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] CensorRequest request)
    {
        var blacklist = ParseBlacklist(request.BlacklistText);
        var modifiedText = CensorText(request.InputText, blacklist);

        var response = new CensorResponse
        {
            ModifiedText = modifiedText,
            OriginalFrequencies = new List<WordFrequency>(),
            ModifiedFrequencies = new List<WordFrequency>()
        };

        return Ok(response);
    }

    private Dictionary<string, List<string>> ParseBlacklist(string raw)
    {
        var blacklist = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var line in lines)
        {
            var parts = line.Split('@');
            if (parts.Length != 2) continue;

            var key = parts[0].Trim();
            var values = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            if (!string.IsNullOrWhiteSpace(key) && values.Any())
            {
                blacklist[key] = values;
            }
        }

        return blacklist;
    }

    private string MatchCasing(string original, string replacement)
    {
        if (string.IsNullOrEmpty(original)) return replacement;

        if (original.All(char.IsUpper))
            return replacement.ToUpper();
        if (char.IsUpper(original[0]))
            return char.ToUpper(replacement[0]) + replacement[1..];

        return replacement.ToLower();
    }
    private string CensorText(string text, Dictionary<string, List<string>> blacklist)
    {
        var rng = new Random();
        var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(' ');
            var usedAlternatives = new HashSet<string>();
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i].Trim(' ', ',', '.', '!', '?', ';', ':', '"', '\'', '(', ')');
                var prefix = words[i].Substring(0, words[i].IndexOf(word, StringComparison.OrdinalIgnoreCase));
                var suffix = words[i].Substring(word.Length + prefix.Length);

                var key = blacklist.Keys.FirstOrDefault(k => string.Equals(k, word, StringComparison.OrdinalIgnoreCase));

                if (key != null)
                {
                    var alternatives = blacklist[key];
                    var available = alternatives.Except(usedAlternatives).ToList();
                    if (!available.Any()) available = alternatives;

                    var replacement = available[rng.Next(available.Count)];
                    usedAlternatives.Add(replacement);

                    var matched = MatchCasing(word, replacement);
                    words[i] = $"{prefix}({word}|{matched}){suffix}";
                }
            }
            result.Add(string.Join(" ", words).Trim());
        }

        return string.Join(". ", result) + (text.EndsWith(".") ? "." : "");
    }

}
