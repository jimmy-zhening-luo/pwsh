namespace Module.Commands.Shell.Remove.Directory;

abstract public class WrappedRemoveDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Remove-Item",
  AcceptsPipelineInput: true
)
{
  abstract public string[] Path { get; set; }

  sealed override private protected object? PipelineInput => Path;

  sealed override private protected Dictionary<string, object?> CoercedParameters { get; } = new()
  {
    ["Recurse"] = true,
    ["Force"] = true,
  };

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
}
