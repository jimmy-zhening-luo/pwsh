namespace Module.Command.Shell.Remove.Directory;

public abstract class WrappedRemoveDirectory : WrappedCommandShouldProcess
{
  private protected WrappedRemoveDirectory() : base(
    "Remove-Item"
  )
  { }

  public abstract string[] Path
  {
    get;
    set;
  }
  private protected string[] paths = [];

  [Parameter(
    ParameterSetName = "Path",
    Position = 1
  )]
  [SupportsWildcards]
  public virtual string Filter
  {
    get => filter;
    set => filter = value;
  }
  private protected string filter = "";

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
    BoundParameters["Recurse"] = SwitchParameter.Present;
    BoundParameters["Force"] = SwitchParameter.Present;
  }
}
