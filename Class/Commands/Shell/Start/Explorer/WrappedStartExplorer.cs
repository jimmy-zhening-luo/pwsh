namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommandShouldProcess(
  "Invoke-Item",
  true
)
{
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
    get => inclusions;
    set => inclusions = value;
  }
  private string[] inclusions = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude
  {
    get => exclusions;
    set => exclusions = value;
  }
  private string[] exclusions = [];

  private protected sealed override void TransformParameters()
  {
    if (paths.Length == 0)
    {
      paths = [
        UsingCurrentLocation
          ? Pwd()
          : Reanchor()
      ];
      MyInvocation.BoundParameters["Path"] = paths;
    }
    else
    {
      if (!UsingCurrentLocation)
      {
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

        MyInvocation.BoundParameters["Path"] = paths;
      }
    }
  }
}
