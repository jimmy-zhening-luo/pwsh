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
    IList<string>? arguments = default,
    bool noNewWindow = default
  ) => System.Diagnostics.Process.Start(
    ProcessStartOptions(
      fileName,
      arguments,
      noNewWindow,
      default,
      default
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
    IList<string>? arguments = default,
    bool administrator = default,
    bool noNewWindow = default
  ) => System.Diagnostics.Process.Start(
    ProcessStartOptions(
      fileName,
      arguments,
      noNewWindow,
      true,
      administrator
    )
  );

  private static System.Diagnostics.ProcessStartInfo ProcessStartOptions(
    string fileName,
    IList<string>? arguments,
    bool noNewWindow,
    bool shellExecute,
    bool administrator
  )
  {
    System.Diagnostics.ProcessStartInfo startInfo = arguments is null or [] or [""]
      ? new(fileName)
      : new(fileName, arguments);

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
