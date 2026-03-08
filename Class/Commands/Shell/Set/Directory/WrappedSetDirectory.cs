namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  "Path"
)
{
  public abstract string Path { get; set; }
  private protected string path = string.Empty;

  [Parameter]
  public SwitchParameter PassThru
  {
    private get;
    set;
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = path = ReanchorPath(path);
    }
  }
}
