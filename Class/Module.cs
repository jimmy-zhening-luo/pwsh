namespace Module;

static internal class Module
{
  static internal string FullPathCurrentLocationRelative(string path) => Client.File.PathString.FullPathLocationRelative(
    FullPathCurrentLocationRelative(),
    path
  );
  static internal string FullPathCurrentLocationRelative()
  {
    using var ps = PowerShell.Create(
      RunspaceMode.CurrentRunspace
    );

    var currentLocation = ps
      .AddCommand(
        @"Microsoft.PowerShell.Management\Get-Location"
      )
      .Invoke()[default]
      .BaseObject
      .ToString();

    System.ArgumentException.ThrowIfNullOrEmpty(currentLocation, nameof(currentLocation));

    return currentLocation;
  }
}
