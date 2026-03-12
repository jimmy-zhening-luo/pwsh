namespace PowerModule.Commands.Shell.Remove.Directory;

abstract public class WrappedRemoveDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Remove-Item"
)
{
  abstract public string[] Path
  { get; init; }

  sealed override private protected PipelineInputSource PipelineInput => () => Path;

  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
  {
    ["Recurse"] = true,
    ["Force"] = true,
  };

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    init => Discard();
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    init => Discard();
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => Discard();
  }
}
