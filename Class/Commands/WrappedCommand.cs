namespace Module.Commands;

abstract public class WrappedCommand(
  string WrappedCommandName,
  bool AcceptsPipelineInput = default,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  virtual private protected object? PipelineInput { get; }

  virtual private protected Dictionary<string, object?> CoercedParameters { get; } = [];

  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
    true,
    nameof(PipelineInput)
  )]
  private bool InPipeline { get; set; }

  virtual private protected void TransformArguments()
  { }

  virtual private protected void TransformPipelineInput()
  { }

  sealed override private protected void Preprocess()
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

  sealed override private protected void Process()
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

  sealed override private protected void Postprocess()
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
