namespace Module.PC.Environment;

using static System.Environment;
using static FileSystem.Path.Normalizer;

internal static class Folder
{
  internal static string WinDir(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.UserProfile,
    subpath
  );

  internal static string Code(
    string subpath = ""
  ) => IO.Path.GetFullPath(
    Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
