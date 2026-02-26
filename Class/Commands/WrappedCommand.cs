namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  bool SkipSsh = default,
  string PipelineInputParameterName = "",
  CommandTypes CommandType = CommandTypes.Cmdlet
) : CoreCommand(
  SkipSsh
)
{
  private protected bool Piped;

  private SteppablePipeline? steppablePipeline = default;

  private protected virtual void TransformArguments()
  { }

  private protected virtual void TransformPipelineInput()
  { }

  private protected sealed override void Preprocess()
  {
    TransformArguments();

    if (
      string.IsNullOrEmpty(
        PipelineInputParameterName
      )
      || BoundParameters.ContainsKey(
        PipelineInputParameterName
      )
    )
    {
      TransformPipelineInput();
    }
    else
    {
      Piped = true;
    }

    AddCommand(
      WrappedCommandName,
      CommandType
    )
      .AddParameters(
        BoundParameters
      );

    steppablePipeline = PS.GetSteppablePipeline();

    steppablePipeline.Begin(
      this
    );
  }

  private protected sealed override void Processor()
  {
    if (steppablePipeline is not null)
    {
      if (
        Piped
        && BoundParameters.ContainsKey(
          PipelineInputParameterName
        )
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
          steppablePipeline.Process(
            pipelineInput
          );
        }
        else
        {
          steppablePipeline.Process();
        }
      }
      else
      {
        steppablePipeline.Process();
      }
    }
  }

  private protected sealed override void Postprocess()
  { }

  private protected sealed override void CleanResources()
  {
    if (steppablePipeline is not null)
    {
      steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();

      steppablePipeline = default;
    }
  }
}
