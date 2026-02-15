namespace Module.PC.Environment.Environmental;

using static System.Environment;
using static Module.PC.FileSystem.Path.Normalizer;

internal static partial class Environmental
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
