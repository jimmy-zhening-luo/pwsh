namespace PowerModule.Commands;

abstract public class WrappedCommand(
  string WrappedCommandName,
  string? PipelineInput = default,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  readonly string? PipelineInput = PipelineInput;

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
    TransformArguments();

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
        MyInvocation.BoundParameters[PipelineInput]
      );
    }
    else
    {
      ProcessSteppablePipeline();
    }
  }

  sealed override private protected void Postprocess() => EndSteppablePipeline();
}
