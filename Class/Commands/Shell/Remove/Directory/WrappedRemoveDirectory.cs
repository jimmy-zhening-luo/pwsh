namespace PowerModule.Commands.Shell.Remove.Directory;

abstract public class WrappedRemoveDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Remove-Item",
  AcceptsPipelineInput: true
)
{
  abstract public Collection<string> Path
  { get; init; }

  sealed override private protected object? PipelineInput => Path;

  sealed override private protected Dictionary<string, object?> CoercedParameters
  { get; } = new()
  {
    ["Recurse"] = true,
    ["Force"] = true,
  };

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  { private get; init; }
}
