namespace Module.Commands.Windows.Configure;

[Cmdlet(
  VerbsData.Edit,
  "SystemPath"
)]
[Alias("path")]
[OutputType(typeof(void))]
public sealed class EditSystemPath() : CoreCommand(true)
{
  [Parameter(
    HelpMessage = "Launch Environment Variables control panel as Administrator"
  )]
  public SwitchParameter Administrator
  {
    private get;
    set;
  }

  private protected sealed override void Postprocess()
  {
    Client.Start.ShellExecute(
      "rundll32.exe",
      "sysdm.cpl,EditEnvironmentVariables",
      Administrator
    );
  }
}
