namespace PowerModule.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  SkipSsh: true
)
{
  abstract public string[] Path
  { get; init; }

  sealed override private protected PipelineInputSource PipelineInput => () => Path;

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

  sealed override private protected void TransformPipelineInput() => SetBoundParameter(
    "Path",
    ReanchorPath(Path)
  );
}
