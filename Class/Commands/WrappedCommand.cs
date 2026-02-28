namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  string PipelineInputParameterName = "",
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected bool Piped;

  private protected virtual Dictionary<string, object?> CoercedParameters => [];

  private protected virtual void TransformArguments()
  { }

  private protected virtual void TransformPipelineInput()
  { }

  private protected sealed override void Preprocess()
  {
    CoerceParameters();

    TransformArguments();

    if (
      PipelineInputParameterName is ""
      || BoundParameters.ContainsKey(PipelineInputParameterName)
    )
    {
      TransformPipelineInput();
    }
    else
    {
      Piped = true;
    }

    _ = AddCommand(
      WrappedCommandName,
      CommandType
    )
      .AddParameters(BoundParameters);

    BeginSteppablePipeline();
  }

  private protected sealed override void Process()
  {
    if (
      Piped
      && BoundParameters.ContainsKey(PipelineInputParameterName)
    )
    {
      TransformPipelineInput();

      if (
        BoundParameters.TryGetValue(
          PipelineInputParameterName,
          out var pipelineInput
        )
        && pipelineInput is not null
      )
      {
        ProcessSteppablePipeline(pipelineInput);
      }
      else
      {
        ProcessSteppablePipeline();
      }
    }
    else
    {
      ProcessSteppablePipeline();
    }
  }

  private protected sealed override void Postprocess()
  { }

  private void CoerceParameters()
  {
    foreach (
      (
        var key,
        var value
      ) in CoercedParameters
    )
    {
      switch (value)
      {
        case null or false or "":
          _ = BoundParameters.Remove(key);
          break;

        case true:
          BoundParameters[key] = SwitchParameter.Present;
          break;

        default:
          BoundParameters[key] = value;
          break;
      }
    }
  }
}
