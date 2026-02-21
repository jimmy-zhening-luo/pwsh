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
  private protected sealed override void ProcessRecordAction()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in Id)
        {
          KillProcess(
            (int)pid,
            true
          );
        }

        break;
      case "InputObject":
        foreach (var input in InputObject)
        {
          KillProcess(
            input.Id,
            true
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
              true
            );
          }
          else
          {
            KillProcesses(
              name,
              true
            );
          }
        }

        break;
    }
  }
}
