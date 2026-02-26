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
  public uint[] Id { get; set; } = [];

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
        if (Name is [])
        {
          KillProcesses("explorer");
        }
        else
        {
          foreach (var name in Name)
          {
            if (name is "")
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
        }

        break;
    }
  }
}
