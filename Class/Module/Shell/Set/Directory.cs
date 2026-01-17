namespace Module.Shell.Set
{
  namespace Commands
  {
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Set,
      "Directory",
      DefaultParameterSetName = "Path",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c")]
    [OutputType(
      typeof(PathInfo),
      typeof(PathInfoStack)
    )]
    public class SetDirectory : WrappedCommand
    {
      [Parameter(
        ParameterSetName = "Path",
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        "",
        PathItemType.Directory
      )]
      public string Path;

      [Parameter(
        ParameterSetName = "LiteralPath",
        Mandatory = true,
        ValueFromPipelineByPropertyName = true
      )]
      [Alias("PSPath", "LP")]
      public string LiteralPath;

      [Parameter(
        ParameterSetName = "Stack",
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      public string Stack;

      [Parameter]
      public SwitchParameter PassThru;

      protected override void BeginProcessing()
      {
        if (Path == null && LiteralPath == null)
        {
          string pwd = Pwd();
          string parent = System.IO.Path.GetFullPath(
            "..",
            pwd
          );

          MyInvocation.BoundParameters["Path"] = parent == pwd
            ? Context.Home()
            : parent;
        }

        using PowerShell ps = PowerShell.Create(
          RunspaceMode.CurrentRunspace
        );
        ps
          .AddCommand(
            SessionState
              .InvokeCommand
              .GetCommand(
                "Set-Location",
                CommandTypes.Cmdlet
              )
          )
          .AddParameters(
            MyInvocation.BoundParameters
          );

        steppablePipeline = ps.GetSteppablePipeline();
        steppablePipeline.Begin(this);
      }
    }
  }
}
