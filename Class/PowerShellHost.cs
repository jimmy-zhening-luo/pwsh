namespace Module;

internal static class PowerShellHost
{
  internal static PowerShell Create(
    bool newRunspace = default
  ) => PowerShell.Create(
    newRunspace
      ? RunspaceMode.NewRunspace
      : RunspaceMode.CurrentRunspace
  );

  internal static string CurrentDirectory(
    string path = ""
  )
  {
    using var ps = Create();

    return System.IO.Path.GetFullPath(
      path,
      ps
        .AddCommand(
          "Get-Location"
        )
        .Invoke()[0]
        .BaseObject
        .ToString()
        ?? string.Empty
    );
  }
}
