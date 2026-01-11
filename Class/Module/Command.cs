namespace Module
{
  using System;
  using System.Collections.ObjectModel;
  using System.Management.Automation;
  using System.Management.Automation.Runspaces;

  public abstract class PSCoreCommand : PSCmdlet
  {
    protected Object Var(string variable) => SessionState
      .PSVariable
      .GetValue(variable);

    protected Collection<PSObject> Call(
      string nativeCommand,
      string verb,
      string[] arguments = null
    ) => Call(
      nativeCommand + " " + verb,
      arguments
    );

    protected Collection<PSObject> Call(
      string nativeCommand,
      string[] arguments = null
    ) => SessionState
      .InvokeCommand
      .InvokeScript(
        arguments != null && arguments.Length != 0
          ? nativeCommand
            + " "
            + string.Join(' ', arguments)
          : nativeCommand
      );

    protected Collection<PSObject> InvokeNative(
      string nativeCommand,
      string verb,
      string[] arguments = null,
      CommandTypes commandType = CommandTypes.Application
    ) => InvokeNative(
      nativeCommand,
      arguments == null
        ? [verb]
        : [verb, ..arguments],
      commandType
    );

    protected Collection<PSObject> InvokeNative(
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
