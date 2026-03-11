namespace PowerModule.Commands.Shell.Move.Location;

abstract public class WrappedSetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Set-Location",
  AcceptsPipelineInput: true
)
{
  abstract public string Path
  { get; init; }

  sealed override private protected string PipelineInput => Path;

  [Parameter]
  public SwitchParameter PassThru
  {
    init => Bind();
  }

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
          Parent()
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
