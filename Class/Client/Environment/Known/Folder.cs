namespace Module.Client.Environment.Known;

using static System.Environment.SpecialFolder;

internal static partial class Folder
{
  internal static string Windows(
    string subpath = ""
  ) => Env.GetFolder(
    Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Env.GetFolder(
    ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Env.GetFolder(
    ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Env.GetFolder(
    LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Env.GetFolder(
    UserProfile,
    subpath
  );
}
