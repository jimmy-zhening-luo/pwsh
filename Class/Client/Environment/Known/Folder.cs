namespace Module.Client.Environment.Known;

using SpecialFolder = System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string subpath = ""
  ) => Env.GetFolder(
    SpecialFolder.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Env.GetFolder(
    SpecialFolder.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Env.GetFolder(
    SpecialFolder.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Env.GetFolder(
    SpecialFolder.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Env.GetFolder(
    SpecialFolder.UserProfile,
    subpath
  );
}
