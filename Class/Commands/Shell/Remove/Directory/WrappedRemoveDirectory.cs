namespace Module.Commands.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Remove-Item",
  AcceptsPipelineInput: true
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

  private protected sealed override Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["Recurse"] = true,
    ["Force"] = true,
  };
}
