namespace Module.Command.Shell.Start.Explorer;

public abstract class WrappedStartExplorer : WrappedCommandShouldProcess
{
  private protected WrappedStartExplorer() : base(
    "Invoke-Item"
  )
  { }

  [Parameter(
    ParameterSetName = "Path",
    Position = 0
  )]
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

  private protected sealed override bool SkipSsh => true;

  private protected sealed override void TransformParameters()
  {
    if (IsPresent("Path"))
    {
      if (!Here)
      {
        string[] paths = (string[])BoundParameters["Path"];

        for (
          int i = 0;
          i < paths.Length;
          i++
        )
        {
          paths[i] = Reanchor(
            paths[i]
          );
        }

        BoundParameters["Path"] = paths;
      }
    }
    else
    {
      BoundParameters["Path"] = new string[]
      {
        Here
          ? Pwd()
          : Reanchor()
      };
    }
  }
}
