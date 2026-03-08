namespace Module.Commands.Shell.Set.Directory;

public abstract class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  AcceptsPipelineInput: true
)
{
  public abstract string Path { get; set; }

  sealed private protected override object? PipelineInput => Path;

  [Parameter]
  public SwitchParameter PassThru
  {
    private get;
    set;
  }

  sealed private protected override void TransformPipelineInput()
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
