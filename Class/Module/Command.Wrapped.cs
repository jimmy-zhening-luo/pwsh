namespace Module
{
  using System.Management.Automation;

  public abstract class WrappedCommand : CoreCommand
  {
    protected static bool NoSsh = false;

    protected SteppablePipeline steppablePipeline = null;

    protected abstract string WrappedCommandName();

    protected virtual bool BeforeBeginProcessing() => true;

    protected virtual void BeforeEndProcessing() { }

    protected override void BeginProcessing()
    {
      if (
        !NoSsh
        && BeforeBeginProcessing()
      )
      {
        Begin(
          AddCommand(
            WrappedCommandName()
          )
            .AddParameters(
              BoundParameters()
            )
        );
      }
    }

    protected void Begin(PowerShell ps)
    {
      steppablePipeline = ps.GetSteppablePipeline();
      steppablePipeline.Begin(this);
    }

    protected override void ProcessRecord()
    {
      steppablePipeline?.Process();
    }

    protected override void EndProcessing()
    {
      BeforeEndProcessing();
      Clean();
    }

    private void Clean()
    {
      if (steppablePipeline != null)
      {
        steppablePipeline.Clean();
        steppablePipeline.Dispose();
        steppablePipeline = null;
      }
    }
  }
}
