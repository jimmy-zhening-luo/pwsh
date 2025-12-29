using System.Management.Automation;

namespace Transform
{
  namespace Test
  {
    namespace Commands
    {
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
        private string path;

        [Parameter(DontShow = true)]
        public SwitchParameter z;

        protected override void EndProcessing()
        {
          string pwd = this.SessionState.Path.CurrentLocation.Path;

          WriteObject(
            pwd,
            true
          );

          string rel = System.IO.Path.GetRelativePath(pwd, path);

          if (System.IO.Path.Exists(rel))
          {
            WriteObject(
              System.IO.Path.GetFullPath(path, pwd),
              true
            );
          }
        }
      }
    }
  } // namespace Test
} // namespace Transform
