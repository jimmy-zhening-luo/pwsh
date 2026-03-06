namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location"
)
{
  public abstract string Path { set; }
  private protected string path = string.Empty;

  [Parameter]
  public SwitchParameter PassThru { get; set; }

  private protected sealed override void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = path = ReanchorPath(path);
    }
  }
}
