namespace PowerModule.Client.Environment;

static internal class Local
{
  static private readonly Dictionary<System.Environment.SpecialFolder, string> folders = [];

  static internal string Get(string variable) => System.Environment.GetEnvironmentVariable(variable)
    ?? string.Empty;

  static internal string GetFolder(
    System.Environment.SpecialFolder folder,
    string path
  ) => File.PathString.GetFullPathLocal(
    GetFolder(folder),
    path
  );
  static internal string GetFolder(System.Environment.SpecialFolder folder) => folders.TryGetValue(
    folder,
    out var folderLocation
  )
    ? folderLocation
    : folders[folder] = System.Environment.GetFolderPath(folder);
}
