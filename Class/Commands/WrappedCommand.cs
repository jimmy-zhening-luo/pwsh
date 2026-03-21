namespace PowerModule.Commands;

abstract public class WrappedCommand(
  string WrappedCommandName,
  CommandTypes CommandType = CommandTypes.Cmdlet,
  bool SkipSsh = default
) : CoreCommand(SkipSsh)
{
  private protected delegate (
    string Name,
    object Value
  ) PipelineInputSource();
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

  virtual private protected void TransformParameters()
  { }

  virtual private protected void TransformPipelineInput()
  { }

  sealed override private protected void Preprocess()
  {
    TransformParameters();

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

    if (
      PipelineInput is not null
      && MyInvocation.ExpectingInput
    )
    {
      InPipeline = true;
    }
    else
    {
      TransformPipelineInput();

      if (
        PipelineInput is not null
        && PipelineInput() is
        {
          Name: var name,
          Value: { } value
        }
      )
      {
        SetBoundParameter(
          name,
          value
        );
      }
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
        PipelineInput().Value
      );
    }
    else
    {
      ProcessSteppablePipeline();
    }
  }

  sealed override private protected void Postprocess() => EndSteppablePipeline();
}
