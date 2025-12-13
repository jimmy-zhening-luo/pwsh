using System.Management.Automation;

namespace Good;

[Cmdlet(
  VerbsCommon.Get,
  "Hello"
)]
public class Hello : Cmdlet
{
  protected override void ProcessRecord()
  {
    WriteObject("Hello, World!");
  }
}
