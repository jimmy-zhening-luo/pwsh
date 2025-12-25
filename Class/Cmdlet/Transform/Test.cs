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
        public Type T
        {
          get => t;
          set => t = value;
        }
        private Type t;

        [Parameter(DontShow = true)]
        public SwitchParameter z;

        protected override void EndProcessing()
        {
          WriteObject(T.IsEnum);
        }
      }
    }
  } // namespace Test
} // namespace Transform
