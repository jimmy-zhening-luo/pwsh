namespace PowerModule.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  AcceptsPipelineInput: true,
  SkipSsh: true
)
{
  abstract public Collection<string> Path
  { get; init; }

  sealed override private protected object? PipelineInput => Path;

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

  sealed override private protected void TransformPipelineInput()
  {
    BoundParameters["Path"] = ReanchorPath(Path);
  }
}
