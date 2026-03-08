namespace Module.Commands.Shell.Start.Explorer;

abstract public class WrappedStartExplorer() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Invoke-Item",
  AcceptsPipelineInput: true,
  SkipSsh: true
)
{
  abstract public string[] Path { get; set; }

  sealed override private protected object? PipelineInput => Path;

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    private get;
    set;
  }

  sealed override private protected void TransformPipelineInput()
  {
    BoundParameters["Path"] = Path = ReanchorPath(Path);
  }
}
