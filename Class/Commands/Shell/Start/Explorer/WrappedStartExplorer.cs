namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  AcceptsPipelineInput: true,
  SkipSsh: true
)
{
  public abstract string[] Path { get; set; }

  private protected sealed override object? PipelineInput => Path;

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
    BoundParameters["Path"] = Path = ReanchorPath(Path);
  }
}
