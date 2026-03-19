namespace PowerModule.Commands.Shell.Remove.Directory;

abstract public class WrappedRemoveDirectory() : WrappedCommand(
  $@"{StandardModule.Management}\Remove-Item"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    StandardParameter.Path,
    Path
  );

  abstract public string[] Path
  { get; init; }

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
    init => _ = value;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    init => _ = value;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => _ = value;
  }
}
