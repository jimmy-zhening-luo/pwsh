namespace Module.Shell.Set.Commands
{
  using System;
  using System.Management.Automation;
  using Completer.PathCompleter;

  [Cmdlet(
    VerbsCommon.Set,
    "DirectorySibling",
    SupportsTransactions = true,
    HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
  )]
  [Alias("c.")]
  [OutputType(
    typeof(System.Management.Automation.PathInfo),
    typeof(System.Management.Automation.PathInfoStack)
  )]
  public class SetDirectorySibling : SetDirectoryLocation
  {
    [Parameter(
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true
    )]
    [AllowEmptyString]
    [SupportsWildcards]
    [RelativePathCompletions(
      "..",
      PathItemType.Directory
    )]
    public new string Path;

    protected override string Reanchor(string typedPath) => System.IO.Path.GetFullPath(
      typedPath,
      System.IO.Path.GetFullPath(
        "..",
        Pwd()
      )
    );
  }

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

    protected abstract string Reanchor(string typedPath);

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
