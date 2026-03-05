namespace Module.Commands;

public abstract partial class CoreCommand
{
  private class PowerShellHost : System.IDisposable
  {
    private PowerShell? powershell;
    private SteppablePipeline? steppablePipeline;

    internal PowerShellHost()
    {
      powershell = Module.Create();
    }

    ~PowerShellHost()
    {
      Dispose(default);
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
      default,
      nameof(powershell)
    )]
    private bool Disposed { get; set; }

    public void Dispose()
    {
      Dispose(true);

      System.GC.SuppressFinalize(this);
    }

    private void CleanPipeline()
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is not null)
      {
        _ = steppablePipeline.End();
        steppablePipeline.Clean();
        steppablePipeline.Dispose();
      }

      steppablePipeline = default;
    }

    private void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          CleanPipeline();

          powershell.Dispose();
          powershell = default;
        }

        Disposed = true;
      }
    }
  }
}
