namespace Module
{
  using System.IO;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics.CodeAnalysis;
  using System.Management.Automation;
  using System;

  public abstract class CoreCommand : PSCmdlet
  {
    protected Dictionary<string, object> BoundParameters => MyInvocation.BoundParameters;

    protected bool IsPresent(
      string parameterName
    ) => BoundParameters.ContainsKey(parameterName);

    protected PowerShell AddCommand(
      string command,
      CommandTypes commandType = CommandTypes.Cmdlet
    ) => AddCommand(
      Context.PS(),
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

    protected Collection<PSObject> Call(
      string nativeCommand,
      string verb,
      string[] arguments,
      CommandTypes commandType = CommandTypes.Application
    ) => Call(
      nativeCommand,
      [verb, .. arguments],
      commandType
    );

    protected Collection<PSObject> Call(
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
        FileSystemItemType.File => File.Exists(psPath),
        FileSystemItemType.Directory => Directory.Exists(psPath),
        _ => Directory.Exists(psPath)
          || File.Exists(psPath)
      };
    }

    protected PSObject Var(
      string variable
    ) => Var<PSObject>(variable);

    protected T Var<T>(
      string variable
    ) where T : object => SessionState
      .PSVariable
      .GetValue(variable);

    protected string Pwd(
      string subpath = ""
    ) => Path.GetFullPath(
      subpath,
      SessionState.Path.CurrentLocation.Path
    );

    protected string Drive(
      string subpath = ""
    ) => Path.GetFullPath(
      subpath,
      SessionState.Drive.Current.Root
    );
  }
}
