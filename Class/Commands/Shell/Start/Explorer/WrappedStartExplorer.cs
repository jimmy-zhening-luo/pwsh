namespace PowerModule.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  $@"{StandardModule.Management}\Invoke-Item",
  SkipSsh: true
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    nameof(Path),
    Path
  );

  abstract public string[] Path
  { get; set; }

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

  sealed override private protected void TransformPipelineInput() => Path = ReanchorPath(Path);
}
