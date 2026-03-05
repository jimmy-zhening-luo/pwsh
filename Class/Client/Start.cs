namespace Module.Client;

internal static class Start
{
  internal static void CreateProcess(string fileName) => System.Diagnostics.Process.Start(
    new System.Diagnostics.ProcessStartInfo(fileName)
    {
      CreateNoWindow = true,
    }
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
