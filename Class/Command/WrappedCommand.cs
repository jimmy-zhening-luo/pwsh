namespace Module.Command;

public abstract class WrappedCommand(
  string WrappedCommandName
) : CoreCommand
{
  private SteppablePipeline? steppablePipeline = null;

  private protected virtual void DefaultAction()
  { }

  private protected sealed override void BeforeBeginProcessing()
  {
    AddCommand(
      WrappedCommandName
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
  {
    DefaultAction();
  }

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
