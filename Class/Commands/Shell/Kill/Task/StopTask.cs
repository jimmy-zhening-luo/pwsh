namespace PowerModule.Commands.Shell.Kill.Task;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = "Name",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
)]
[Alias("tkill")]
[OutputType(typeof(void))]
public class StopTask : CoreCommand
{
  [Parameter(
    ParameterSetName = "Name",
    Position = default,
    HelpMessage = "Names of the processes to stop"
  )]
  [Alias("ProcessName")]
  [SupportsWildcards]
  [ValidateNotNullOrWhiteSpace]
  public Collection<string> Name
  { private get; init; } = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = default,
    HelpMessage = "Process IDs of the processes to stop"
  )]
  [AllowEmptyCollection]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  required public Collection<int> Id
  { private get; init; }

  [Parameter(
    ParameterSetName = "InputObject",
    Mandatory = true,
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Process objects to stop"
  )]
  [AllowEmptyCollection]
  [ValidateNotNull]
  required public Collection<System.Diagnostics.Process> InputObject
  { get; init; }

  [Parameter(
    HelpMessage = "Stop the entire process tree (the processe and all of its descendants)"
  )]
  public SwitchParameter Descendant
  { private protected get; init; }

  static void KillProcesses(
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

  static void KillProcess(
    int pid,
    bool entireProcessTree = default
  ) => System.Diagnostics.Process
    .GetProcessById(pid)
    .Kill(entireProcessTree);

  sealed override private protected void Process()
  {
    if (ParameterSetName is "InputObject")
    {
      foreach (var process in InputObject)
      {
        process.Kill(Descendant);
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
          KillProcess(pid, Descendant);
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
              out var pid
            ):
              KillProcess(pid, Descendant);

              break;

            default:
              KillProcesses(name, Descendant);

              break;
          }
        }

        break;

      default:
        break;
    }
  }
}
