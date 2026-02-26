namespace Module.Commands.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory() : WrappedCommandShouldProcess(
  @"Microsoft.PowerShell.Management\Remove-Item"
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

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["Recurse"] = SwitchParameter.Present,
    ["Force"] = SwitchParameter.Present,
  };
}
