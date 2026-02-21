using System.Reflection.Metadata;

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
    HelpMessage = "Stop the entire process tree (the process and all its descendants)."
  )]
  public SwitchParameter Descendant
  {
    get => descendant;
    set => descendant = value;
  }
  private bool descendant;

  private protected sealed override void ProcessRecordAction()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in Id)
        {
          KillProcess(
            (int)pid,
            descendant
          );
        }

        break;
      case "InputObject":
        foreach (var input in InputObject)
        {
          KillProcess(
            input.Id,
            descendant
          );
        }

        break;
      case "Name":
        foreach (var name in Name)
        {
          if (string.IsNullOrEmpty(name))
          {
            continue;
          }
          else if (
            int.TryParse(
              name,
              out int pid
            )
          )
          {
            KillProcess(
              pid,
              descendant
            );
          }
          else
          {
            KillProcesses(
              name,
              descendant
            );
          }
        }

        break;
    }
  }
}
