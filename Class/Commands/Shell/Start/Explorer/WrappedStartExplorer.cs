namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  "Path",
  SkipSsh: true
)
{
  public abstract string[] Path { set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public required string Filter
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  public required string[] Include
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude
  {
    private get;
    set;
  }

  private protected sealed override void TransformPipelineInput()
  {
    BoundParameters["Path"] = paths = ReanchorPath(paths);
  }
}
