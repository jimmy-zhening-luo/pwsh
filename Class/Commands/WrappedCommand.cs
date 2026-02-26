namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  string PipelineInputParameterName = "",
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected bool Piped;

  private SteppablePipeline? steppablePipeline = default;

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

    steppablePipeline = PS.GetSteppablePipeline();

    steppablePipeline.Begin(this);
  }

  private protected sealed override void Process()
  {
    if (steppablePipeline is not null)
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
          _ = steppablePipeline.Process(pipelineInput);
        }
        else
        {
          _ = steppablePipeline.Process();
        }
      }
      else
      {
        _ = steppablePipeline.Process();
      }
    }
  }

  private protected sealed override void Postprocess()
  { }

  private protected sealed override void CleanResources()
  {
    if (steppablePipeline is not null)
    {
      _ = steppablePipeline.End();

      steppablePipeline.Clean();
      steppablePipeline.Dispose();

      steppablePipeline = default;
    }
  }

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
