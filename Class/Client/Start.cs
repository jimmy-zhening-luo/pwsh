namespace Module.Client;

internal static class Start
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
    IEnumerable<string>? arguments = default,
    bool noNewWindow = default
  ) => System.Diagnostics.Process.Start(
    ArgumentList(
      fileName,
      arguments,
      noNewWindow
    )
  );

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
    IEnumerable<string>? arguments = default,
    bool administrator = default,
    bool noNewWindow = default
  ) => System.Diagnostics.Process.Start(
    ArgumentList(
      fileName,
      arguments,
      noNewWindow,
      true,
      administrator
    )
  );

  private static System.Diagnostics.ProcessStartInfo ArgumentList(
    string fileName,
    IEnumerable<string>? arguments,
    bool noNewWindow,
    bool shellExecute = default,
    bool administrator = default
  )
  {
    List<string> argumentList = [
      .. arguments ?? [],
    ];

    System.Diagnostics.ProcessStartInfo startInfo = argumentList is []
      ? new(fileName)
      : new(
          fileName,
          argumentList
        );

    if (noNewWindow)
    {
      startInfo.CreateNoWindow = true;
    }

    if (shellExecute)
    {
      startInfo.UseShellExecute = true;
    }

    if (administrator)
    {
      startInfo.Verb = "RunAs";
    }

    return startInfo;
  }
}
