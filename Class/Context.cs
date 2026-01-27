using System.Linq;

namespace Module;

internal static class Context
{
  internal static void CreateProcess(
    string fileName,
    string arguments,
    bool noNewWindow = false
  ) => CreateProcess(
    fileName,
    [arguments],
    noNewWindow
  );

  internal static void CreateProcess(
    string fileName,
    IEnumerable<string>? arguments = null,
    bool noNewWindow = false
  )
  {
    if (!Ssh)
    {
      var argumentList = arguments == null
        ? null
        : new List<string>(
            arguments
          );

      var startInfo = argumentList == null
        || argumentList.Count == 0
        ? new ProcessStartInfo(
            fileName
          )
        : new ProcessStartInfo(
            fileName,
            argumentList
          );

      if (noNewWindow)
      {
        startInfo.CreateNoWindow = true;
      }

      Process.Start(
        startInfo
      );
    }
  }

  internal static void ShellExecute(
    string fileName,
    string arguments,
    bool administrator = false,
    bool noNewWindow = false
  ) => ShellExecute(
    fileName,
    [arguments],
    administrator,
    noNewWindow
  );

  internal static void ShellExecute(
    string fileName,
    IEnumerable<string>? arguments = null,
    bool administrator = false,
    bool noNewWindow = false
  )
  {
    if (!Ssh)
    {
      var argumentList = arguments == null
        ? null
        : new List<string>(
            arguments
          );

      var startInfo = argumentList == null
        || argumentList.Count == 0
        ? new ProcessStartInfo(
            fileName
          )
        : new ProcessStartInfo(
            fileName,
            argumentList
          );

      startInfo.UseShellExecute = true;

      if (noNewWindow)
      {
        startInfo.CreateNoWindow = true;
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
