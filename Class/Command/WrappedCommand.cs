namespace Module.Command;

public abstract class WrappedCommand(
  string WrappedCommandName
) : CoreCommand
{
  private SteppablePipeline? steppablePipeline = null;

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

  protected sealed override void ProcessRecord()
  {
    steppablePipeline?.Process();
  }

  private protected virtual void Default()
  { }

  private protected sealed override void AfterEndProcessing()
  {
    Default();
  }

  private protected sealed override void Clean()
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
