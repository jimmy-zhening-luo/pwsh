namespace Module.Commands.Code.Node;

public abstract class NpmCommand(string Verb) : NativeCommand
{
  [Parameter(
    Position = 50,
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V
  {
    get => v;
    set => v = value;
  }
  private bool v;

  private protected abstract List<string> ParseArguments();

  private protected sealed override List<string> BuildNativeCommand()
  {
    List<string> command = [
      "&",
      "npm.ps1",
      "--color=always",
    ];

    List<string> arguments = [];

    arguments.AddRange(ParseArguments());

    if (WorkingDirectory is not "")
    {
      if (NodeWorkingDirectory.Test(Pwd(), WorkingDirectory))
      {
        var packagePrefix = WorkingDirectory is ""
          ? ""
          : "--prefix="
            + Client.File.PathString.FullPathLocationRelative(
                Pwd(),
                WorkingDirectory
              );

        if (packagePrefix is not "")
        {
          command.Add(packagePrefix);
        }
      }
      else
      {
        arguments.Insert(default, WorkingDirectory);

        WorkingDirectory = string.Empty;
      }
    }

    command.Add(Verb.ToLower());

    if (d)
    {
      arguments.Add("-D");
    }
    if (e)
    {
      arguments.Add("-E");
    }
    if (i)
    {
      arguments.Add("-i");
    }
    if (o)
    {
      arguments.Add("-o");
    }
    if (p)
    {
      arguments.Add("-P");
    }
    if (v)
    {
      arguments.Add("-v");
    }

    if (arguments is not [])
    {
      command.AddRange(arguments);
    }

    return command;
  }
}
