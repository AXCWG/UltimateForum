
using Microsoft.Data.Sqlite;

namespace UltimateForum;

public static class Helper
{
    extension(string[] source)
    {
        public string[] RemoveLast()
        {
            return source[0..^1]; 
        }

        public string CombineWithPathSeparator()
        {
            return string.Join(Path.DirectorySeparatorChar, source);
        }
    }

    extension(string source)
    {
        public void CreateDirectoryOfDataSource()
        {
            var pathToCreate = new SqliteConnectionStringBuilder(source).DataSource
                .Split(['/', '\\'], StringSplitOptions.TrimEntries).RemoveLast().CombineWithPathSeparator(); 
            Directory.CreateDirectory(pathToCreate);
            
        }
    }
}