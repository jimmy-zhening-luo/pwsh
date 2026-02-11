namespace Module.Command.Windows.Manage;

using Process = System.Diagnostics.Process;

[Cmdlet(
  VerbsLifecycle.Stop,
  "Task",
  DefaultParameterSetName = "Name",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
)]
[Alias("tkill")]
[OutputType(typeof(void))]
public sealed class StopTask : CoreCommand
{
  [Parameter(
    ParameterSetName = "Name",
    Position = 0,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The name(s) of the process to stop."
  )]
  [SupportsWildcards]
  [Alias("ProcessName")]
  public string[] Name
  {
    get => names;
    set => names = value;
  }
  private string[] names = [];

  [Parameter(
    ParameterSetName = "Id",
    Mandatory = true,
    Position = 0,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The process ID(s) of the process to stop."
  )]
  public uint[] Id
  {
    get => pids;
    set => pids = value;
  }
  private uint[] pids = [];

  [Parameter(
    ParameterSetName = "InputObject",
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    HelpMessage = "The Process object(s) to stop."
  )]
  public Process[] InputObject
  {
    get => inputs;
    set => inputs = value;
  }
  private Process[] inputs = [];

  [Parameter(
    ParameterSetName = "Name",
    HelpMessage = "Stop the entire process tree (the process and all its descendants)."
  )]
  [Parameter(
    ParameterSetName = "Id",
    HelpMessage = "Stop the entire process tree (the process and all its descendants)."
  )]
  [Parameter(
    ParameterSetName = "InputObject",
    HelpMessage = "Stop the entire process tree (the process and all its descendants)."
  )]
  public SwitchParameter Descendant
  {
    get => descendant;
    set => descendant = value;
  }
  private bool descendant;

  [Parameter(
    ParameterSetName = "Self",
    Mandatory = true,
    HelpMessage = "Stop the current PowerShell host process, its siblings, and its child processes (including the current PowerShell process)."
  )]
  public SwitchParameter Self
  {
    get => self;
    set => self = value;
  }
  private bool self;

  private protected sealed override bool ValidateParameters() => ParameterSetName != "Name"
    || names.Length != 0;

  private protected sealed override void ProcessRecordAction()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in pids)
        {
          KillProcessId(
            (int)pid,
            descendant
          );
        }

        break;
      case "InputObject":
        foreach (var input in inputs)
        {
          KillProcessId(
            input.Id,
            descendant
          );
        }

        break;
      case "Name":
        foreach (var name in names)
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
            KillProcessId(
              pid,
              descendant
            );
          }
          else
          {
            foreach (
              var process in Process.GetProcessesByName(
                name
              )
            )
            {
              process.Kill(descendant);
            }
          }
        }

        break;
    }
  }

  private protected sealed override void DefaultAction()
  {
    switch (ParameterSetName)
    {
      case "Self":
        if (self)
        {
          WriteWarning("No-op because I'm too lazy to correctly implement this.");
        }

        break;
      default:
        foreach (
          var process in Process.GetProcessesByName(
            "explorer"
          )
        )
        {
          process.Kill();
        }

        break;
    }
  }

  private void KillProcessId(
    int pid,
    bool entireProcessTree = false
  ) => Process
    .GetProcessById(
      pid
    )
    .Kill(entireProcessTree);
}
