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

    SteppablePipeline.Begin(this);
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
        _ = SteppablePipeline.Process(pipelineInput);
      }
      else
      {
        _ = SteppablePipeline.Process();
      }
    }
    else
    {
      _ = SteppablePipeline.Process();
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
      if (value is null)
      {
        _ = BoundParameters.Remove(key);
      }
      else
      {
        BoundParameters[key] = value;
      }
    }
  }
}
