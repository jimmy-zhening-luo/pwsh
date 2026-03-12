namespace PowerModule.Commands.Shell.Move.Location;

abstract public class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location"
)
{
  abstract public string Path
  { get; init; }

  sealed override private protected PipelineInputSource PipelineInput => () => Path;

  [Parameter]
  public SwitchParameter PassThru
  {
    init => Discard();
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (InCurrentLocation)
    {
      if (
        ParameterSetName is StandardParameter.Path
        && Path is ""
      )
      {
        SetBoundParameter(
          StandardParameter.Path,
          Parent()
        );
      }
    }
    else
    {
      SetBoundParameter(
        StandardParameter.Path,
        ReanchorPath(Path)
      );
    }
  }
}
