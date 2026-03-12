namespace PowerModule.Commands;

abstract public class WrappedCommand(
  string WrappedCommandName,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected delegate object PipelineInputSource();
  virtual private protected PipelineInputSource? PipelineInput
  { get; }

  virtual private protected Dictionary<string, object?> CoercedParameters
  { get; } = [];

  [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
    true,
    nameof(PipelineInput)
  )]
  bool InPipeline
  { get; set; }

  virtual private protected void TransformArguments()
  { }

  virtual private protected void TransformPipelineInput()
  { }

  sealed override private protected void Preprocess()
  {
    CoerceParameters();

    TransformArguments();

    if (PipelineInput is not null && MyInvocation.ExpectingInput)
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

      ProcessSteppablePipeline(
        PipelineInput()
      );
    }
    else
    {
      ProcessSteppablePipeline();
    }
  }

  sealed override private protected void Postprocess() => EndSteppablePipeline();

  void CoerceParameters()
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
