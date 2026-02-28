namespace Module.Commands;

public abstract class CoreCommand(
  bool SkipSsh = default
) : PSCmdlet, System.IDisposable
{
  private protected record Locator(
    string Root = "",
    string Subpath = ""
  )
  {
    public bool IsEmpty => this is
    {
      Root: "",
      Subpath: "",
    };

    public bool IsRooted => this is
    {
      Root: not "",
    };
  }

  private uint steps;

  private bool stopped;

  private bool disposed;

  ~CoreCommand()
  {
    Dispose(default);
  }

  private protected virtual Locator Location => new();

  private protected bool UsingDefaultLocation => Location.IsEmpty;

  private protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

  private protected bool HadErrors => powershell?.HadErrors ?? default;

  private PowerShell PS => powershell ??= PowerShellHost.Create();
  private PowerShell? powershell;

  private bool BlockedBySsh => SkipSsh
    && Client.Environment.Known.Variable.InSsh;

  private bool Alive => !disposed
    && !stopped;

  private bool ContinueProcessing => Alive
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

  private protected PowerShell AddParameter(string parameterName) => PS.AddParameter(parameterName);
  private protected PowerShell AddParameter(
    string parameterName,
    object value
  ) => PS.AddParameter(
    parameterName,
    value
  );

  private protected PowerShell AddParameters(IList parameters) => PS.AddParameters(parameters);
  private protected PowerShell AddParameters(IDictionary parameters) => PS.AddParameters(parameters);

  private protected PowerShell AddStatement() => PS.AddStatement();

  private protected PowerShell AddScript(string script) => PS.AddScript(script);

  private protected void Clear() => PS
    .Commands
    .Clear();

  private protected Collection<PSObject> InvokePowerShell() => PS.Invoke();
  private protected Collection<T> InvokePowerShell<T>() => PS.Invoke<T>();

  private protected void BeginSteppablePipeline()
  {
    if (Alive)
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

  private protected void ProcessSteppablePipeline(
    object? input = default
  )
  {
    if (Alive)
    {
      if (steppablePipeline is null)
      {
        BeginSteppablePipeline();
      }

      if (steppablePipeline is not null)
      {
        _ = input is null
          ? steppablePipeline.Process()
          : steppablePipeline.Process(input);
      }
    }
  }

  private protected void EndSteppablePipeline()
  {
    if (Alive && steppablePipeline is not null)
    {
      _ = steppablePipeline.End();
    }
  }

  private protected string Reanchor(string path = "") => Client.File.PathString.FullPathLocationRelative(
    Location.IsRooted
      ? Client.File.PathString.FullPathLocationRelative(
          Location.Root,
          Location.Subpath
        )
      : Pwd(Location.Subpath),
    path
  );

  private protected PSObject PSVariable(string variable) => PSVariable<PSObject>(variable);
  private protected T PSVariable<T>(string variable) => (T)SessionState
    .PSVariable
    .GetValue(variable);

  private protected string Pwd(string path = "") => Client.File.PathString.FullPathLocationRelative(
    SessionState.Path.CurrentLocation.Path,
    path
  );

  private protected string Drive(string path = "") => Client.File.PathString.FullPathLocationRelative(
    SessionState.Drive.Current.Root,
    path
  );

  private protected void WriteEvent(
    object log,
    string? source = default
  ) => WriteInformation(
    new(
      log,
      source ?? GetName()
    )
  );

  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    string message,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default,
    string? id = default
  ) => Throw(
    new System.Exception(message),
    category,
    target,
    id
  );
  [System.Diagnostics.CodeAnalysis.DoesNotReturn]
  private protected void Throw(
    System.Exception exception,
    ErrorCategory category = ErrorCategory.InvalidOperation,
    object? target = default,
    string? id = default
  )
  {
    Dispose();

    ThrowTerminatingError(
      new(
        exception,
        $"{GetName()}Exception" + (id ?? string.Empty),
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
    CleanPipeline();

    powershell?.Dispose();
    powershell = default;
  }

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
}

