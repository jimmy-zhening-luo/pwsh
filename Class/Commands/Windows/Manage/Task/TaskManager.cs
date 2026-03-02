using System.Reflection.Metadata;

namespace Module.Commands.Windows.Manage.Task;

public abstract class TaskManager : CoreCommand
{
  [Parameter(
    ParameterSetName = "Name",
    Position = default,
    HelpMessage = "The name(s) of the process to stop."
  )]
  [SupportsWildcards]
  [Alias("ProcessName")]
  public string[] Name { get; set; } = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = default,
    HelpMessage = "The process ID(s) of the process to stop."
  )]
  public int[] Id { get; set; } = [];

  [Parameter(
    ParameterSetName = "InputObject",
    Mandatory = true,
    Position = default,
    HelpMessage = "The Process object(s) to stop."
  )]
  public System.Diagnostics.Process[] InputObject { get; set; } = [];

  private protected bool descendant;

  private protected static void KillProcess(
    int pid,
    bool entireProcessTree = default
  ) => System.Diagnostics.Process
    .GetProcessById(pid)
    .Kill(entireProcessTree);

  private protected static void KillProcesses(
    string name,
    bool entireProcessTree = default
  )
  {
    foreach (
      var process in System.Diagnostics.Process.GetProcessesByName(
        name
      )
    )
    {
      process.Kill(entireProcessTree);
    }
  }

  private protected sealed override void Postprocess()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in Id)
        {
          KillProcess(pid, descendant);
        }
        break;

      case "InputObject":
        foreach (var process in InputObject)
        {
          process.Kill(descendant);
        }
        break;

      case "Name" when Name is []:
        KillProcesses("explorer");
        break;

      default:
        foreach (var name in Name)
        {
          switch (name)
          {
            case "":
              break;
            case var n when int.TryParse(
              name,
              out int pid
            ):
              KillProcess(pid, descendant);
              break;
            default:
              KillProcesses(name, descendant);
              break;
          }
        }
        break;
    }
  }
}
