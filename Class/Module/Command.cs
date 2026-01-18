namespace Module
{
  using System.IO;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Management.Automation;

  public enum FileSystemItemType
  {
    Any,
    File,
    Directory
  }

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
