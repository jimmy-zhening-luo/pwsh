namespace PowerModule.Commands.Shell.Kill.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "TaskTree",
  DefaultParameterSetName = StandardParameter.Name,
  HelpUri = $"{HelpLink}2097058"
)]
[Alias("tkilld")]
[OutputType(typeof(void))]
sealed public class StopTaskTree : StopTask
{
  public StopTaskTree() => base.Descendant = true;

  new public SwitchParameter Descendant
  { get; init; }
}
