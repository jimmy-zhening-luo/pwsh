namespace Module.Shell.Set.Commands
{
  using System;
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
    typeof(System.Management.Automation.PathInfo),
    typeof(System.Management.Automation.PathInfoStack)
  )]
  public class SetDirectory : PSCoreCommand
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
    public string Path
    {
      get => path;
      set => path = value;
    }
    private string path = string.Empty;

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

    private SteppablePipeline steppablePipeline = null;

    protected override void BeginProcessing()
    {
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
