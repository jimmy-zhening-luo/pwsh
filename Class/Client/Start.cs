namespace Module.Client;

internal static class Start
{
  internal static void CreateProcess(
    string fileName,
    bool window = default
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      CreateNoWindow = !window,
    }
  );
  internal static void CreateProcess(
    string fileName,
    string argument,
    bool window = default
  ) => CreateProcess(
    fileName,
    [argument],
    window
  );
  internal static void CreateProcess(
    string fileName,
    IList<string> arguments,
    bool window = default
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(
      fileName,
      arguments
    )
    {
      CreateNoWindow = !window,
    }
  );

  internal static void ShellExecute(string fileName) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      UseShellExecute = true,
    }
  );
  internal static void ShellExecute(
    string fileName,
    bool administrator,
    bool noWindow = default
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      UseShellExecute = true,
      Verb = administrator
        ? "RunAs"
        : string.Empty,
      CreateNoWindow = noWindow,
    }
  );
  internal static void ShellExecute(
    string fileName,
    string argument,
    bool administrator = default,
    bool noWindow = default
  ) => ShellExecute(
    fileName,
    [argument],
    administrator,
    noWindow
  );
  internal static void ShellExecute(
    string fileName,
    IList<string> arguments,
    bool administrator = default,
    bool noWindow = default
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(
      fileName,
      arguments
    )
    {
      UseShellExecute = true,
      Verb = administrator
        ? "RunAs"
        : string.Empty,
      CreateNoWindow = noWindow,
    }
  );
}
