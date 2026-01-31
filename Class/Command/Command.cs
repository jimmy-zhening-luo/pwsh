namespace Module.Command;

public abstract class CoreCommand : PSCmdlet, System.IDisposable
{
  private bool doProcess;
  private bool disposed;

  ~CoreCommand() => Dispose(false);

  private protected virtual string Location => string.Empty;

  private protected virtual string LocationSubpath => string.Empty;

  private protected virtual bool NoSsh => false;

  private protected bool Here => string.IsNullOrEmpty(
    Location
  )
    && string.IsNullOrEmpty(
      LocationSubpath
    );

  private protected bool Interactive => !NoSsh
    || !Ssh;

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= Terminal.CreatePS();
  private PowerShell? powershell;

  public void Dispose()
  {
    Dispose(true);

    System.GC.SuppressFinalize(this);
  }

  protected sealed override void BeginProcessing()
  {
    if (
      Interactive
      && ValidateParameters()
    )
    {
      TransformParameters();

      BeforeBeginProcessing();

      doProcess = true;
    }
  }

  protected sealed override void ProcessRecord()
  {
    if (doProcess)
    {
      ProcessRecordAction();
    }
  }

  protected sealed override void EndProcessing()
  {
    if (Interactive)
    {
      AfterEndProcessing();
    }

    StopProcessing();
  }

  protected sealed override void StopProcessing() => Dispose();

  private protected virtual bool ValidateParameters() => true;

  private protected virtual void TransformParameters()
  { }

  private protected virtual void BeforeBeginProcessing()
  { }

  private protected virtual void ProcessRecordAction()
  { }

  private protected virtual void AfterEndProcessing()
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
  ) => GetFullPath(
    Normalize(
      typedPath
    ),
    GetFullPath(
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
      FileSystemItemType.File => File.Exists(
        absolutePath
      ),
      FileSystemItemType.Directory => Directory.Exists(
        absolutePath
      ),
      _ => Exists(
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
  ) => GetFullPath(
    Normalize(
      subpath
    ),
    SessionState.Path.CurrentLocation.Path
  );

  private protected string Drive(
    string subpath = ""
  ) => GetFullPath(
    Normalize(
      subpath
    ),
    SessionState.Drive.Current.Root
  );

  [CodeAnalysis.DoesNotReturn]
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

  [CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    string id,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = null
  ) => ThrowTerminatingError(
    new ErrorRecord(
      exception,
      id,
      category,
      target
    )
  );

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
