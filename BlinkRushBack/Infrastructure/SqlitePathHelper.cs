namespace BlinkRush.Infrastructure;

public static class SqlitePathHelper
{
    /// <summary>
    /// Résout la chaîne de connexion SQLite depuis la config et assure l’existence du répertoire parent.
    /// </summary>
    public static string ResolveConnectionString(IConfiguration configuration, string contentRootPath)
    {
        var raw = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

        var pathPart = raw.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase)
            ? raw["Data Source=".Length..].Trim()
            : raw.Trim();

        var fullPath = Path.IsPathRooted(pathPart)
            ? pathPart
            : Path.GetFullPath(Path.Combine(contentRootPath, pathPart));

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        return $"Data Source={fullPath}";
    }
}
