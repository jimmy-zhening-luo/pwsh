namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommandShouldProcess(
  "Invoke-Item",
  true
)
{
  public abstract string[] Path { get; set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public string Filter { get; set; } = string.Empty;

  [Parameter]
  [SupportsWildcards]
  public string[] Include { get; set; } = [];

  [Parameter]
  [SupportsWildcards]
  public string[] Exclude { get; set; } = [];

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
