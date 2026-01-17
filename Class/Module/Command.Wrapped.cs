namespace Module
{
  using System.Management.Automation;

  public abstract class WrappedCommand : CoreCommand
  {
    protected SteppablePipeline steppablePipeline = null;

    protected override void ProcessRecord()
    {
      steppablePipeline?.Process();
    }

    protected override void EndProcessing()
    {
      Clean();
    }

    protected void Clean()
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
