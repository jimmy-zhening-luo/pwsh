namespace PowerModule.Commands.Shell.Kill.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "TaskTree",
  DefaultParameterSetName = nameof(Name),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2097058"
)]
[Alias("tkilld")]
[OutputType(typeof(void))]
sealed public class StopTaskTree : StopTask
{
  public StopTaskTree() => base.Descendant = true;

  new internal SwitchParameter Descendant
  { get; }
}
