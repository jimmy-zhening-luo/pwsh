namespace PowerModule.Client;

static internal class Start
{
  static internal void CreateProcess(
    string fileName,
    bool window = default
  ) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      CreateNoWindow = !window,
    }
  );
  static internal void CreateProcess(
    string fileName,
    string argument,
    bool window = default
  ) => CreateProcess(
    fileName,
    [argument],
    window
  );
  static internal void CreateProcess(
    string fileName,
    IEnumerable<string> arguments,
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

  static internal void ShellExecute(string fileName) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      UseShellExecute = true,
    }
  );
  static internal void ShellExecute(
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
  static internal void ShellExecute(
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
  static internal void ShellExecute(
    string fileName,
    IEnumerable<string> arguments,
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
