namespace Module.Client;

internal static class Invocation
{
  internal static void CreateProcess(
    string fileName,
    string argument,
    bool noNewWindow = default
  ) => CreateProcess(
    fileName,
    [argument],
    noNewWindow
  );

  internal static void CreateProcess(
    string fileName,
    IEnumerable<string>? arguments = null,
    bool noNewWindow = default
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
    bool administrator = default,
    bool noNewWindow = default
  ) => ShellExecute(
    fileName,
    [argument],
    administrator,
    noNewWindow
  );

  internal static void ShellExecute(
    string fileName,
    IEnumerable<string>? arguments = null,
    bool administrator = default,
    bool noNewWindow = default
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

  private static System.Diagnostics.ProcessStartInfo ArgumentList(
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
      ? new System.Diagnostics.ProcessStartInfo(
          fileName
        )
      : new System.Diagnostics.ProcessStartInfo(
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
