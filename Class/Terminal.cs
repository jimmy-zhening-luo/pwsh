namespace Module;

using System.Linq;

internal static class Terminal
{
  internal static PowerShell CreatePS(
    bool newRunspace = false
  ) => PowerShell.Create(
    newRunspace
      ? RunspaceMode.NewRunspace
      : RunspaceMode.CurrentRunspace
  );

  internal static string PSLocation(
    string path = ""
  )
  {
    using var ps = CreatePS();

    return GetFullPath(
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
