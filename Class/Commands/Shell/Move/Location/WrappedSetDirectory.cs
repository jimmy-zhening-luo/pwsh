namespace PowerModule.Commands.Shell.Move.Location;

abstract public class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  AcceptsPipelineInput: true
)
{
  abstract public string Path
  { get; init; }

  sealed override private protected object? PipelineInput => Path;

  [Parameter]
  public SwitchParameter PassThru
  { private get; init; }

  sealed override private protected void TransformPipelineInput()
  {
    if (InCurrentLocation)
    {
      if (
        ParameterSetName is "Path"
        && Path is ""
      )
      {
        SetBoundParameter(
          "Path",
          Pwd("..")
        );
      }
    }
    else
    {
      SetBoundParameter(
        "Path",
        ReanchorPath(Path)
      );
    }
  }
}
