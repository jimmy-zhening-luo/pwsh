namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  "Set-Location"
)
{
  public abstract string Path
  {
    get;
    set;
  }
  private protected string path = "";

  [Parameter]
  public SwitchParameter PassThru
  {
    get => passThru;
    set => passThru = value;
  }
  private bool passThru;

  private protected override void TransformParameters()
  {
    path = Reanchor(
      path
    );
    BoundParameters["Path"] = path;
  }
}
