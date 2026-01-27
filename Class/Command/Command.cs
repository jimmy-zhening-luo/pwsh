namespace Module.Command;

public abstract class CoreCommand : PSCmdlet, System.IDisposable
{
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

  private protected PowerShell PS => powershell ??= CreatePS();
  private PowerShell? powershell;

  [CodeAnalysis.MemberNotNullWhenAttribute(
    true,
    "powershell"
  )]
  private protected bool Initialized => powershell != null;

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
    }
    else
    {
      CleanRunspace();
    }
  }

  protected sealed override void EndProcessing()
  {
    if (Interactive)
    {
      AfterEndProcessing();
    }

    Dispose();
  }

  protected sealed override void StopProcessing() => Dispose();

  private protected virtual bool ValidateParameters() => true;

  private protected virtual void TransformParameters()
  { }

  private protected virtual void BeforeBeginProcessing()
  { }

  private protected virtual void AfterEndProcessing()
  { }

  private protected virtual void Clean()
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

  private protected void CleanRunspace()
  {
    if (Initialized)
    {
      powershell.Dispose();
      powershell = null;
    }
  }

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
    string psPath = Pwd(
      path
    );

    return type switch
    {
      FileSystemItemType.File => File.Exists(
        psPath
      ),
      FileSystemItemType.Directory => Directory.Exists(
        psPath
      ),
      _ => Exists(
        psPath
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

  private void Dispose(
    bool disposing
  )
  {
    if (!disposed)
    {
      if (disposing)
      {
        Clean();
        CleanRunspace();
      }

      disposed = true;
    }
  }
}
