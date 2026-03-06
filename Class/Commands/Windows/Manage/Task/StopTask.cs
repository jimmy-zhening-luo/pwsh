namespace Module.Commands.Windows.Manage.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = "Name",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
)]
[Alias("tkill")]
[OutputType(typeof(void))]
public sealed class StopTask : TaskManager
{
  [Parameter(
    HelpMessage = "Stop the entire process tree (the processe and all of its descendants)"
  )]
  public SwitchParameter Descendant
  {
    set => descendant = value;
  }
}
