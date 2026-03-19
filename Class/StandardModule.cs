namespace PowerModule;

static class StandardModule
{
  internal const string Management = "Microsoft.PowerShell.Management";
  internal const string Utility = "Microsoft.PowerShell.Utility";

  static internal string GetPowerShellHostLocation()
  {
    using var ps = PowerShell.Create(
      RunspaceMode.CurrentRunspace
    );

    var currentLocation = ps
      .AddCommand(
        $@"{Management}\Get-Location"
      )
      .Invoke()[default]
      .BaseObject
      .ToString();

    System.ArgumentException.ThrowIfNullOrEmpty(
      currentLocation,
      "PowerShell host current location"
    );

    return currentLocation;
  }
}
