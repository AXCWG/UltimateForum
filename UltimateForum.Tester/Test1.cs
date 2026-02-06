using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Xml;
using AngleSharp.Xml.Dom;
using AngleSharp.Xml.Parser;
using AXHelper.Extensions;

namespace UltimateForum.Tester;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
        var s = """
                Hello, my name is jinpaigongsi $$[icon id="smile"]$$ how are you $$[icon id="question"]$$ ??
                """;
        var extracted = s.Split(Regex.Matches(s, "\\$\\$\\[(.*?)\\]\\$\\$").Select(i=>i.Value).ToArray(), StringSplitOptions.None);
        var split = Regex.Split(s, "\\$\\$\\[(.*?)\\]\\$\\$"); 
        
        Debug.WriteLine(extracted.Serialize());
        Debug.WriteLine(split.Serialize());
    }

    [TestMethod]
    public void T2()
    {
        var s = """
                $$[icon id="smile"]$$$$[icon id="question"]$$
                """;
        
        Debug.WriteLine(SplitWithDelimiter(s, "\\$\\$\\[(.*?)\\]\\$\\$").Serialize());
    }
    public List<string> SplitWithDelimiter(string source, string regex)
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

    [TestMethod]
    public void Random()
    {
        IEnumerable<double> Doubles()
        {
            for (var i = 0; i < 1000000000; i++)
                yield return System.Random.Shared.NextSingle(0, 5);
        }
        Console.WriteLine(Doubles().Any(i=>(int)i == 5));
    }
}

public static class Ext
{
    extension (Random r)
    {
        public double NextSingle(double min, double max)
        {
            return r.NextSingle()  * (max - min) + min;
        }
    }
}