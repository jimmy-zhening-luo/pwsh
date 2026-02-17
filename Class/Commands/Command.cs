namespace Module.Commands;

public abstract class CoreCommand(
  bool SkipSsh = false
) : PSCmdlet, System.IDisposable
{
  private protected enum FileSystemItemType
  {
    Any,
    File,
    Directory
  }

  private enum CommandLifecycle
  {
    NotStarted,
    Initialized,
    Processing,
    Skipped,
    Stopped
  }

  private CommandLifecycle stage;

  private bool disposed;

  ~CoreCommand() => Dispose(
    false
  );

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
    if (stage == CommandLifecycle.NotStarted)
    {
      stage = CommandLifecycle.Initialized;
    }

    if (ContinueProcessing)
    {
      TransformParameters();

      if (ValidateParameters())
      {
        BeforeBeginProcessing();

        stage = CommandLifecycle.Processing;
      }
      else
      {
        stage = CommandLifecycle.Skipped;
      }
    }
    else
    {
      stage = CommandLifecycle.Stopped;
    }
  }

  protected sealed override void ProcessRecord()
  {
    if (ContinueProcessing)
    {
      ProcessRecordAction();
    }
  }

  protected sealed override void EndProcessing()
  {
    if (stage == CommandLifecycle.NotStarted)
    {
      stage = CommandLifecycle.Skipped;
    }

    if (ContinueProcessing)
    {
      AfterEndProcessing();
    }
    else if (stage == CommandLifecycle.Skipped)
    {
      DefaultAction();
    }

    StopProcessing();
  }

  protected sealed override void StopProcessing()
  {
    stage = CommandLifecycle.Stopped;

    Dispose();
  }

  private protected virtual bool ValidateParameters() => true;

  private protected virtual void TransformParameters()
  { }

  private protected virtual void BeforeBeginProcessing()
  { }

  private protected virtual void ProcessRecordAction()
  { }

  private protected virtual void AfterEndProcessing()
  { }

  private protected virtual void DefaultAction()
  { }

  private protected virtual void CleanResources()
  { }

  private protected bool IsPresent(
    string parameterName
  ) => BoundParameters.ContainsKey(
    parameterName
  );

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

  private protected bool TestPath(
    string path,
    FileSystemItemType type = FileSystemItemType.Any
  )
  {
    string absolutePath = Pwd(
      path
    );

    return type switch
    {
      FileSystemItemType.File => System.IO.File.Exists(
        absolutePath
      ),
      FileSystemItemType.Directory => System.IO.Directory.Exists(
        absolutePath
      ),
      _ => System.IO.Path.Exists(
        absolutePath
      )
    };
  }

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

  private protected string Drive(
    string subpath = ""
  ) => System.IO.Path.GetFullPath(
    Client.FileSystem.PathString.Normalize(
      subpath
    ),
    SessionState.Drive.Current.Root
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
    if (powershell != null)
    {
      powershell.Dispose();
      powershell = null;
    }
  }
}
