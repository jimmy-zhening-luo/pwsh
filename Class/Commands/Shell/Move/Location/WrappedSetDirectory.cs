namespace PowerModule.Commands.Shell.Move.Location;

abstract public class WrappedSetDirectory() : WrappedCommand(
  $@"{StandardModule.Management}\Set-Location"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    nameof(Path),
    Path
  );

  abstract public string Path
  { get; set; }

  [Parameter]
  public SwitchParameter PassThru
  {
    init => _ = value;
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (InCurrentLocation)
    {
      if (
        ParameterSetName is nameof(Path)
        && Path is ""
      )
      {
        Path = Parent();
      }
    }
    else
    {
      Path = ReanchorPath(Path);
    }
  }
}
