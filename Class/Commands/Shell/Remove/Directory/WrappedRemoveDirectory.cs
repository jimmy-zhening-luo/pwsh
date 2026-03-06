namespace Module.Commands.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Remove-Item"
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

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["Recurse"] = true,
    ["Force"] = true,
  };
}
