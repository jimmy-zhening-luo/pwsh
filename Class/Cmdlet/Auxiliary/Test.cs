namespace Auxiliary
{
  using System.Management.Automation;

  [Cmdlet(
    VerbsDiagnostic.Test,
    "Cmdlet",
    HelpUri = "https://learn.microsoft.com/dotnet/api/system.management.automation.cmdlet"
  )]
  [Alias("test")]
  [OutputType(typeof(string))]
  public class TestCmdlet : PSCmdlet
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
      string pwd = SessionState.Path.CurrentLocation.Path;

      WriteObject(
        pwd,
        true
      );

      string rel = Path == string.Empty
        ? pwd
        : System.IO.Path.GetRelativePath(pwd, Path);

      if (System.IO.Path.Exists(rel))
      {
        WriteObject(
          System.IO.Path.GetFullPath(Path, pwd),
          true
        );
      }
    }
  }
}
