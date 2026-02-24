namespace Module.Commands.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory() : WrappedCommandShouldProcess(
  "Remove-Item"
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
    BoundParameters["Recurse"] = SwitchParameter.Present;
    BoundParameters["Force"] = SwitchParameter.Present;
  }
}
