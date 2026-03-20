namespace PowerModule.Commands.Windows.Configure;

[Cmdlet(
  VerbsData.Edit,
  "SystemPath"
)]
[Alias("path")]
[OutputType(typeof(void))]
sealed public class EditSystemPath() : CoreCommand(true)
{
  [Parameter]
  public SwitchParameter Administrator
  { private get; init; }

  sealed override private protected void Postprocess() => Client.Start.ShellExecute(
    "rundll32.exe",
    "sysdm.cpl,EditEnvironmentVariables",
    Administrator
  );
}
