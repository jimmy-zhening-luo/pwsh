namespace Module.Command.Help.Type;

using static System.Reflection.BindingFlags;

[Cmdlet(
  VerbsCommon.Get,
  "Type",
  HelpUri = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_type_accelerators"
)]
[Alias("types", "ty")]
[OutputType(typeof(string))]
public class GetType : CoreCommand
{
  protected override void EndProcessing()
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
          Static
          | Public
        )
        ?.GetValue(null) is IDictionary typeAccelerators
    )
    {
      var uniqueTypes = new StringHashSet(
        StringComparer.OrdinalIgnoreCase
      );

      foreach (var type in typeAccelerators.Keys)
      {
        _ = uniqueTypes.Add(type.ToString()!);
      }

      WriteObject(
        new SortedStringSet(
          uniqueTypes,
          StringComparer.OrdinalIgnoreCase
        ),
        true
      );
    }
  }
}
