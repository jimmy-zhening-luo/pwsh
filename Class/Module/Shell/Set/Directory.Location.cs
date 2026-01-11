namespace Module.Shell.Set
{
  using System;
  using System.Management.Automation;

  public abstract class SetDirectoryLocation : PSCoreCommand
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

    private SteppablePipeline steppablePipeline = null;

    protected abstract string location();

    protected virtual string Reanchor(string typedPath) => System.IO.Path.GetFullPath(
      typedPath,
      System.IO.Path.GetFullPath(
        location(),
        Pwd()
      )
    );

    protected override void BeginProcessing()
    {
      MyInvocation.BoundParameters["Path"] = Reanchor(Path ?? string.Empty);

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

    protected override void ProcessRecord()
    {
      steppablePipeline?.Process();
    }

    protected override void EndProcessing()
    {
      if (steppablePipeline != null)
      {
        steppablePipeline.End();
        steppablePipeline.Clean();
        steppablePipeline.Dispose();
        steppablePipeline = null;
      }
    }
  }
}
