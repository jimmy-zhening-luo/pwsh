namespace Module
{
  using System;
  using System.Diagnostics;

  public static class Context
  {
    public static string Env(string variable) => Environment.GetEnvironmentVariable(variable) ?? string.Empty;

    public static bool Ssh() => Env("SSH_CLIENT") != string.Empty;

    public static string AppData() => Env("APPDATA");

    public static string LocalAppData() => Env("LOCALAPPDATA");

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
