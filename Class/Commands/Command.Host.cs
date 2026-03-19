namespace PowerModule.Commands;

partial class CoreCommand
{
  sealed class PowerShellHost : System.IDisposable
  {
    ~PowerShellHost()
    {
      Dispose(false);
    }

    PowerShell PS
    {
      get
      {
        System.ObjectDisposedException.ThrowIf(Disposed, this);

        return powershell;
      }
    }
    PowerShell? powershell = PowerShell.Create(
      RunspaceMode.CurrentRunspace
    );

    SteppablePipeline? Pipeline
    {
      get
      {
        System.ObjectDisposedException.ThrowIf(Disposed, this);

        return field;
      }
      set;
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(
      default,
      nameof(powershell)
    )]
    bool Disposed
    { get; set; }

    public void Dispose()
    {
      Dispose(true);

      System.GC.SuppressFinalize(this);
    }

    internal PowerShell AddStatement() => PS.AddStatement();

    internal PowerShell AddCommand(CommandInfo command) => PS.AddCommand(command);

    internal PowerShell AddParameter(string parameterName) => PS.AddParameter(parameterName);
    internal PowerShell AddParameter(
      string parameterName,
      object value
    ) => PS.AddParameter(
      parameterName,
      value
    );

    internal PowerShell AddParameters(Dictionary<string, object> parameters) => PS.AddParameters(parameters);

    internal PowerShell AddScript(string script) => PS.AddScript(script);

    internal Collection<PSObject> InvokePowerShell() => PS.Invoke();
    internal Collection<T> InvokePowerShell<T>() => PS.Invoke<T>();

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
        _ = Pipeline.End();
        Pipeline.Clean();
        Pipeline.Dispose();
      }

      Pipeline = default;
    }

    void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          EndSteppablePipeline();

          powershell.Dispose();
          powershell = default;
        }

        Disposed = true;
      }
    }
  }
}
