namespace Module
{
  using System.Collections.ObjectModel;
  using System.Management.Automation;

  public abstract class PSCoreCommand : PSCmdlet
  {
    protected Object Var(string variable) => SessionState
      .PSVariable
      .GetValue(
        variable,
        string.Empty
      ) ?? string.Empty;

    protected Collection<PSObject> Call(
      string nativeCommand,
      string[] arguments,
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
