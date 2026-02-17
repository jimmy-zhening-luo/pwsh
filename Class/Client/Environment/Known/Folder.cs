namespace Module.Client.Environment.Known;

internal static class Folder
{
  internal static string WinDir(
    string subpath = ""
  ) => Env.GetFolder(
    System.Environment.SpecialFolder.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => Env.GetFolder(
    System.Environment.SpecialFolder.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => Env.GetFolder(
    System.Environment.SpecialFolder.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Env.GetFolder(
    System.Environment.SpecialFolder.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => Env.GetFolder(
    System.Environment.SpecialFolder.UserProfile,
    subpath
  );

  internal static string Code(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    FileSystem.PathString.Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
