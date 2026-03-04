namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommandShouldProcess(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  SkipSsh: true
)
{
  public abstract string[] Path { get; set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public required string Filter { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Include { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude { get; set; }

  private protected sealed override void TransformPipelineInput()
  {
    BoundParameters["Path"] = Path = ReanchorPath(Path);
  }
}
