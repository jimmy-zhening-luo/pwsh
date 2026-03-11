namespace PowerModule.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  AcceptsPipelineInput: true,
  SkipSsh: true
)
{
  abstract public Collection<string> Path
  { get; init; }

  sealed override private protected Collection<string> PipelineInput => Path;

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    init => Bind();
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  {
    init => Bind();
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  {
    init => Bind();
  }

  sealed override private protected void TransformPipelineInput() => SetBoundParameter(
    "Path",
    ReanchorPath(Path)
  );
}
