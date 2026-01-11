namespace Module
{
  using System;
  using System.IO;
  using System.Diagnostics;

  public static class Context
  {
    public static string Env(string variable) => Environment.GetEnvironmentVariable(variable) ?? string.Empty;

    public static bool Ssh() => Env("SSH_CLIENT") != string.Empty;

    public static string Home() => Environment.GetFolderPath(
      Environment
        .SpecialFolder
        .UserProfile
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
      if (!Ssh())
      {
        var startInfo = new ProcessStartInfo(fileName)
        {
          CreateNoWindow = noNewWindow
        };

        if (arguments != string.Empty)
        {
          startInfo.Arguments = arguments;
        }

        Process.Start(startInfo);
      }
    }

    public static void ShellExecute(
      string fileName,
      string arguments = "",
      bool runAsAdmin = false,
      bool noNewWindow = false
    )
    {
      if (!Ssh())
      {
        var startInfo = new ProcessStartInfo(fileName)
        {
          UseShellExecute = true,
          CreateNoWindow = noNewWindow
        };

        if (arguments != string.Empty)
        {
          startInfo.Arguments = arguments;
        }

        if (runAsAdmin)
        {
          startInfo.Verb = "RunAs";
        }

        Process.Start(startInfo);
      }
    }
  }
}
