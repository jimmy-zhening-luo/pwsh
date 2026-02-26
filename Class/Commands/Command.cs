namespace Module.Commands;

public abstract class CoreCommand(
  bool SkipSsh = default
) : PSCmdlet, System.IDisposable
{
  private protected record Locator(
    string Root = "",
    string Subpath = ""
  );

  private uint steps;

  private bool stopped;

  private bool disposed;

  ~CoreCommand() => Dispose(default);

  private protected virtual Locator Location => new();

  private protected bool UsingDefaultLocation => Location is
  {
    Root: "",
    Subpath: ""
  };

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= PowerShellHost.Create();
  private PowerShell? powershell;

  private bool BlockedBySsh => SkipSsh
    && Client.Environment.Known.Variable.InSsh;

  private bool ContinueProcessing => !disposed
    && !stopped
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
    else
    {
      stopped = true;
    }

    WriteDebug("</BEGIN>");
  }

  protected sealed override void ProcessRecord()
  {
    if (ContinueProcessing)
    {
      ++steps;

      WriteDebug($"<PROCESS:{steps}>");

      Process();

      WriteDebug($"</PROCESS:{steps}>");
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
    stopped = true;

    Dispose();
  }

  private protected virtual void Preprocess()
  { }

  private protected virtual void Process()
  { }

  private protected virtual void Postprocess()
  { }

  private protected virtual void CleanResources()
  { }

  private protected CommandInfo GetCommand(
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => SessionState
    .InvokeCommand
    .GetCommand(
      command,
      commandType
    );

  private protected PowerShell AddCommand(
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => AddCommand(
    PS,
    command,
    commandType
  );

  private protected PowerShell AddCommand(
    PowerShell ps,
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => ps.AddCommand(
    GetCommand(
      command,
      commandType
    )
  );

  private protected string Reanchor(string path = "") => System.IO.Path.GetFullPath(
    Client.File.PathString.Normalize(path),
    System.IO.Path.GetFullPath(
      Location.Subpath,
      Location is
      {
        Root: ""
      }
        ? Pwd()
        : Location.Root
    )
  );

  private protected PSObject PSVariable(string variable) => PSVariable<PSObject>(variable);

  private protected T PSVariable<T>(string variable) => (T)SessionState
    .PSVariable
    .GetValue(variable);

  private protected string Pwd(string subpath = "") => System.IO.Path.GetFullPath(
    Client.File.PathString.Normalize(subpath),
    SessionState.Path.CurrentLocation.Path
  );

  private protected string CurrentDrive(string subpath = "") => System.IO.Path.GetFullPath(
    Client.File.PathString.Normalize(subpath),
    SessionState.Drive.Current.Root
  );

  private protected void WriteLog(object log) => WriteLog(
    log,
    GetName()
  );

  private protected void WriteLog(
    object log,
    string source
  ) => WriteInformation(
    new(
      log,
      source
    )
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    string message,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default
  ) => Throw(
    message,
    $"{GetName()}Exception",
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    string message,
    string id,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default
  ) => Throw(
    new System.Exception(message),
    id,
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default
  ) => Throw(
    exception,
    $"{GetName()}Exception",
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    string id,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default
  )
  {
    Dispose();

    ThrowTerminatingError(
      new(
        exception,
        id,
        category,
        target
      )
    );
  }

  private string GetName()
  {
    var type = GetType();

    return type.FullName
      ?? type.ToString();
  }

  private void Dispose(bool disposing)
  {
    if (!disposed)
    {
      if (disposing)
      {
        Clean();
      }

      disposed = true;
    }
  }

  private void Clean()
  {
    CleanResources();

    powershell?.Dispose();
    powershell = default;
  }
}
