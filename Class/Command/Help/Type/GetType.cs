namespace Module.Help.Type
{
  namespace Commands
  {
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Management.Automation;

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
              BindingFlags.Static
              | BindingFlags.Public
            )
            ?.GetValue(null) is IDictionary typeAccelerators
        )
        {
          var uniqueTypes = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase
          );

          foreach (var type in typeAccelerators.Keys)
          {
            _ = uniqueTypes.Add(type.ToString()!);
          }

          WriteObject(
            new SortedSet<string>(
              uniqueTypes,
              StringComparer.OrdinalIgnoreCase
            ),
            true
          );
        }
      }
    }
  }
}
