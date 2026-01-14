namespace Module
{
  namespace Commands
  {
    using System.Management.Automation;

    [Cmdlet(
      VerbsDiagnostic.Test,
      "Cmdlet",
      HelpUri = "https://learn.microsoft.com/dotnet/api/system.management.automation.cmdlet"
    )]
    [Alias("test")]
    [OutputType(typeof(string))]
    public class TestCmdlet : PSCoreCommand
    {
      [Parameter(
        Position = 0
      )]
      [AllowEmptyString]
      public string Path
      {
        get => path;
        set => path = value;
      }
      private string path = string.Empty;

      [Parameter(DontShow = true)]
      public SwitchParameter z;

      protected override void EndProcessing()
      {
        WriteObject(path);
      }
    }
  }
}
