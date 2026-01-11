namespace Module.Profile.Commands
{
  using System.Management.Automation;

  [Cmdlet(
    VerbsLifecycle.Start,
    "History"
  )]
  [Alias("oc")]
  [OutputType(typeof(void))]
  public class StartHistory : Cmdlet
  {
    protected override void EndProcessing() => Context.CreateProcess(
      Context.LocalAppData() + @"\Programs\Microsoft VS Code\bin\code.cmd",
      Context.AppData() + @"\Microsoft\Windows\PowerShell\PSReadLine\ConsoleHost_history.txt --profile=Setting --new-window",
      true
    );
  }
}
