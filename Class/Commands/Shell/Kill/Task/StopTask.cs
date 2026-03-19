namespace PowerModule.Commands.Shell.Kill.Task;

using SystemProcess = System.Diagnostics.Process;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = StandardParameter.Name,
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
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
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Name
  { private get; init; } = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = default,
    HelpMessage = "Process IDs of the processes to stop"
  )]
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
    var processes = SystemProcess.GetProcessesByName(
      name
    );

    foreach (var process in processes)
    {
      process.Kill(entireProcessTree);
      process.Dispose();
    }
  }

  sealed override private protected void Process()
  {
    if (ParameterSetName is "InputObject")
    {
      foreach (var process in InputObject)
      {
        if (
          ShouldProcess(
            $"{process.ProcessName} : {process.Id} (Descendants: {Descendant})",
            "Stop process"
          )
        )
        {
          process.Kill(Descendant);
        }
      }
    }
  }

  sealed override private protected void Postprocess()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in Id)
        {
          if (
            ShouldProcess(
              $"PID: {pid} (Descendants: {Descendant})",
              "Stop process"
            )
          )
          {
            KillProcess(
              pid,
              Descendant
            );
          }
        }

        break;

      case StandardParameter.Name when Name is []:
        if (
          ShouldProcess(
            "explorer",
            "Stop processes by name"
          )
        )
        {
          KillProcesses("explorer");
        }

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
            if (
              ShouldProcess(
                $"PID: {pid} (Descendants: {Descendant})",
                "Stop process"
              )
            )
            {
              KillProcess(
                pid,
                Descendant
              );
            }
          }
          else
          {
            if (
              ShouldProcess(
                $"{name} (Descendants: {Descendant})",
                "Stop processes by name"
              )
            )
            {
              KillProcesses(
                name,
                Descendant
              );
            }
          }
        }

        break;

      default:
        break;
    }
  }
}
