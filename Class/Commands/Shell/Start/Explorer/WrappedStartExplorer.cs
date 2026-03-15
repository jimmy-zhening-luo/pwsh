namespace PowerModule.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  StandardParameter.Path,
  SkipSsh: true
)
{
  abstract public string[] Path
  { get; init; }

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

  sealed override private protected void TransformPipelineInput() => SetBoundParameter(
    StandardParameter.Path,
    ReanchorPath(Path)
  );
}
