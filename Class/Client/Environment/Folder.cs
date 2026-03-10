namespace PowerModule.Client.Environment;

static internal partial class Folder
{
  static private readonly Dictionary<System.Environment.SpecialFolder, string> folders = [];

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
