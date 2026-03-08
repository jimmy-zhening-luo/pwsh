namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  bool AcceptsPipelineInput = default,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected virtual Dictionary<string, object?> CoercedParameters { get; } = [];

  private protected virtual object? PipelineInput { get; }

  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
    true,
    nameof(PipelineInput)
  )]
  private bool InPipeline { get; set; }

  private protected virtual void TransformArguments()
  { }

  private protected virtual void TransformPipelineInput()
  { }

  private protected sealed override void Preprocess()
  {
    InPipeline = false;

    CoerceParameters();

    TransformArguments();

    if (AcceptsPipelineInput && MyInvocation.ExpectingInput)
    {
      InPipeline = true;
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
    if (InPipeline)
    {
      TransformPipelineInput();

      ProcessSteppablePipeline(PipelineInput);
    }
    else
    {
      ProcessSteppablePipeline();
    }
  }

  private protected sealed override void Postprocess()
  {
    EndSteppablePipeline();
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
