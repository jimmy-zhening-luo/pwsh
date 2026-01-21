namespace Module.Command;

using static System.IO.Path;
using Exception = System.Exception;
using DoesNotReturn = System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute;
using BoundParameterDictionary = System.Collections.Generic.Dictionary<string, object>;
using PSObjectCollection = System.Collections.ObjectModel.Collection<PSObject>;

public abstract class CoreCommand : PSCmdlet
{
  protected BoundParameterDictionary BoundParameters => MyInvocation.BoundParameters;

  protected bool IsPresent(
    string parameterName
  ) => BoundParameters.ContainsKey(parameterName);

  protected PowerShell AddCommand(
    string command,
    CommandTypes commandType = CommandTypes.Cmdlet
  ) => AddCommand(
    PS(),
    command,
    commandType
  );

  protected PowerShell AddCommand(
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
  protected void Throw(
    string message,
    string id,
    ErrorCategory category,
    object? target = null
  ) => Throw(
    new Exception(
      message
    ),
    id,
    category,
    target
  );

  [DoesNotReturn]
  protected void Throw(
    Exception exception,
    string id,
    ErrorCategory category,
    object? target = null
  ) => ThrowTerminatingError(
    new ErrorRecord(
      exception,
      id,
      category,
      target
    )
  );

  protected PSObjectCollection Call(
    string nativeCommand,
    string verb,
    string[] arguments,
    CommandTypes commandType = CommandTypes.Application
  ) => Call(
    nativeCommand,
    [verb, .. arguments],
    commandType
  );

  protected PSObjectCollection Call(
    string nativeCommand,
    string[] arguments,
    CommandTypes commandType = CommandTypes.Application
  )
  {
    using PowerShell ps = AddCommand(
      nativeCommand,
      commandType
    );
    foreach (string argument in arguments)
    {
      ps.AddArgument(argument);
    }

    return ps.Invoke();
  }

  protected bool TestPath(
    string path,
    FileSystemItemType type = FileSystemItemType.Any
  )
  {
    string psPath = Pwd(path);

    return type switch
    {
      FileSystemItemType.File => System.IO.File.Exists(psPath),
      FileSystemItemType.Directory => System.IO.Directory.Exists(psPath),
      _ => Exists(psPath)
    };
  }

  protected PSObject Var(
    string variable
  ) => Var<PSObject>(variable);

  protected T Var<T>(
    string variable
  ) => (T)SessionState
    .PSVariable
    .GetValue(variable);

  protected string Pwd(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    SessionState.Path.CurrentLocation.Path
  );

  protected string Drive(
    string subpath = ""
  ) => GetFullPath(
    subpath,
    SessionState.Drive.Current.Root
  );
}
