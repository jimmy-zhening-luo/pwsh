namespace Module.Command.Shell.Start.Explorer;

public abstract class WrappedStartExplorer : WrappedCommandShouldProcess
{
  private protected WrappedStartExplorer() : base(
    "Invoke-Item"
  )
  { }

  public abstract string[] Path
  {
    get;
    set;
  }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public string Filter
  {
    get => filter;
    set => filter = value;
  }
  private string filter = "";

  [Parameter]
  [SupportsWildcards]
  public string[] Include
  {
    get => includes;
    set => includes = value;
  }
  private string[] includes = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude
  {
    get => excludes;
    set => excludes = value;
  }
  private string[] excludes = [];

  private protected sealed override bool SkipSsh => true;

  private protected sealed override void TransformParameters()
  {
    if (IsPresent("Path"))
    {
      if (!UsingCurrentLocation)
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
        UsingCurrentLocation
          ? Pwd()
          : Reanchor()
      };
    }
  }
}
