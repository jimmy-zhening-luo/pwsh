namespace Module.Command.Shell.Set.Directory;

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

  private protected sealed override void TransformParameters()
  {
    if (UsingCurrentLocation)
    {
      if (
        ParameterSetName != "LiteralPath"
        && string.IsNullOrEmpty(
          path
        )
      )
      {
        string pwd = Pwd();
        string parent = IO.Path.GetFullPath(
          "..",
          pwd
        );

        path = parent == pwd
          ? Home()
          : parent;
        BoundParameters["Path"] = path;
      }
    }
    else
    {
      path = Reanchor(
        path
      );
      BoundParameters["Path"] = path;
    }
  }
}
