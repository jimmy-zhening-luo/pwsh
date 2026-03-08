namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  "Path"
)
{
  public abstract string Path { get; set; }

  [Parameter]
  public SwitchParameter PassThru
  {
    private get;
    set;
  }

  private protected sealed override void TransformPipelineInput()
  {
    if (InCurrentLocation)
    {
      if (
        ParameterSetName is "Path"
        && Path is ""
      )
      {
        BoundParameters["Path"] = Path = Pwd("..");
      }
    }
    else
    {
      BoundParameters["Path"] = Path = ReanchorPath(Path);
    }
  }
}
