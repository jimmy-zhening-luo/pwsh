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
  public System.Diagnostics.Process[] InputObject
  {
    get => inputs;
    set => inputs = value;
  }
  private System.Diagnostics.Process[] inputs = [];

  [Parameter(
    HelpMessage = "Stop the entire process tree (the process and all its descendants)."
  )]
  public SwitchParameter Descendant
  {
    get => descendant;
    set => descendant = value;
  }
  private bool descendant;

  private protected sealed override bool ValidateParameters() => ParameterSetName != "Name"
    || names.Length != 0;

  private static void KillProcess(
    int pid,
    bool entireProcessTree = false
  ) => System.Diagnostics.Process
    .GetProcessById(
      pid
    )
    .Kill(
      entireProcessTree
    );

  private static void KillProcesses(
    string name,
    bool entireProcessTree = false
  )
  {
    foreach (
      var process in System.Diagnostics.Process.GetProcessesByName(
        name
      )
    )
    {
      process.Kill(
        entireProcessTree
      );
    }
  }

  private protected sealed override void ProcessRecordAction()
  {
    switch (ParameterSetName)
    {
      case "Id":
        foreach (var pid in pids)
        {
          KillProcess(
            (int)pid,
            descendant
          );
        }

        break;
      case "InputObject":
        foreach (var input in inputs)
        {
          KillProcess(
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

  private protected sealed override void DefaultAction()
  {
    if (ParameterSetName == "Name")
    {
      KillProcesses(
        "explorer"
      );
    }
  }
}
