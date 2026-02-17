namespace Module.Commands.Help.Type;

using BindingFlags = System.Reflection.BindingFlags;

[Cmdlet(
  VerbsCommon.Get,
  "Type",
  HelpUri = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_type_accelerators"
)]
[Alias("types", "ty")]
[OutputType(typeof(string))]
public sealed class GetType : Cmdlet
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
          BindingFlags.Static
          | BindingFlags.Public
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
