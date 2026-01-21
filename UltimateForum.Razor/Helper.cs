using System.Text.RegularExpressions;

namespace UltimateForum.Razor;

public static class Helper
{
    public enum TagType
    {
        Quote, Icon, None
    }

    public static TagType Evaluate(string tag)
    {
        if (tag.ToLowerInvariant().Contains("!quote"))
        {
            return TagType.Quote;
        }

        if (tag.ToLowerInvariant().Contains("!icon"))
        {
            return TagType.Icon;
        }

        return TagType.None;
    }

    public static long GetQuoteId(string tag)
    {
        var ts = tag.ToLowerInvariant().Split(" ", StringSplitOptions.TrimEntries);
        foreach (var se in ts.Where(i=>i.Contains('=')))
        {
            var tt = se.Split(['\"', '\''], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tt.Length; i++)
            {
                if (tt[i] == "content=")
                {
                    try
                    {
                        return long.Parse(tt[i + 1]);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
                
            }
            
        }

        throw new InvalidOperationException();
    }

    public static List<string> SplitWithDelimiter(string source, string regex)
    {
        var m = Regex.Matches(source, regex).Select(i => i).ToList();
        var res = new List<string>();
        int start = 0;
        for (var i =0; i < m.Count(); i++)
        {
            var match = m[i];
            string s = source[start.. match.Index];
            if (s.Length != 0)
            {
                res.Add(s);
            }
            res.Add(match.Value);
            string e = source[(match.Index + match.Length ) .. (m.ElementAtOrDefault(i + 1)?.Index ?? source.Length)];
            if (e.Length != 0)
            {
                res.Add(e);
            }
            start = source.IndexOf(res.Last(), StringComparison.InvariantCulture) + res.Last().Length;
        }

        return res;
    }
    public static List<string> SplitWithDelimiterTrim(string source, string regex)
    {
        var res = SplitWithDelimiter(source, regex);
        for (var i = 0; i < res.Count; i++)
        {
            res[i] =  res[i].Trim();
        }
        return res;
    }
}