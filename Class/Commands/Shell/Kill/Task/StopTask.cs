namespace PowerModule.Commands.Shell.Kill.Task;

using SystemProcess = System.Diagnostics.Process;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = nameof(Name),
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2097058"
)]
[Alias("tkill")]
[OutputType(typeof(void))]
public class StopTask : CoreCommand
{
  const string ShouldProcessAction = "Stop process";

  const string DefaultProcess = "explorer";

  [Parameter(
    ParameterSetName = nameof(Name),
    Position = default
  )]
  [Alias("ProcessName")]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  public string[] Name
  { private get; init; } = [];

  [Parameter(
    ParameterSetName = nameof(Id),
    Mandatory = true,
    Position = default
  )]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  required public int[] Id
  { private get; init; }

  [Parameter(
    ParameterSetName = nameof(InputObject),
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true
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
    if (ParameterSetName is nameof(InputObject))
    {
      foreach (var process in InputObject)
      {
        if (
          ShouldProcess(
            $"{process.ProcessName} : {process.Id} ({nameof(Descendant)}: {Descendant})",
            ShouldProcessAction
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
      case nameof(Id):
        foreach (var pid in Id)
        {
          if (
            ShouldProcess(
              $"{nameof(Id)}: {pid} ({nameof(Descendant)}: {Descendant})",
              ShouldProcessAction
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

      case nameof(Name) when Name is []:
        if (
          ShouldProcess(
            DefaultProcess,
            $"{ShouldProcessAction}es by name"
          )
        )
        {
          KillProcesses(DefaultProcess);
        }

        break;

      case nameof(Name):
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
                $"{nameof(Id)}: {pid} ({nameof(Descendant)}: {Descendant})",
                ShouldProcessAction
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
                $"{name} ({nameof(Descendant)}: {Descendant})",
                $"{ShouldProcessAction}es by name"
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
