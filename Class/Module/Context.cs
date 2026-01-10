namespace Module
{
  using System;
  using System.Diagnostics;

  public static class Context
  {
    public static bool Ssh() => Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;

    public static string Env(string variable) => Environment.GetEnvironmentVariable(variable) ?? string.Empty;

    public static void CreateProcess(
      string fileName,
      string arguments = ""
    )
    {
      if (!Ssh())
      {
        var startInfo = new ProcessStartInfo(fileName);

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
      bool runAsAdmin = false
    )
    {
      if (!Ssh())
      {
        var startInfo = new ProcessStartInfo(fileName)
        {
          UseShellExecute = true
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
