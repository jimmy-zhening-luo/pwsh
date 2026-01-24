namespace Module.Command;

public abstract class CoreCommand : PSCmdlet, System.IDisposable
{
  private bool disposed;

  ~CoreCommand() => Dispose(false);

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= CreatePS();
  private PowerShell? powershell;

  public void Dispose()
  {
    Dispose(true);

    System.GC.SuppressFinalize(this);
  }

  protected sealed override void EndProcessing()
  {
    AfterEndProcessing();

    Dispose();
  }

  protected sealed override void StopProcessing() => Dispose();

  private protected virtual void AfterEndProcessing()
  { }

  private protected virtual void Clean()
  { }

  private protected bool IsPresent(
    string parameterName
  ) => BoundParameters.ContainsKey(
    parameterName
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
    SessionState
      .InvokeCommand
      .GetCommand(
        command,
        commandType
      )
  );

  [DoesNotReturn]
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

  [DoesNotReturn]
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

  private protected Collection<PSObject> Call(
    string nativeCommand,
    string verb,
    string[] arguments,
    CommandTypes commandType = CommandTypes.Application
  ) => Call(
    nativeCommand,
    [
      verb,
      .. arguments
    ],
    commandType
  );

  private protected Collection<PSObject> Call(
    string nativeCommand,
    string[] arguments,
    CommandTypes commandType = CommandTypes.Application
  )
  {
    using var ps = CreatePS();

    AddCommand(
      ps,
      nativeCommand,
      commandType
    );

    foreach (string argument in arguments)
    {
      ps.AddArgument(
        argument
      );
    }

    return ps.Invoke();
  }

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
      _ => Path.Exists(
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
  ) => Path.GetFullPath(
    subpath,
    SessionState.Path.CurrentLocation.Path
  );

  private protected string Drive(
    string subpath = ""
  ) => Path.GetFullPath(
    subpath,
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

        if (powershell != null)
        {
          powershell.Dispose();
          powershell = null;
        }
      }

      disposed = true;
    }
  }
}
