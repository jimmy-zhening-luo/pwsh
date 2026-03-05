namespace Module.Client;

internal static class Start
{
  internal static void CreateProcess(string fileName) => CreateProcess(
    fileName,
    default(bool)
  );
  internal static void CreateProcess(
    string fileName,
    bool window
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      CreateNoWindow = !window,
    }
  );
  internal static void CreateProcess(
    string fileName,
    string argument
  ) => CreateProcess(
    fileName,
    [argument],
    default
  );
  internal static void CreateProcess(
    string fileName,
    IList<string> arguments
  ) => CreateProcess(
    fileName,
    arguments,
    default
  );
  internal static void CreateProcess(
    string fileName,
    string argument,
    bool window
  ) => CreateProcess(
    fileName,
    [argument],
    window
  );
  internal static void CreateProcess(
    string fileName,
    IList<string> arguments,
    bool window
  )
  {
    switch (arguments)
    {
      case [] or [""]:
        CreateProcess(fileName, window);
        break;

      default:
        _ = System.Diagnostics.Process.Start(
          new System.Diagnostics.ProcessStartInfo(
            fileName,
            arguments
          )
          {
            CreateNoWindow = !window,
          }
        );
        break;
    }
  }

  internal static void ShellExecute(string fileName) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      UseShellExecute = true,
    }
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
    IList<string> arguments,
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
    IList<string> arguments,
    bool noNewWindow,
    bool shellExecute,
    bool administrator
  )
  {
    System.Diagnostics.ProcessStartInfo startInfo = arguments is [] or [""]
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
