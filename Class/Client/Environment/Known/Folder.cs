namespace Module.Client.Environment.Known;

using FolderName = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string subpath = ""
  ) => Local.GetFolder(
    FolderName.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Local.GetFolder(
    FolderName.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Local.GetFolder(
    FolderName.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Local.GetFolder(
    FolderName.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Local.GetFolder(
    FolderName.UserProfile,
    subpath
  );
}
