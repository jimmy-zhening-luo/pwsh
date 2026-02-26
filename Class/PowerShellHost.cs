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

  internal static string FullPathCurrentLocationRelative(
    string path = ""
  )
  {
    using var ps = Create();

    var currentLocation = ps
      .AddCommand(
        @"Microsoft.PowerShell.Management\Get-Location"
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

    return Client.File.PathString.FullPathLocationRelative(
      currentLocation,
      path
    );
  }
}
