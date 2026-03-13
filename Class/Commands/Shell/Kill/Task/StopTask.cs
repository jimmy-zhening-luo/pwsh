namespace PowerModule.Commands.Shell.Kill.Task;

using SystemProcess = System.Diagnostics.Process;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = StandardParameter.Name,
  HelpUri = $"{HelpLink}2097058"
)]
[Alias("tkill")]
[OutputType(typeof(void))]
public class StopTask : CoreCommand
{
  [Parameter(
    ParameterSetName = StandardParameter.Name,
    Position = default,
    HelpMessage = "Names of the processes to stop"
  )]
  [Alias("ProcessName")]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  public string[] Name
  { private get; init; } = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = default,
    HelpMessage = "Process IDs of the processes to stop"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  required public int[] Id
  { private get; init; }

  [Parameter(
    ParameterSetName = "InputObject",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Process items to stop"
  )]
  [AllowEmptyCollection]
  [ValidateNotNull]
  required public SystemProcess[] InputObject
  { get; init; }

  [Parameter(
    HelpMessage = "Stop the entire process tree (the processes and all of their descendants)"
  )]
  public SwitchParameter Descendant
  { private protected get; init; }

  static void KillProcess(
    int pid,
    bool entireProcessTree = default
  )
  {
    using var process = SystemProcess.GetProcessById(
      pid
    );

    process.Kill(entireProcessTree);
  }

  static void KillProcesses(
    string name,
    bool entireProcessTree = default
  )
  {
    foreach (
      var process in SystemProcess.GetProcessesByName(
        name
      )
    )
    {
      process.Kill(entireProcessTree);
    }
  }

  static void KillProcesses(
    SystemProcess[] processes,
    bool entireProcessTree = default
  )
  {
    foreach (var process in processes)
    {
      process.Kill(entireProcessTree);
    }
  }

  sealed override private protected void Process()
  {
    if (ParameterSetName is "InputObject")
    {
      KillProcesses(
        InputObject,
        Descendant
      );
    }
  }

  sealed override private protected void Postprocess()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in Id)
        {
          KillProcess(
            pid,
            Descendant
          );
        }

        break;

      case StandardParameter.Name when Name is []:
        KillProcesses("explorer");

        break;

      case StandardParameter.Name:
        foreach (var name in Name)
        {
          if (
            int.TryParse(
              name,
              out var pid
            )
          )
          {
            KillProcess(
              pid,
              Descendant
            );
          }
          else
          {
            KillProcesses(
              name,
              Descendant
            );
          }
        }

        break;

      default:
        break;
    }
  }
}
