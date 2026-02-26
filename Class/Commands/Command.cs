namespace Module.Commands;

public abstract partial class CoreCommand(
  bool SkipSsh = default
) : PSCmdlet, System.IDisposable
{
  private enum CommandLifecycle
  {
    NotStarted,
    Processing,
    Stopped
  }

  private protected record Locator(
    string Root = "",
    string Subpath = ""
  );

  private CommandLifecycle stage;

  private uint steps;

  private bool disposed;

  ~CoreCommand()
  {
    Dispose(
      default
    );
  }

  private protected virtual Locator LocationRecord => new();

  private protected bool UsingCurrentLocation => LocationRecord is
  {
    Root: "",
    Subpath: ""
  };

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= PowerShellHost.Create();
  private PowerShell? powershell;

  private bool ContinueProcessing => !disposed
    && stage is not CommandLifecycle.Stopped
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

    if (ContinueProcessing)
    {
      Preprocess();

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
        $"<PROCESS:{steps}>"
      );

      Processor();

      WriteDebug(
        $"</PROCESS:{steps}>"
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
      Postprocess();
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

  private protected virtual void Preprocess()
  { }

  private protected virtual void Processor()
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

  private protected string Reanchor(
    string typedPath = ""
  ) => System.IO.Path.GetFullPath(
    Client.FileSystem.PathString.Normalize(
      typedPath
    ),
    System.IO.Path.GetFullPath(
      LocationRecord.Subpath,
      LocationRecord is
      {
        Root: ""
      }
        ? Pwd()
        : LocationRecord.Root
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
    powershell = default;
  }
}
