using System.Reflection.Metadata;

namespace Module.Commands.Windows.Manage.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "TaskTree",
  DefaultParameterSetName = "Name",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
)]
[Alias("tkilld")]
[OutputType(typeof(void))]
public sealed class StopTaskTree : TaskManager
{
  public StopTaskTree() => descendant = true;
}
