namespace PowerModule;

static class PowerModule
{
  static internal string GetPowerShellHostLocation()
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
