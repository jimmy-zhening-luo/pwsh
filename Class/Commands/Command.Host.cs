namespace Module.Commands;

public partial class CoreCommand
{
  private class PowerShellHost : System.IDisposable
  {
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
    private PowerShell? powershell = PowerShell.Create(
      RunspaceMode.CurrentRunspace
    );

    private SteppablePipeline? Pipeline
    {
      get
      {
        System.ObjectDisposedException.ThrowIf(Disposed, this);

        return pipeline;
      }
      set => pipeline = value;
    }
    private SteppablePipeline? pipeline;

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

    internal PowerShell AddParameter(string parameterName) => PS.AddParameter(parameterName);
    internal PowerShell AddParameter(
      string parameterName,
      object value
    ) => PS.AddParameter(
      parameterName,
      value
    );

    internal PowerShell AddParameters(System.Collections.IList parameters) => PS.AddParameters(parameters);
    internal PowerShell AddParameters(System.Collections.IDictionary parameters) => PS.AddParameters(parameters);

    internal PowerShell AddStatement() => PS.AddStatement();

    internal PowerShell AddScript(string script) => PS.AddScript(script);

    internal System.Collections.ObjectModel.Collection<PSObject> InvokePowerShell() => PS.Invoke();
    internal System.Collections.ObjectModel.Collection<T> InvokePowerShell<T>() => PS.Invoke<T>();

    internal void ClearCommands() => PS.Commands.Clear();

    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(Pipeline)
    )]
    internal void BeginSteppablePipeline(CoreCommand owner)
    {
      if (Pipeline is not null)
      {
        EndSteppablePipeline();
      }

      Pipeline = PS.GetSteppablePipeline();

      Pipeline.Begin(owner);
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(Pipeline)
    )]
    internal void ProcessSteppablePipeline(CoreCommand owner)
    {
      if (Pipeline is null)
      {
        BeginSteppablePipeline(owner);
      }

      _ = Pipeline.Process();
    }
    [System.Diagnostics.CodeAnalysis.MemberNotNull(
      nameof(Pipeline)
    )]
    internal void ProcessSteppablePipeline(
      CoreCommand owner,
      object input
    )
    {
      if (Pipeline is null)
      {
        BeginSteppablePipeline(owner);
      }

      _ = Pipeline.Process(input);
    }

    internal void EndSteppablePipeline()
    {
      if (Pipeline is not null)
      {
        Pipeline.End();
        Pipeline.Clean();
        Pipeline.Dispose();
      }

      Pipeline = default;
    }

    private void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          EndSteppablePipeline();

          PS.Dispose();
          powershell = default;
        }

        Disposed = true;
      }
    }
  }
}
