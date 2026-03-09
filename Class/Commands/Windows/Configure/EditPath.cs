namespace PowerModule.Commands.Windows.Configure;

[Cmdlet(
  VerbsData.Edit,
  "SystemPath"
)]
[Alias("path")]
[OutputType(typeof(void))]
sealed public class EditSystemPath() : CoreCommand(true)
{
  [Parameter(
    HelpMessage = "Launch Environment Variables control panel as Administrator"
  )]
  public SwitchParameter Administrator { private get; set; }

  sealed override private protected void Postprocess()
  {
    Client.Start.ShellExecute(
      "rundll32.exe",
      "sysdm.cpl,EditEnvironmentVariables",
      Administrator
    );
  }
}
