using System.Reflection.Metadata;

namespace Module.Commands.Windows.Manage.Task;

public abstract class TaskManager : CoreCommand
{
  [Parameter(
    ParameterSetName = "Name",
    Position = 0,
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
    HelpMessage = "The Process object(s) to stop."
  )]
  public System.Diagnostics.Process[] InputObject
  {
    get => inputs;
    set => inputs = value;
  }
  private System.Diagnostics.Process[] inputs = [];

  private protected sealed override bool ValidateParameters() => ParameterSetName != "Name"
    || names.Length != 0;

  private protected static void KillProcess(
    int pid,
    bool entireProcessTree = false
  ) => System.Diagnostics.Process
    .GetProcessById(
      pid
    )
    .Kill(
      entireProcessTree
    );

  private protected static void KillProcesses(
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
