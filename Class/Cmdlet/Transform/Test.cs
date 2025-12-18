using System.Management.Automation;

namespace Transform
{
  namespace Commands
  {
    namespace Test
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
          Position = 0,
          HelpMessage = "Name to be displayed"
        )]
        [AllowEmptyString]
        public string Name
        {
          get => name;
          set => name = value;
        }
        private string name = "";

        [Parameter(DontShow = true)]
        public SwitchParameter Nothing;

        protected override void EndProcessing()
        {
          WriteObject(name);
        }
      }
    }
  }
}
