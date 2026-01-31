namespace Module.Environment;

internal static partial class Environmental
{
  internal static string WinDir(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.Windows,
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
  ) => GetFullPath(
    Normalize(
      subpath
    ),
    Home(
      "code"
    )
  );
}
