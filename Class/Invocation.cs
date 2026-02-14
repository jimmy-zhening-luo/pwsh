namespace Module;

using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

internal static class Invocation
{
  internal static void CreateProcess(
    string fileName,
    string argument,
    bool noNewWindow = false
  ) => CreateProcess(
    fileName,
    [argument],
    noNewWindow
  );

  internal static void CreateProcess(
    string fileName,
    IEnumerable<string>? arguments = null,
    bool noNewWindow = false
  )
  {
    System.Diagnostics.Process.Start(
      ArgumentList(
        fileName,
        arguments,
        noNewWindow
      )
    );
  }

  internal static void ShellExecute(
    string fileName,
    string argument,
    bool administrator = false,
    bool noNewWindow = false
  ) => ShellExecute(
    fileName,
    [argument],
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
    var startInfo = ArgumentList(
      fileName,
      arguments,
      noNewWindow
    );

    startInfo.UseShellExecute = true;

    if (administrator)
    {
      startInfo.Verb = "RunAs";
    }

    System.Diagnostics.Process.Start(
      startInfo
    );
  }

  private static ProcessStartInfo ArgumentList(
    string fileName,
    IEnumerable<string>? arguments,
    bool noNewWindow
  )
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

    return startInfo;
  }
}
