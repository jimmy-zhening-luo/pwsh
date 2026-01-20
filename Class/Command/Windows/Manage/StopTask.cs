namespace Module.Command.Windows.Manage
{
  namespace Commands
  {
    using System.Diagnostics;

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
        Position = 0,
        ValueFromPipeline = true,
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
            foreach (var pid in pids)
            {
              Process
                .GetProcessById((int)pid)
                .Kill(true);
            }

            break;
          default:
            foreach (var name in names)
            {
              if (string.IsNullOrEmpty(name))
              {
                continue;
              }
              else if (uint.TryParse(name, out uint pid))
              {
                Process
                  .GetProcessById((int)pid)
                  .Kill(true);
              }
              else
              {
                foreach (
                  var process in Process.GetProcessesByName(
                    name
                  )
                )
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
            if (names.Length == 0)
            {
              foreach (
                var process in Process.GetProcessesByName(
                  "explorer"
                )
              )
              {
                process.Kill(true);
              }
            }

            break;
        }
      }
    }
  }
}
