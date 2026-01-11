namespace Module.Shell.Set.Commands
{
  using System;
  using System.Management.Automation;
  using Completer.PathCompleter;

  public abstract class SetDirectoryLocation : PSCoreCommand
  {
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyString]
    [SupportsWildcards]
    public string Path
    {
      get => path;
      set => path = value;
    }
    private string path = string.Empty;

    [Parameter]
    public SwitchParameter PassThru;

    private SteppablePipeline steppablePipeline = null;

    protected abstract string Reanchor(string typedPath);

    protected override void BeginProcessing()
    {
      path = Reanchor(path);

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
