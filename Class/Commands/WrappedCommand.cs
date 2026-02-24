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
  private SteppablePipeline? steppablePipeline = null;

  private bool piped;

  private protected sealed override void BeforeBeginProcessing()
  {
    if (
      !string.IsNullOrEmpty(
        PipelineInputParameterName
      )
      && !BoundParameters.ContainsKey(
        PipelineInputParameterName
      )
    )
    {
      piped = true;
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

  private protected sealed override void ProcessRecordAction()
  {
    if (steppablePipeline is not null)
    {
      if (piped)
      {
        var input = BoundParameters.TryGetValue(
          PipelineInputParameterName,
          out var value
        )
          ? value
          : null;

        if (input is null)
        {
          steppablePipeline.Process();
        }
        else
        {
          steppablePipeline.Process(
            input
          );
        }
      }
      else
      {
        steppablePipeline.Process();
      }
    }
  }

  private protected sealed override void AfterEndProcessing()
  { }

  private protected sealed override void CleanResources()
  {
    if (steppablePipeline != null)
    {
      steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();

      steppablePipeline = null;
    }
  }
}
