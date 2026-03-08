namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  string? PipelineInputParameterName = default,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected virtual Dictionary<string, object?> CoercedParameters { get; } = [];

  private string? PipelineInputParameterName { get; } = PipelineInputParameterName;

  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
    true,
    nameof(PipelineInputParameterName)
  )]
  private bool Piped { get; set; }

  private protected virtual void TransformArguments()
  { }

  private protected virtual void TransformPipelineInput()
  { }

  private protected sealed override void Preprocess()
  {
    CoerceParameters();

    TransformArguments();

    if (
      PipelineInputParameterName is not null
      && !BoundParameters.ContainsKey(
        PipelineInputParameterName
      )
    )
    {
      Piped = true;
    }
    else
    {
      TransformPipelineInput();
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
    if (Piped)
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
