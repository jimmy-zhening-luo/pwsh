namespace Module.Environment.Environmental;

using static System.Environment.SpecialFolder;

internal static partial class Environmental
{
  internal static string WinDir(
    string subpath = ""
  ) => EnvPath(
    Windows,
    subpath
  );

  internal static string ProgramFiles(
    string subpath = ""
  ) => EnvPath(
    using::ProgramFiles,
    subpath
  );

  internal static string AppData(
    string subpath = ""
  ) => EnvPath(
    ApplicationData,
    subpath
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => EnvPath(
    LocalApplicationData,
    subpath
  );

  internal static string Home(
    string subpath = ""
  ) => EnvPath(
    UserProfile,
    subpath
  );

  internal static string Code(
    string subpath = ""
  ) => GetFullPath(
    Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
