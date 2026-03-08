namespace Module.Commands.Shell.Stop.Task;

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

public abstract class TaskManager : CoreCommand
{
  private protected bool descendant;

  [Parameter(
    ParameterSetName = "Name",
    Position = default,
    HelpMessage = "Names of the processes to stop"
  )]
  [SupportsWildcards]
  [Alias("ProcessName")]
  public string[] Name
  {
    private get;
    set;
  } = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = default,
    HelpMessage = "Process IDs of the processes to stop"
  )]
  public required int[] Id
  {
    private get;
    set;
  }

  [Parameter(
    ParameterSetName = "InputObject",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Process objects to stop"
  )]
  public required System.Diagnostics.Process[] InputObject { get; set; }

  private static void KillProcesses(
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

  private static void KillProcess(
    int pid,
    bool entireProcessTree = default
  ) => System.Diagnostics.Process
    .GetProcessById(pid)
    .Kill(entireProcessTree);

  private protected sealed override void Process()
  {
    if (ParameterSetName is "InputObject")
    {
      foreach (var process in InputObject)
      {
        process.Kill(descendant);
      }
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

      case "Name" when Name is []:
        KillProcesses("explorer");

        break;

      case "Name":
        foreach (var name in Name)
        {
          switch (name)
          {
            case "":
              break;

            case var n when int.TryParse(
              n,
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
