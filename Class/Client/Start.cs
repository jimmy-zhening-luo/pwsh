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
  )
  {
    _ = System.Diagnostics.Process.Start(
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
    IEnumerable<string>? arguments = default,
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

    _ = System.Diagnostics.Process.Start(startInfo);
  }

  private static System.Diagnostics.ProcessStartInfo ArgumentList(
    string fileName,
    IEnumerable<string>? arguments,
    bool noNewWindow
  )
  {
    List<string>? argumentList = arguments is null
      ? default
      : new(arguments);

    System.Diagnostics.ProcessStartInfo startInfo = argumentList is null or []
      ? new(fileName)
      : new(
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
