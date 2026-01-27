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
    subpath,
    GetFolderPath(
      folder
    )
  );

  internal static string Home(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    GetFolderPath(
      SpecialFolder.UserProfile
    )
  );

  internal static string AppData(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    GetFolderPath(
      SpecialFolder.ApplicationData
    )
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    GetFolderPath(
      SpecialFolder.LocalApplicationData
    )
  );
}
