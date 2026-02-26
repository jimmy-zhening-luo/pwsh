namespace Module.Commands.Pwsh.Help.Type;

[Cmdlet(
  VerbsCommon.Get,
  "TypeAccelerator",
  HelpUri = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_type_accelerators"
)]
[Alias("types")]
[OutputType(typeof(string))]
public sealed class GetTypeAccelerator : Cmdlet
{
  protected sealed override void EndProcessing()
  {
    if (
      typeof(
        PSObject
      )
        .Assembly
        .GetType(
          "System.Management.Automation.TypeAccelerators"
        )
        ?.GetProperty(
          "Get",
          System.Reflection.BindingFlags.Static
          | System.Reflection.BindingFlags.Public
        )
        ?.GetValue(null) is IDictionary typeAccelerators
    )
    {
      HashSet<string> uniqueTypes = new(
        System.StringComparer.OrdinalIgnoreCase
      );

      foreach (string type in typeAccelerators.Keys)
      {
        _ = uniqueTypes.Add(
          type
        );
      }

      WriteObject(
        new SortedSet<string>(
          uniqueTypes,
          System.StringComparer.OrdinalIgnoreCase
        ),
        true
      );
    }
  }
}
