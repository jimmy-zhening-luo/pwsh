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
    HelpMessage = "Launch Environment Variables control panel as administrator to edit system variables, triggering a UAC prompt if needed."
  )]
  public SwitchParameter Administrator
  {
    get => administrator;
    set => administrator = value;
  }
  private bool administrator;

  private protected sealed override void Postprocess()
  {
    Client.Start.ShellExecute(
      "rundll32.exe",
      "sysdm.cpl,EditEnvironmentVariables",
      administrator
    );
  }
}
