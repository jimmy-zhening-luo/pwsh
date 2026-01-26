namespace Module.Command.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory : WrappedCommandShouldProcess
{
  private protected WrappedRemoveDirectory() : base(
    "Remove-Item"
  )
  { }

  [Parameter(
    ParameterSetName = "Path",
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [SupportsWildcards]
  public string[]? Path;

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [SupportsWildcards]
  public string? Filter;

  [Parameter]
  [SupportsWildcards]
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  private protected sealed override bool ValidateParameters()
  {
    BoundParameters["Recurse"] = SwitchParameter.Present;
    BoundParameters["Force"] = SwitchParameter.Present;

    return true;
  }
}
