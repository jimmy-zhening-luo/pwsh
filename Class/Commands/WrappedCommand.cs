namespace Module.Commands;

public abstract class WrappedCommand(
  string WrappedCommandName,
  bool SkipSsh = false,
  CommandTypes CommandType = CommandTypes.Cmdlet
) : CoreCommand(
  SkipSsh
)
{
  private SteppablePipeline? steppablePipeline = null;

  private protected sealed override void BeforeBeginProcessing()
  {
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
    steppablePipeline?.Process();
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
