namespace Module.Client.Environment.Known;

using FolderName = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string path = ""
  ) => Local.GetFolder(
    FolderName.Windows,
    path
  );

  internal static string ProgramFiles(
    string path = ""
  ) => Local.GetFolder(
    FolderName.ProgramFiles,
    path
  );

  internal static string AppData(
    string path = ""
  ) => Local.GetFolder(
    FolderName.ApplicationData,
    path
  );

  internal static string LocalAppData(
    string path = ""
  ) => Local.GetFolder(
    FolderName.LocalApplicationData,
    path
  );

  internal static string Home(
    string path = ""
  ) => Local.GetFolder(
    FolderName.UserProfile,
    path
  );
}
