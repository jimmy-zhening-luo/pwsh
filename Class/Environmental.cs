namespace Module;

internal static class Environmental
{
  internal static bool Ssh
  {
    get => !string.IsNullOrEmpty(
      Env(
        "SSH_CLIENT"
      )
    );
  }

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

  internal static string Windir(
    string subpath = ""
  ) => EnvPath(
    SpecialFolder.Windows,
    subpath
  );
}
