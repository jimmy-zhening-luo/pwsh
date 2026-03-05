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
  ) => StartWithArguments(
    fileName,
    !window,
    default,
    default
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
  ) => StartWithArguments(
    fileName,
    arguments,
    !window,
    default,
    default
  );

  internal static void ShellExecute(string fileName) => ShellExecute(
    fileName,
    default(bool),
    default
  );
  internal static void ShellExecute(
    string fileName,
    bool administrator
  ) => ShellExecute(
    fileName,
    administrator,
    default
  );
  internal static void ShellExecute(
    string fileName,
    bool administrator,
    bool noWindow
  ) => StartWithArguments(
    fileName,
    noWindow,
    administrator,
    true
  );
  internal static void ShellExecute(
    string fileName,
    string argument
  ) => ShellExecute(
    fileName,
    [argument],
    default,
    default
  );
  internal static void ShellExecute(
    string fileName,
    IList<string> arguments
  ) => ShellExecute(
    fileName,
    arguments,
    default,
    default
  );
  internal static void ShellExecute(
    string fileName,
    string argument,
    bool administrator
  ) => ShellExecute(
    fileName,
    [argument],
    administrator,
    default
  );
  internal static void ShellExecute(
    string fileName,
    IList<string> arguments,
    bool administrator
  ) => ShellExecute(
    fileName,
    arguments,
    administrator,
    default
  );
  internal static void ShellExecute(
    string fileName,
    string argument,
    bool administrator,
    bool noWindow
  ) => ShellExecute(
    fileName,
    [argument],
    administrator,
    noWindow
  );
  internal static void ShellExecute(
    string fileName,
    IList<string> arguments,
    bool administrator,
    bool noWindow
  ) => StartWithArguments(
    fileName,
    arguments,
    noWindow,
    administrator,
    true
  );

  private static void StartWithArguments(
    string fileName,
    bool noWindow,
    bool administrator,
    bool shellExecute
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      UseShellExecute = shellExecute,
      Verb = administrator
        ? "RunAs"
        : string.Empty,
      CreateNoWindow = noWindow,
    }
  );
  private static void StartWithArguments(
    string fileName,
    IList<string> arguments,
    bool noWindow,
    bool administrator,
    bool shellExecute
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName, arguments)
    {
      UseShellExecute = shellExecute,
      Verb = administrator
        ? "RunAs"
        : string.Empty,
      CreateNoWindow = noWindow,
    }
  );
}
