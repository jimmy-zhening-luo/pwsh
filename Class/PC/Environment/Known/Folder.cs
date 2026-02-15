namespace Module.PC.Environment.Known;

using SpecialFolder = System.Environment.SpecialFolder;

internal static class Folder
{
  internal static string WinDir(
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

  internal static string Code(
    string subpath = ""
  ) => IO.Path.GetFullPath(
    FileSystem.Path.Normalizer.Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
