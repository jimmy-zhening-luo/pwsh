namespace Module.Command;

using static Module.FileSystem.Path.Normalizer;

public abstract class CoreCommand : PSCmdlet, System.IDisposable
{
  public enum FileSystemItemType
  {
    Any,
    File,
    Directory
  }

  private bool disposed;

  ~CoreCommand() => Dispose(
    false
  );

  private protected virtual string Location => string.Empty;

  private protected virtual string LocationSubpath => string.Empty;

  private protected virtual bool SkipSsh => false;

  private bool ContinueProcessing
  {
    get => continueProcessing
      && (
        !SkipSsh
        || !Ssh
      );
    set => continueProcessing = value;
  }
  private bool continueProcessing;

  private protected bool Here => string.IsNullOrEmpty(
    Location
  )
    && string.IsNullOrEmpty(
      LocationSubpath
    );

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected PowerShell PS => powershell ??= CommandLine.Create();
  private PowerShell? powershell;

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
    continueProcessing = true;

    if (
      ContinueProcessing
      && ValidateParameters()
    )
    {
      TransformParameters();

      BeforeBeginProcessing();
    }
    else
    {
      continueProcessing = false;
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
    if (ContinueProcessing)
    {
      AfterEndProcessing();
    }
    else
    {
      DefaultAction();
    }

    StopProcessing();
  }

  protected sealed override void StopProcessing()
  {
    continueProcessing = false;

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
  ) => IO.Path.GetFullPath(
    Normalize(
      typedPath
    ),
    IO.Path.GetFullPath(
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
      FileSystemItemType.File => IO.File.Exists(
        absolutePath
      ),
      FileSystemItemType.Directory => IO.Directory.Exists(
        absolutePath
      ),
      _ => IO.Path.Exists(
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
  ) => IO.Path.GetFullPath(
    Normalize(
      subpath
    ),
    SessionState.Path.CurrentLocation.Path
  );

  private protected string Drive(
    string subpath = ""
  ) => IO.Path.GetFullPath(
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
