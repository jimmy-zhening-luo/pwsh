namespace Module;

public static class Context
{
  public static bool Ssh
  {
    get => !string.IsNullOrEmpty(
      Env("SSH_CLIENT")
    );
  }

  public static PowerShell PS() => PowerShell.Create(
    CurrentRunspace
  );

  public static string Env(
    string variable
  ) => Environment.GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  public static string Home(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    Environment.GetFolderPath(
      Environment
        .SpecialFolder
        .UserProfile
    )
  );

  public static string AppData(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
    Env("APPDATA")
  );

  public static string LocalAppData(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
    Env("LOCALAPPDATA")
  );

  public static void CreateProcess(
    string fileName,
    string arguments = "",
    bool noNewWindow = false
  )
  {
    if (!Ssh)
    {
      var startInfo = new ProcessStartInfo(
        fileName
      )
      {
        CreateNoWindow = noNewWindow
      };

      if (
        !string.IsNullOrEmpty(
          arguments
        )
      )
      {
        startInfo.Arguments = arguments;
      }

      Process.Start(
        startInfo
      );
    }
  }

  public static void ShellExecute(
    string fileName,
    string arguments = "",
    bool administrator = false,
    bool noNewWindow = false
  )
  {
    if (!Ssh)
    {
      var startInfo = new ProcessStartInfo(
        fileName
      )
      {
        UseShellExecute = true,
        CreateNoWindow = noNewWindow
      };

      if (
        !string.IsNullOrEmpty(
          arguments
        )
      )
      {
        startInfo.Arguments = arguments;
      }

      if (administrator)
      {
        startInfo.Verb = "RunAs";
      }

      Process.Start(
        startInfo
      );
    }
  }
}
