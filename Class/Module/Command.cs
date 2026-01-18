namespace Module
{
  using System.IO;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Management.Automation;

  public abstract class CoreCommand : PSCmdlet
  {
    protected Dictionary<string, object> BoundParameters() => MyInvocation.BoundParameters;

    protected bool IsPresent(
      string parameterName
    ) => BoundParameters().ContainsKey(parameterName);

    protected static PowerShell PS() => PowerShell.Create(
      RunspaceMode.CurrentRunspace
    );

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

    protected Collection<PSObject> Call(
      string nativeCommand,
      string verb,
      string[] arguments = null,
      CommandTypes commandType = CommandTypes.Application
    ) => Call(
      nativeCommand,
      arguments == null
        ? [verb]
        : [verb, .. arguments],
      commandType
    );

    protected Collection<PSObject> Call(
      string nativeCommand,
      string[] arguments = null,
      CommandTypes commandType = CommandTypes.Application
    )
    {
      using PowerShell ps = AddCommand(
        nativeCommand,
        commandType
      );

      if (arguments != null)
      {
        foreach (string argument in arguments)
        {
          ps.AddArgument(argument);
        }
      }

      return ps.Invoke();
    }

    protected object Var(
      string variable
    ) => SessionState
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
