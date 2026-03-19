namespace PowerModule.Commands.Pwsh.Help;

[Cmdlet(
  VerbsCommon.Get,
  "TypeAccelerator",
  HelpUri = "https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about/about_type_accelerators"
)]
[Alias("types")]
[OutputType(typeof(string))]
sealed public class GetTypeAccelerator : Cmdlet
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Style",
    "IDE0028: Use collection initializers or expressions",
    Justification = "Incorrect suggestion, .NET fix expected in 10.0.3xx: https://github.com/dotnet/Roslyn/issues/82586"
  )]
  sealed override protected void EndProcessing()
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
      SortedSet<string> uniqueTypes = new(
        System.StringComparer.OrdinalIgnoreCase
      );

      foreach (string type in typeAccelerators.Keys)
      {
        _ = uniqueTypes.Add(type);
      }

      WriteObject(
        uniqueTypes,
        true
      );
    }
  }
}
