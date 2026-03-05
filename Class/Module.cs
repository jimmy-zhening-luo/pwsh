namespace Module;

internal static class Module
{
  internal static PowerShell Create() => PowerShell.Create(RunspaceMode.CurrentRunspace);

  internal static string FullPathCurrentLocationRelative(string path) => Client.File.PathString.FullPathLocationRelative(
    FullPathCurrentLocationRelative(),
    path
  );
  internal static string FullPathCurrentLocationRelative()
  {
    using var ps = Create();

    var currentLocation = ps
      .AddCommand(
        @"Microsoft.PowerShell.Management\Get-Location"
      )
      .Invoke()[default]
      .BaseObject
      .ToString();

    if (currentLocation is null or "")
    {
      throw new System.InvalidOperationException(
        "Failed to get the current location of the PowerShell host."
      );
    }

    return currentLocation;
  }
}
