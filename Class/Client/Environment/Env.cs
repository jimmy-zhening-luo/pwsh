namespace Module.Client.Environment;

internal static partial class Env
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
    string? folderLocation;

    if(
      !folders.TryGetValue(
        folder,
        out folderLocation
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
