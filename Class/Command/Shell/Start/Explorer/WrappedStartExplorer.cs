namespace Module.Command.Shell.Start.Explorer;

public abstract class WrappedStartExplorer : WrappedCommandShouldProcess
{
  private protected WrappedStartExplorer() : base(
    "Invoke-Item"
  )
  { }

  [Parameter(
    ParameterSetName = "Path",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [AllowEmptyCollection]
  [SupportsWildcards]
  public string[]? Path;

  [Parameter]
  [SupportsWildcards]
  public string? Filter;

  [Parameter]
  [SupportsWildcards]
  public string[]? Include;

  [Parameter]
  [SupportsWildcards]
  public string[]? Exclude;

  private protected override bool NoSsh => true;

  private protected override void AnchorBoundPath()
  {
    if (IsPresent("Path"))
    {
      string[] paths = (string[])BoundParameters["Path"];

      for (int i = 0; i < paths.Length; i++)
      {
        paths[i] = Reanchor(paths[i]);
      }

      BoundParameters["Path"] = paths;
    }
    else
    {
      BoundParameters["Path"] = new string[] { Reanchor() };
    }
  }

  private protected override bool BeforeBeginProcessing()
  {
    if (
      Here
      && !IsPresent("Path")
    )
    {
      BoundParameters["Path"] = new string[] { Pwd() };
    }

    return true;
  }
}
