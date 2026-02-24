namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  "Set-Location"
)
{
  public abstract string Path { get; set; }
  private protected string path = string.Empty;

  [Parameter]
  public SwitchParameter PassThru
  {
    get => passThru;
    set => passThru = value;
  }
  private bool passThru;

  private protected sealed override void TransformPipelineInput()
  {
    path = Reanchor(
      path
    );
    BoundParameters["Path"] = path;
  }
}
