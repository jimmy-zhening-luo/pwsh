namespace Module;

internal static class CommandLine
{
  internal static PowerShell Create(
    bool newRunspace = false
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

    return IO.Path.GetFullPath(
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
