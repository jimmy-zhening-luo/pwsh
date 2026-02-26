namespace Module.Client.Environment;

internal static class Env
{
  private static readonly Dictionary<System.Environment.SpecialFolder, string> folders = [];

  internal static string Get(
    string variable
  ) => System.Environment.GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  internal static string GetFolder(
    System.Environment.SpecialFolder folder,
    string subpath = ""
  )
  {
    if (
      !folders.TryGetValue(
        folder,
        out string? folderLocation
      )
    )
    {
      folderLocation = System.Environment.GetFolderPath(
        folder
      );
      folders[folder] = folderLocation;
    }

    return System.IO.Path.GetFullPath(
      FileSystem.PathString.Normalize(
        subpath
      ),
      folderLocation
    );
  }
}
