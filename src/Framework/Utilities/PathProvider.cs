namespace Framework.Utilities;

public interface IPathProvider
{
    string MapPath(string relativePath);
}

public class PathProvider : IPathProvider
{
    public string MapPath(string relativePath)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), relativePath);
    }
}

