namespace Module.Commands.Shell.Set.Directory;

abstract public class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  AcceptsPipelineInput: true
)
{
  abstract public string Path { get; set; }

  sealed override private protected object? PipelineInput => Path;

  [Parameter]
  public SwitchParameter PassThru { private get; set; }

  sealed override private protected void TransformPipelineInput()
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
