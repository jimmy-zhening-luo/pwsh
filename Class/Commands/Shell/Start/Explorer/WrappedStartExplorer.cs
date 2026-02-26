namespace Module.Commands.Shell.Start.Explorer;

public abstract class WrappedStartExplorer() : WrappedCommandShouldProcess(
  @"Microsoft.PowerShell.Management\Invoke-Item",
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

  private protected sealed override void TransformPipelineInput()
  {
    if (Path is [])
    {
      Path = [
        UsingDefaultLocation
          ? Pwd()
          : Reanchor()
      ];
      BoundParameters["Path"] = Path;
    }
    else
    {
      if (!UsingDefaultLocation)
      {
        for (
          int i = default;
          i < Path.Length;
          ++i
        )
        {
          Path[i] = Reanchor(
            Path[i]
          );
        }

        BoundParameters["Path"] = Path;
      }
    }
  }
}
