using System.Text.RegularExpressions;
using AXHelper.Extensions;
using Microsoft.EntityFrameworkCore;
using UltimateForum.Db.Models;

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

    public static long GetQuoteIdFromTag(string tag)
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

    public static string GetIconIdFromTag(string tag)
    {
        var ts = tag.ToLowerInvariant().Split(" ", StringSplitOptions.TrimEntries);
        foreach (var se in ts.Where(i=>i.Contains('=')))
        {
            var tt = se.Split(['\"', '\''], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tt.Length; i++)
            {
                if (tt[i] == "ico=")
                {
                    return tt[i+1];
                }
                
            }
            
        }

        throw new InvalidOperationException();
    }

    public static string GenericTemplateReplace(string source, string regex, params (string keyInSource, string value)[] replacements)
    {
        var templates = Regex.Matches(source, regex).Select(i => i.Value).ToList(); 
        var variables = new List<string>(templates);
        for (int i = 0; i < variables.Count; i++)
        {
            variables[i] = variables[i].Replace("{{", "");
            variables[i] = variables[i].Replace("}}", "");
        }

        variables = variables.Select(i => i.Trim()).ToList(); 
        var res = ""; 
        res = source; 
        for (var index = 0; index < templates.Count; index++)
        {
            
            var template = templates[index];
            res = res.Replace(template, replacements.FirstOrDefault(i => i.keyInSource == variables[index]).value);
        }

        res=  res.Replace("\\{", "{");
        res = res.Replace("\\}", "}");
        return res; 
    }

    public static List<string> SplitWithDelimiter(string source, string regex)
    {
        var m = Regex.Matches(source, regex).Select(i => i).ToList();
        if (m.Count == 0)
        {
            return [source];
        }
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
            start = res.Sum(i=>i.Length) ;
        }
        Console.WriteLine(res.Serialize());

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

    extension(DbSet<User> users)
    {
        public bool UserAllowedToDoAction(long? uid, bool anonymousAllow)
        {
            if (uid is null &&anonymousAllow)
            {
                return true;
            }

            return users.Any(i => i.Id == uid);
        }
    }
}
