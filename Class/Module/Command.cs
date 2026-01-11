namespace Module
{
  using System;
  using System.Collections.ObjectModel;
  using System.Management.Automation;
  using System.Management.Automation.Runspaces;

  public abstract class PSCoreCommand : PSCmdlet
  {
    protected Object Var(
      string variable
    ) => SessionState
      .PSVariable
      .GetValue(variable);

    protected string Pwd(
      string subpath = ""
    )
    {
      using var ps = PowerShell.Create(
        RunspaceMode.CurrentRunspace
      );
      ps.AddCommand("Get-Location");

      string pwd = ps
        .Invoke()[0]
        .BaseObject
        .ToString();

      return Pathing.Resolve(
        pwd,
        subpath
      );
    }

    protected Collection<PSObject> Call(
      string nativeCommand,
      string verb,
      string[] arguments = null,
      CommandTypes commandType = CommandTypes.Application
    ) => Call(
      nativeCommand,
      arguments == null
        ? [verb]
        : [verb, ..arguments],
      commandType
    );

    protected Collection<PSObject> Call(
      string nativeCommand,
      string[] arguments = null,
      CommandTypes commandType = CommandTypes.Application
    )
    {
      using var ps = PowerShell.Create(
        RunspaceMode.CurrentRunspace
      );
      ps.AddCommand(
        SessionState
          .InvokeCommand
          .GetCommand(
            nativeCommand,
            commandType
          )
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
  }
}
