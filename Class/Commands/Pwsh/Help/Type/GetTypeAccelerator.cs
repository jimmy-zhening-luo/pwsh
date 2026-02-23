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
      var uniqueTypes = new HashSet<string>(
        System.StringComparer.OrdinalIgnoreCase
      );

      foreach (var type in typeAccelerators.Keys)
      {
        _ = uniqueTypes.Add(type.ToString()!);
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
