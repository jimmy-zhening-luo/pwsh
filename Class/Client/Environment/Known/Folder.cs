namespace Module.Client.Environment.Known;

using FolderName = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string subpath = ""
  ) => Environment.GetFolder(
    FolderName.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Environment.GetFolder(
    FolderName.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Environment.GetFolder(
    FolderName.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Environment.GetFolder(
    FolderName.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Environment.GetFolder(
    FolderName.UserProfile,
    subpath
  );
}
