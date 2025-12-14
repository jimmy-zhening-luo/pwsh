using System.Management.Automation;

namespace Good
{
  [Cmdlet(
    VerbsDiagnostic.Test,
    "Hello"
  )]
  public class Hello : Cmdlet
  {
    [Parameter(
      Position = 0
    )]
    public string Greeting
    {
      get;
      set;
    } = "Hello";

    [Parameter(
      Position = 1
    )]
    public string Name
    {
      get;
      set;
    } = "World";

    protected override void ProcessRecord()
    {
      WriteObject($"{Greeting}, {Name}!");
    }
  }
}
