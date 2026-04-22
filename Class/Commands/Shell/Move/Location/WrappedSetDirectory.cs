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
    switch (
      (
        InCurrentLocation,
        ParameterSetName,
        Path
      )
    )
    {
      case (
        true,
        nameof(Path),
        ""
      ):
        Path = Parent();
        break;

      case (true, _, _):
        break;

      default:
        Path = ReanchorPath(Path);
        break;
    }
  }
}
