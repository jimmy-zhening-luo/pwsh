namespace PowerModule.Commands;

abstract public partial class CoreCommand(bool SkipSsh = default) : PSCmdlet, System.IDisposable
{
  bool disposed;

  ~CoreCommand()
  {
    Dispose(false);
  }

  private protected delegate string Localizer(string path);
  virtual private protected Localizer? Location
  { get; }

  private protected bool InCurrentLocation => Location is null;

  bool BlockedBySsh => SkipSsh
  && Client.Environment.Variable.InSsh;

  PowerShellHost PSHost
  {
    get
    {
      System.ObjectDisposedException.ThrowIf(disposed, this);

      return pshost ??= new();
    }
  }
  PowerShellHost? pshost;

  public void Dispose()
  {
    Dispose(true);

    System.GC.SuppressFinalize(this);
  }

  virtual protected void Dispose(bool disposing)
  {
    if (!disposed)
    {
      if (disposing)
      {
        if (pshost is not null)
        {
          pshost.Dispose();
          pshost = default;
        }
      }

      disposed = true;
    }
  }

  sealed override protected void BeginProcessing()
  {
    System.ObjectDisposedException.ThrowIf(disposed, this);

    if (!BlockedBySsh)
    {
      Preprocess();
    }
  }

  sealed override protected void ProcessRecord()
  {
    System.ObjectDisposedException.ThrowIf(disposed, this);

    if (!BlockedBySsh)
    {
      Process();
    }
  }

  sealed override protected void EndProcessing()
  {
    System.ObjectDisposedException.ThrowIf(disposed, this);

    if (!BlockedBySsh)
    {
      Postprocess();
    }
  }

  sealed override protected void StopProcessing() => Dispose();

  virtual private protected void Preprocess()
  { }

  virtual private protected void Process()
  { }

  virtual private protected void Postprocess()
  { }

  private protected void WriteInformation(object log) => base.WriteInformation(
    new InformationRecord(
      log,
      string.Empty
    )
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

  private protected void CheckNativeError(bool stop = default) => CheckNativeError(
    "Native command error",
    stop
  );
  private protected void CheckNativeError(
    string message,
    bool stop = default
  )
  {
    if (
      SessionState.PSVariable.GetValue(
        "LASTEXITCODE"
      )
      is not (null or 0)
    )
    {
      if (stop)
      {
        throw new System.InvalidOperationException(message);
      }
      else
      {
        WriteWarning(message);
      }
    }
  }

  private protected void SetBoundParameter(
    string parameter,
    object? value
  )
  {
    switch (value)
    {
      case null:
      case false:
        RemoveBoundParameter(parameter);

        break;

      case true:
        SwitchBoundParameter(parameter);

        break;

      default:
        MyInvocation.BoundParameters[parameter] = value;

        break;
    }
  }

  private protected void SwitchBoundParameter(string parameter) => MyInvocation.BoundParameters[parameter] = SwitchParameter.Present;

  private protected void RemoveBoundParameter(string parameter) => MyInvocation.BoundParameters.Remove(parameter);

  private protected PowerShell AddStatement() => PSHost.AddStatement();

  private protected PowerShell AddCommand(
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => PSHost.AddCommand(
    SessionState.InvokeCommand.GetCommand(
      command,
      commandType
    )
  );

  private protected PowerShell AddParameter(string parameterName) => PSHost.AddParameter(parameterName);
  private protected PowerShell AddParameter(
    string parameterName,
    object value
  ) => PSHost.AddParameter(
    parameterName,
    value
  );

  private protected PowerShell AddBoundParameters() => PSHost.AddParameters(MyInvocation.BoundParameters);

  private protected PowerShell AddScript(string script) => PSHost.AddScript(script);

  private protected Collection<PSObject> InvokePowerShell() => PSHost.InvokePowerShell();
  private protected Collection<T> InvokePowerShell<T>() => PSHost.InvokePowerShell<T>();

  private protected void ClearCommands() => PSHost.ClearCommands();

  private protected void BeginSteppablePipeline() => PSHost.BeginSteppablePipeline(this);

  private protected void ProcessSteppablePipeline() => PSHost.ProcessSteppablePipeline(this);
  private protected void ProcessSteppablePipeline(object input) => PSHost.ProcessSteppablePipeline(
    this,
    input
  );

  private protected void EndSteppablePipeline() => PSHost.EndSteppablePipeline();

  private protected string[] ReanchorPath(string[] paths)
  {
    if (paths is [])
    {
      return [(Location ?? Pwd)(string.Empty)];
    }

    List<string> reanchoredPaths = new(
      paths.Length
    );

    foreach (var path in paths)
    {
      reanchoredPaths.Add(
        ReanchorPath(path)
      );
    }

    return reanchoredPaths.ToArray();
  }
  private protected string ReanchorPath(string path) => (
    Location ?? Pwd
  )(
    path
  );

  private protected string Drive() => SessionState.Drive.Current.Root;
  private protected string Drive(string path) => Client.File.PathString.GetFullPathLocal(
    SessionState.Drive.Current.Root,
    path
  );

  private protected string Pwd() => SessionState.Path.CurrentLocation.Path;
  private protected string Pwd(string path) => Client.File.PathString.GetFullPathLocal(
    SessionState.Path.CurrentLocation.Path,
    path
  );

  private protected string Parent() => Pwd(Client.File.PathString.Parent);
  private protected string Parent(string path) => Client.File.PathString.GetFullPathLocal(
    Parent(),
    path
  );

  private protected string ParentParent() => Pwd(Client.File.PathString.ParentParent);
  private protected string ParentParent(string path) => Client.File.PathString.GetFullPathLocal(
    ParentParent(),
    path
  );
}
