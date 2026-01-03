using System;
using System.Diagnostics;

namespace Core
{
  public static class Context
  {
    public static bool Ssh() => Environment.GetEnvironmentVariable(
      "SSH_CLIENT"
    ) != null;

    public static void Start(
      string fileName,
      string arguments = "",
      bool runAsAdmin = false,
      bool noShellExecute = false
    )
    {
      if (!Ssh())
      {
        var startInfo = new ProcessStartInfo(fileName);

        if (arguments != string.Empty)
        {
          startInfo.Arguments = arguments;
        }

        if (runAsAdmin)
        {
          startInfo.UseShellExecute = true;
          startInfo.Verb = "RunAs";
        }

        if (!noShellExecute)
        {
          startInfo.UseShellExecute = true;
        }

        Process.Start(startInfo);
      }
    }
  }
}
