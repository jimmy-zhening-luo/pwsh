namespace Module.Commands;

public abstract partial class CoreCommand
{
  private class PowerShellHost : System.IDisposable
  {
    private SteppablePipeline? steppablePipeline;

    ~PowerShellHost()
    {
      Dispose(default);
    }

    internal bool HadErrors => PS.HadErrors;

    private PowerShell PS
    {
      get
      {
        System.ObjectDisposedException.ThrowIf(Disposed, this);

        return powershell;
      }
    }
    private PowerShell? powershell = Module.Create();

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

    internal PowerShell AddCommand(CommandInfo command) => PS.AddCommand(command);

    internal PowerShell AddParameters(System.Collections.IList parameters) => PS.AddParameters(parameters);
    internal PowerShell AddParameters(System.Collections.IDictionary parameters) => PS.AddParameters(parameters);

    internal PowerShell AddStatement() => PS.AddStatement();

    internal PowerShell AddScript(string script) => PS.AddScript(script);

    internal System.Collections.ObjectModel.Collection<PSObject> InvokePowerShell() => PS.Invoke();
    internal System.Collections.ObjectModel.Collection<T> InvokePowerShell<T>() => PS.Invoke<T>();

    internal void ClearCommands() => PS.Commands.Clear();

    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(steppablePipeline)
    )]
    internal void BeginSteppablePipeline(CoreCommand owner)
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is not null)
      {
        EndSteppablePipeline();
      }

      steppablePipeline = PS.GetSteppablePipeline();

      steppablePipeline.Begin(owner);
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(steppablePipeline)
    )]
    internal void ProcessSteppablePipeline(CoreCommand owner)
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is null)
      {
        BeginSteppablePipeline(owner);
      }

      _ = steppablePipeline.Process();
    }
    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(steppablePipeline)
    )]
    internal void ProcessSteppablePipeline(
      CoreCommand owner,
      object input
    )
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is null)
      {
        BeginSteppablePipeline(owner);
      }

      _ = steppablePipeline.Process(input);
    }

    internal void EndSteppablePipeline()
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is not null)
      {
        _ = steppablePipeline.End();
      }

      CleanPipeline();
    }

    private void CleanPipeline()
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      if (steppablePipeline is not null)
      {
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

          PS.Dispose();
          powershell = default;
        }

        Disposed = true;
      }
    }
  }
}
