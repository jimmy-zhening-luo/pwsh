namespace Module.Client.Environment;

internal static class Local
{
  private static readonly Dictionary<System.Environment.SpecialFolder, string> folders = [];

  internal static string Get(string variable) => System.Environment.GetEnvironmentVariable(variable)
    ?? string.Empty;

  internal static string GetFolder(
    System.Environment.SpecialFolder folder,
    string path = ""
  ) => File.PathString.FullPathLocationRelative(
    folders.TryGetValue(
      folder,
      out var folderLocation
    )
      ? folderLocation
      : folders[folder] = System.Environment.GetFolderPath(folder),
    path
  );
}
