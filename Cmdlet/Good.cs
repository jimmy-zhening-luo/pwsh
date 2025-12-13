using System.Management.Automation;

namespace Good;

[Cmdlet(
  VerbsCommon.Get,
  "Hello"
)]
public class Hello : Cmdlet
{
  [Parameter(
    Position = 0,
    Mandatory = true
  )]
  public string Greeting
  {
    get { return greeting; }
    set { greeting = value; }
  }
  private string greeting;

  protected override void ProcessRecord()
  {
    WriteObject(this.Greeting + ", World!");
  }
}
