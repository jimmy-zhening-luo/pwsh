using System;
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
      [OutputType(typeof(string))]
      public class TestCmdlet : Cmdlet
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
          if (System.IO.Path.Exists(path))
          {
            WriteObject(
              System.IO.Path.GetFullPath(path),
              true
            );
          }
        }
      }
    }
  } // namespace Test
} // namespace Transform
