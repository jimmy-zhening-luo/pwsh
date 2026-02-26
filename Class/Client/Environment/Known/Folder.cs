namespace Module.Client.Environment.Known;

using FolderName = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string subpath = ""
  ) => Env.GetFolder(
    FolderName.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Env.GetFolder(
    FolderName.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Env.GetFolder(
    FolderName.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Env.GetFolder(
    FolderName.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Env.GetFolder(
    FolderName.UserProfile,
    subpath
  );
}
