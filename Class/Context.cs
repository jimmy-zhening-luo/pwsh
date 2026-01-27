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

  internal static string Env(
    string variable,
    EnvironmentVariableTarget target = EnvironmentVariableTarget.Process
  ) => GetEnvironmentVariable(
    variable,
    target
  )
    ?? string.Empty;

  internal static string EnvironmentPath(
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

  internal static PowerShell CreatePS(
    bool newRunspace = false
  ) => PowerShell.Create(
    newRunspace
      ? RunspaceMode.NewRunspace
      : RunspaceMode.CurrentRunspace
  );

  internal static string PSLocation(
    string path = ""
  )
  {
    using var ps = CreatePS();

    return GetFullPath(
      path,
      ps
        .AddCommand(
          "Get-Location"
        )
        .Invoke()[0]
        .BaseObject
        .ToString()
        ?? string.Empty
    );
  }
}
