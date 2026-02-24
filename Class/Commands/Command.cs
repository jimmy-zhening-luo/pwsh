namespace Module.Commands;

public abstract partial class CoreCommand(
  bool SkipSsh = default
) : PSCmdlet, System.IDisposable
{
  private CommandLifecycle stage;

  private uint steps;

  private bool disposed;

  ~CoreCommand()
  {
    Dispose(
      false
    );
  }

  private protected virtual string Location => string.Empty;

  private protected virtual string LocationSubpath => string.Empty;

  private protected bool UsingCurrentLocation => string.IsNullOrEmpty(
    Location
  )
    && string.IsNullOrEmpty(
      LocationSubpath
    );

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= PowerShellHost.Create();
  private PowerShell? powershell;

  private bool ContinueProcessing => !disposed
    && (
      stage == CommandLifecycle.Initialized
      || stage == CommandLifecycle.Processing
    )
    && (
      !SkipSsh
      || !Client.Environment.Known.Variable.Ssh
    );

  public void Dispose()
  {
    Dispose(
      true
    );

    System.GC.SuppressFinalize(
      this
    );
  }

  protected sealed override void BeginProcessing()
  {
    WriteDebug(
      "<BEGIN>"
    );

    if (stage == CommandLifecycle.NotStarted)
    {
      stage = CommandLifecycle.Initialized;
    }

    steps = default;

    if (ContinueProcessing)
    {
      BeforeBeginProcessing();

      stage = CommandLifecycle.Processing;
    }
    else
    {
      Stop();
    }

    WriteDebug(
      "</BEGIN>"
    );
  }

  protected sealed override void ProcessRecord()
  {
    if (ContinueProcessing)
    {
      ++steps;

      WriteDebug(
        "<PROCESS:"
          + steps.ToString()
          + ">"
      );

      ProcessRecordAction();

      WriteDebug(
        "</PROCESS:"
          + steps.ToString()
          + ">"
      );
    }
  }

  protected sealed override void EndProcessing()
  {
    WriteDebug(
      "<END>"
    );

    if (ContinueProcessing)
    {
      AfterEndProcessing();
    }

    WriteDebug(
      "</END>"
    );
    StopProcessing();
  }

  protected sealed override void StopProcessing()
  {
    Stop();

    Dispose();
  }

  private protected virtual void BeforeBeginProcessing()
  { }

  private protected virtual void ProcessRecordAction()
  { }

  private protected virtual void AfterEndProcessing()
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

  private protected string Reanchor(
    string typedPath = ""
  ) => System.IO.Path.GetFullPath(
    Client.FileSystem.PathString.Normalize(
      typedPath
    ),
    System.IO.Path.GetFullPath(
      LocationSubpath,
      string.IsNullOrEmpty(
        Location
      )
        ? Pwd()
        : Location
    )
  );

  private protected PSObject Var(
    string variable
  ) => Var<PSObject>(
    variable
  );

  private protected T Var<T>(
    string variable
  ) => (T)SessionState
    .PSVariable
    .GetValue(
      variable
    );

  private protected string Pwd(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    Client.FileSystem.PathString.Normalize(
      subpath
    ),
    SessionState.Path.CurrentLocation.Path
  );

  private protected string CurrentDrive(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    Client.FileSystem.PathString.Normalize(
      subpath
    ),
    SessionState.Drive.Current.Root
  );

  private protected void WriteLog(
    object log
  ) => WriteLog(
    log,
    GetName()
  );

  private protected void WriteLog(
    object log,
    string source
  ) => WriteInformation(
    new InformationRecord(
      log,
      source
    )
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    string message,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = null
  ) => Throw(
    message,
    GetName() + "Exception",
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    string message,
    string id,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = null
  ) => Throw(
    new System.Exception(
      message
    ),
    id,
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = null
  ) => Throw(
    exception,
    GetName() + "Exception",
    category,
    target
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    string id,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = null
  )
  {
    Dispose();

    ThrowTerminatingError(
      new ErrorRecord(
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

  private void Stop() => stage = CommandLifecycle.Stopped;

  private void Dispose(
    bool disposing
  )
  {
    if (!disposed)
    {
      if (disposing)
      {
        CleanResources();
        Clean();
      }

      disposed = true;
    }
  }

  private void Clean()
  {
    powershell?.Dispose();
    powershell = null;
  }
}
