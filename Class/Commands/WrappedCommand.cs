namespace PowerModule.Commands;

abstract public class WrappedCommand(
  string WrappedCommandName,
  bool AcceptsPipelineInput = default,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  virtual private protected object? PipelineInput
  { get; }

  virtual private protected Dictionary<string, object?> CoercedParameters
  { get; } = [];

  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
    true,
    nameof(PipelineInput)
  )]
  private bool InPipeline
  { get; set; }

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
    );
    _ = AddBoundParameters();

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
        var parameter,
        var value
      ) in CoercedParameters
    )
    {
      SetBoundParameter(
        parameter,
        value
      );
    }
  }
}
