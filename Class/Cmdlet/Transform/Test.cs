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
          Mandatory = true,
          Position = 0,
          HelpMessage = "Type to reflect"
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
          WriteObject(
            Enum.GetNames(t),
            true
          );
        }
      }
    }
  } // namespace Test
} // namespace Transform
