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

    var currentLocation = ps
      .AddCommand(
        CommandInvocationIntrinsics.GetCommand(
          @"Microsoft.PowerShell.Management\Get-Location",
          CommandTypes.Cmdlet
        )
      )
      .Invoke()[0]
      .BaseObject
      .ToString();

    if (currentLocation is null or "")
    {
      throw new System.InvalidOperationException(
        "Failed to get current location of PowerShell host."
      );
    }

    return System.IO.Path.GetFullPath(
      path,
      currentLocation
    );
  }
}
