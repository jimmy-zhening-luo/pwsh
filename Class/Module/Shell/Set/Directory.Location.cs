namespace Module.Shell.Set
{
  using System.Management.Automation;

  public abstract class SetLocalDirectoryCommand : WrappedCommand
  {
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyString]
    [SupportsWildcards]
    public string Path;

    [Parameter]
    public SwitchParameter PassThru;

    protected abstract string location();

    protected abstract string root();

    protected string Reanchor(string typedPath) => System.IO.Path.GetFullPath(
      typedPath,
      System.IO.Path.GetFullPath(
        location(),
        root()
      )
    );

    protected override void BeginProcessing()
    {
      MyInvocation.BoundParameters["Path"] = Reanchor(
        MyInvocation.BoundParameters.ContainsKey("Path")
          ? MyInvocation.BoundParameters["Path"].ToString()
          : string.Empty
      );

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
