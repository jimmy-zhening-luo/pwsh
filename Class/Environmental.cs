namespace Module;

internal static class Environmental
{
  internal static bool Ssh => ssh ??= !string.IsNullOrEmpty(
    Env(
      "SSH_CLIENT"
    )
  );
  private static string? ssh;

  internal static string Env(
    string variable,
    EnvironmentVariableTarget target = EnvironmentVariableTarget.Process
  ) => GetEnvironmentVariable(
    variable,
    target
  )
    ?? string.Empty;

  internal static string EnvPath(
    SpecialFolder folder,
    string subpath = ""
  ) => GetFullPath(
    Normalize(
      subpath
    ),
    GetFolderPath(
      folder
    )
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

  internal static string Home(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.UserProfile,
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

  internal static string WinDir(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.Windows,
    subpath
  );
}
