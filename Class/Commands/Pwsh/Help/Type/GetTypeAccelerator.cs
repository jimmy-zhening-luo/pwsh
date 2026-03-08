namespace Module.Commands.Pwsh.Help.Type;

[Cmdlet(
  VerbsCommon.Get,
  "TypeAccelerator",
  HelpUri = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_type_accelerators"
)]
[Alias("types")]
[OutputType(typeof(string))]
sealed public class GetTypeAccelerator : Cmdlet
{
  sealed protected override void EndProcessing()
  {
    if (
      typeof(PSObject)
        .Assembly
        .GetType(
          "System.Management.Automation.TypeAccelerators"
        )
        ?.GetProperty(
          "Get",
          System.Reflection.BindingFlags.Public
          | System.Reflection.BindingFlags.Static
        )
        ?.GetValue(default) is System.Collections.IDictionary typeAccelerators
    )
    {
      HashSet<string> uniqueTypes = new(System.StringComparer.OrdinalIgnoreCase);

      foreach (string type in typeAccelerators.Keys)
      {
        _ = uniqueTypes.Add(type);
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
