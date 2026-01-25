namespace Module;

internal static class Context
{
  internal static bool Ssh
  {
    get => !string.IsNullOrEmpty(
      Env(
        "SSH_CLIENT"
      )
    );
  }

  internal static PowerShell CreatePS(
    bool newRunspace = false
  ) => PowerShell.Create(
    newRunspace
      ? RunspaceMode.NewRunspace
      : RunspaceMode.CurrentRunspace
  );

  internal static string Env(
    string variable
  ) => Environment.GetEnvironmentVariable(
    variable
  )
    ?? string.Empty;

  internal static string Home(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
    Environment.GetFolderPath(
      Environment
        .SpecialFolder
        .UserProfile
    )
  );

  internal static string AppData(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
    Env("APPDATA")
  );

  internal static string LocalAppData(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
    Env("LOCALAPPDATA")
  );

  internal static void CreateProcess(
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

  internal static void ShellExecute(
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
