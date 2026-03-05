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

  private enum CommandState
  {
    Alive,
    Stopped,
    Disposed,
  }
  private CommandState state;

  ~CoreCommand()
  {
    Dispose(default);
  }

  private protected virtual Locator Location => new();

  private protected bool InCurrentLocation => Location.IsEmpty;

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected bool HadErrors => powershell?.HadErrors ?? default;

  private protected bool HadNativeError => PSVariable<int>("LASTEXITCODE") is not (0 or 1);

  private PowerShell PS => powershell ??= Module.Create();
  private PowerShell? powershell;

  private SteppablePipeline? steppablePipeline;

  private bool BlockedBySsh => SkipSsh
    && Client.Environment.Known.Variable.InSsh;

  private bool ContinueProcessing => state is CommandState.Alive
    && !BlockedBySsh;

  public void Dispose()
  {
    Dispose(true);

    System.GC.SuppressFinalize(this);
  }

  protected sealed override void BeginProcessing()
  {
    WriteDebug("<BEGIN>");
    if (ContinueProcessing)
    {
      Preprocess();
    }
    WriteDebug("</BEGIN>");
  }

  protected sealed override void ProcessRecord()
  {
    if (ContinueProcessing)
    {
      WriteDebug("<PROCESS>");
      Process();

      WriteDebug("</PROCESS>");
    }
  }

  protected sealed override void EndProcessing()
  {
    WriteDebug("<END>");
    if (ContinueProcessing)
    {
      Postprocess();
    }
    WriteDebug("</END>");

    StopProcessing();
  }

  protected sealed override void StopProcessing()
  {
    state = CommandState.Stopped;

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
  ) => PS.AddCommand(
    SessionState.InvokeCommand.GetCommand(
      command,
      commandType
    )
  );

  private protected PowerShell AddParameter(string parameterName) => PS.AddParameter(parameterName);
  private protected PowerShell AddParameter(
    string parameterName,
    object value
  ) => PS.AddParameter(
    parameterName,
    value
  );

  private protected PowerShell AddParameters(System.Collections.IList parameters) => PS.AddParameters(parameters);
  private protected PowerShell AddParameters(System.Collections.IDictionary parameters) => PS.AddParameters(parameters);

  private protected PowerShell AddStatement() => PS.AddStatement();

  private protected PowerShell AddScript(string script) => PS.AddScript(script);

  private protected System.Collections.ObjectModel.Collection<PSObject> InvokePowerShell() => PS.Invoke();
  private protected System.Collections.ObjectModel.Collection<T> InvokePowerShell<T>() => PS.Invoke<T>();

  private protected void ClearCommands() => powershell
    ?.Commands
    .Clear();

  private protected void BeginSteppablePipeline()
  {
    if (state is CommandState.Alive)
    {
      if (steppablePipeline is not null)
      {
        CleanPipeline();
      }

      if (powershell is not null)
      {
        steppablePipeline = powershell.GetSteppablePipeline();

        steppablePipeline.Begin(this);
      }
    }
  }

  private protected void ProcessSteppablePipeline()
  {
    if (state is CommandState.Alive)
    {
      if (steppablePipeline is null)
      {
        BeginSteppablePipeline();
      }

      if (steppablePipeline is not null)
      {
        steppablePipeline.Process();
      }
    }
  }
  private protected void ProcessSteppablePipeline(object input)
  {
    if (state is CommandState.Alive)
    {
      if (steppablePipeline is null)
      {
        BeginSteppablePipeline();
      }

      if (steppablePipeline is not null)
      {
        steppablePipeline.Process(input);
      }
    }
  }

  private protected void EndSteppablePipeline() => CleanPipeline();

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

  private void CleanPipeline()
  {
    if (steppablePipeline is not null)
    {
      _ = steppablePipeline.End();
      steppablePipeline.Clean();
      steppablePipeline.Dispose();
      steppablePipeline = default;
    }
  }

  private void Clean()
  {
    CleanPipeline();

    powershell?.Dispose();
    powershell = default;
  }

  private void Dispose(bool disposing)
  {
    if (state is not CommandState.Disposed)
    {
      if (disposing)
      {
        Clean();
      }

      state = CommandState.Disposed;
    }
  }
}
