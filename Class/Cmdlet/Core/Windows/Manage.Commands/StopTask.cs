namespace Core.Windows.Manage.Commands
{
  using System.Diagnostics;
  using System.Management.Automation;

  [Cmdlet(
    VerbsLifecycle.Stop,
    "Task",
    DefaultParameterSetName = "Name",
    HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097058"
  )]
  [Alias("tkill")]
  [OutputType(typeof(void))]
  public class StopTask : PSCmdlet
  {
    [Parameter(
      ParameterSetName = "Name",
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true,
      HelpMessage = "The name(s) of the process to stop."
    )]
    [SupportsWildcards]
    [Alias("ProcessName")]
    public string[] Name
    {
      get => name;
      set => name = value;
    }
    private string[] name = [];

    [Parameter(
      ParameterSetName = "Id",
      Mandatory = true,
      Position = 0,
      ValueFromPipelineByPropertyName = true,
      HelpMessage = "The process ID(s) of the process to stop."
    )]
    public uint[] Id
    {
      get => id;
      set => id = value;
    }
    private uint[] id;

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

    protected override void ProcessRecord()
    {
      switch (ParameterSetName)
      {
        case "Id":
          foreach (var i in id)
          {
            Process
              .GetProcessById((int)i)
              .Kill(true);
          }
          break;
        default:
          foreach (var n in name)
          {
            if (n == string.Empty)
            {
              continue;
            }
            else if (uint.TryParse(n, out uint pid))
            {
              Process
                .GetProcessById((int)pid)
                .Kill(true);
            }
            else
            {
              Process[] processes = Process.GetProcessesByName(n);

              foreach (var process in processes)
              {
                process.Kill(true);
              }
            }
          }
          break;
      }
    }
    protected override void EndProcessing()
    {
      switch (ParameterSetName)
      {
        case "Self":
          if (self)
          {
            WriteWarning("No-op because I'm too lazy to correctly implement this.");
          }
          break;
        case "Name":
          if (name.Length == 0)
          {
            var processes = Process.GetProcessesByName("explorer");

            foreach (var process in processes)
            {
              process.Kill(true);
            }
          }
          break;
      }
    }
  }
}
