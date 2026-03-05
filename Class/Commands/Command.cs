namespace Module.Commands;

public abstract partial class CoreCommand(bool SkipSsh = default) : PSCmdlet, System.IDisposable
{
  private protected record Locator(
    string Root = "",
    string Subpath = ""
  )
  {
    internal bool IsEmpty => this is
    {
      Root: "",
      Subpath: "",
    };

    internal bool IsRooted => this is
    {
      Root: not "",
    };
  }

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
    private PowerShell? powershell = Module.Create();

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
        _ = Pipeline.End();
      }

      CleanPipeline();
    }

    private void CleanPipeline()
    {
      if (Pipeline is not null)
      {
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
          CleanPipeline();

          PS.Dispose();
          powershell = default;
        }

        Disposed = true;
      }
    }
  }

  ~CoreCommand()
  {
    Dispose(default);
  }

  private protected virtual Locator Location => new();

  private protected bool InCurrentLocation => Location.IsEmpty;

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected bool HadErrors => pshost?.HadErrors ?? default;

  private protected bool HadNativeError => PSVariable<int>("LASTEXITCODE") is not (0 or 1);

  private PowerShellHost PSHost
  {
    get
    {
      System.ObjectDisposedException.ThrowIf(Disposed, this);

      return pshost ??= new();
    }
  }
  private PowerShellHost? pshost;

  private bool BlockedBySsh => !SkipSsh
    || !Client.Environment.Known.Variable.InSsh;

  private bool Disposed { get; set; }

  public void Dispose()
  {
    Dispose(true);

    System.GC.SuppressFinalize(this);
  }

  protected sealed override void BeginProcessing()
  {
    System.ObjectDisposedException.ThrowIf(Disposed, this);

    WriteDebug("<BEGIN>");
    if (BlockedBySsh)
    {
      Preprocess();
    }
    WriteDebug("</BEGIN>");
  }

  protected sealed override void ProcessRecord()
  {
    System.ObjectDisposedException.ThrowIf(Disposed, this);

    if (BlockedBySsh)
    {
      WriteDebug("<PROCESS>");
      Process();

      WriteDebug("</PROCESS>");
    }
  }

  protected sealed override void EndProcessing()
  {
    System.ObjectDisposedException.ThrowIf(Disposed, this);

    WriteDebug("<END>");
    if (BlockedBySsh)
    {
      Postprocess();
    }
    WriteDebug("</END>");

    StopProcessing();
  }

  protected sealed override void StopProcessing()
  {
    Dispose();
  }

  private protected virtual void Preprocess()
  { }

  private protected virtual void Process()
  { }

  private protected virtual void Postprocess()
  { }

  private protected PowerShell AddCommand(
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => PSHost.AddCommand(
    SessionState.InvokeCommand.GetCommand(
      command,
      commandType
    )
  );

  private protected PowerShell AddParameter(string parameterName) => PSHost.AddParameter(parameterName);
  private protected PowerShell AddParameter(
    string parameterName,
    object value
  ) => PSHost.AddParameter(
    parameterName,
    value
  );

  private protected PowerShell AddParameters(System.Collections.IList parameters) => PSHost.AddParameters(parameters);
  private protected PowerShell AddParameters(System.Collections.IDictionary parameters) => PSHost.AddParameters(parameters);

  private protected PowerShell AddStatement() => PSHost.AddStatement();

  private protected PowerShell AddScript(string script) => PSHost.AddScript(script);

  private protected System.Collections.ObjectModel.Collection<PSObject> InvokePowerShell() => PSHost.InvokePowerShell();
  private protected System.Collections.ObjectModel.Collection<T> InvokePowerShell<T>() => PSHost.InvokePowerShell<T>();

  private protected void ClearCommands() => PSHost.ClearCommands();

  private protected void BeginSteppablePipeline() => PSHost.BeginSteppablePipeline(this);

  private protected void ProcessSteppablePipeline() => PSHost.ProcessSteppablePipeline(this);
  private protected void ProcessSteppablePipeline(object input) => PSHost.ProcessSteppablePipeline(
    this,
    input
  );

  private protected void EndSteppablePipeline() => PSHost.EndSteppablePipeline();

  private protected string[] ReanchorPath(IEnumerable<string> paths)
  {
    List<string> reanchoredPaths = [];

    foreach (var path in paths)
    {
      reanchoredPaths.Add(ReanchorPath(path));
    }

    if (reanchoredPaths is [])
    {
      reanchoredPaths.Add(ReanchorPath());
    }

    return [.. reanchoredPaths];
  }
  private protected string ReanchorPath(string path) => Client.File.PathString.FullPathLocationRelative(
    ReanchorPath(),
    path
  );
  private protected string ReanchorPath() => Location.IsRooted
    ? Client.File.PathString.FullPathLocationRelative(
        Location.Root,
        Location.Subpath
      )
    : Pwd(Location.Subpath);

  private protected object? PSVariable(string name) => SessionState
    .PSVariable
    .GetValue(name, default);
  private protected T? PSVariable<T>(string name) => SessionState
    .PSVariable
    .GetValue(name) is { } value
    ? (T)value
    : default;

  private protected string Pwd() => SessionState.Path.CurrentLocation.Path;
  private protected string Pwd(string path) => Client.File.PathString.FullPathLocationRelative(
    SessionState.Path.CurrentLocation.Path,
    path
  );

  private protected string Drive() => SessionState.Drive.Current.Root;
  private protected string Drive(string path) => Client.File.PathString.FullPathLocationRelative(
    SessionState.Drive.Current.Root,
    path
  );

  private protected void WriteProgress(
    int total,
    int progress,
    string activity = "Progress",
    int activityId = default
  ) => WriteProgress(
    new(
      activityId,
      activity,
      $"{progress}/{total}"
    )
    {
      PercentComplete = 100 * progress / total,
      RecordType = progress == total
        ? ProgressRecordType.Completed
        : ProgressRecordType.Processing,
    }
  );

  private protected void WriteInformation(object log) => base.WriteInformation(
    new InformationRecord(
      log,
      GetName()
    )
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void ThrowError(
    string message,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default,
    string id = ""
  ) => ThrowError(
    new System.Exception(message),
    category,
    target,
    id
  );
  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void ThrowError(
    System.Exception exception,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default,
    string id = ""
  )
  {
    StopProcessing();

    ThrowTerminatingError(
      new(
        exception,
        $"{GetName()}Exception{id}",
        category,
        target
      )
    );
  }

  private string GetName() => GetType() is
  {
    FullName: var name
  } type
    ? name ?? type.ToString()
    : string.Empty;

  private void Dispose(bool disposing)
  {
    if (!Disposed)
    {
      if (disposing)
      {
        if (pshost is not null)
        {
          PSHost.Dispose();
          pshost = default;
        }
      }

      Disposed = true;
    }
  }
}
