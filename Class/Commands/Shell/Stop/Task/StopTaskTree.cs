namespace Module.Commands.Shell.Stop.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "TaskTree",
  DefaultParameterSetName = "Name",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
)]
[Alias("tkilld")]
[OutputType(typeof(void))]
sealed public class StopTaskTree : StopTask
{
  public StopTaskTree() => base.Descendant = true;

  new private SwitchParameter Descendant { get; set; }
}
