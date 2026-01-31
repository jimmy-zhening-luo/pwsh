namespace Module.Command.Windows.Configure;

[Cmdlet(
  VerbsData.Edit,
  "SystemPath"
)]
[Alias("path")]
[OutputType(typeof(void))]
public sealed class EditSystemPath : CoreCommand
{
  private protected sealed override bool SkipSsh => true;

  [Parameter(
    HelpMessage = "Launch Environment Variables control panel as administrator to edit system variables, triggering a UAC prompt if needed."
  )]
  public SwitchParameter Administrator
  {
    get => administrator;
    set => administrator = value;
  }
  private bool administrator;

  private protected sealed override void AfterEndProcessing()
  {
    Invocation.ShellExecute(
      "rundll32.exe",
      "sysdm.cpl,EditEnvironmentVariables",
      administrator
    );
  }
}
