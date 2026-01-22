namespace UltimateForum.Razor;

public class OpenMojiIconPackHelperService : IIconPackHelperService
{
    private readonly Dictionary<string, string> _packs = new(); 

    public OpenMojiIconPackHelperService(IWebHostEnvironment webHostEnvironment)
    {
        var e = Directory.EnumerateFiles(Path.Join(webHostEnvironment.WebRootPath, "assets","icons","openmoji-72x72-color")).OrderByDescending(i=>i).ToArray();
        var p = e.Select(i => Path.GetFileNameWithoutExtension(i)?? throw new FileLoadException("How could a file does not have a name???")).ToArray() ;
        var n = e.Select(i => Path.GetRelativePath(webHostEnvironment.WebRootPath, i)).ToArray(); 
        if (e.Length != p.Length) throw new FileLoadException("Something very weird is happening right now. "); 
        for (int i = 0; i < p.Length; i++)
        {
            _packs.Add(p[i], n[i]);
        }
    }

    public Dictionary<string, string> GetAllIcons()
    {
        return _packs;
    }

    public string GetIcon(string name)
    {
        return _packs.FirstOrDefault(i => i.Key.ToLowerInvariant() == name.ToLowerInvariant()).Value ?? throw new KeyNotFoundException();
    }
}

public interface IIconPackHelperService
{
    Dictionary<string, string> GetAllIcons();
}