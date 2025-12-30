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
        [AllowNull]
        public string Path;

        protected override void EndProcessing()
        {
          string pwd = SessionState.Path.CurrentLocation.Path;

          WriteObject(
            pwd,
            true
          );

          string rel = System.IO.Path.GetRelativePath(pwd, Path);

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
  } // namespace Test
} // namespace Transform
