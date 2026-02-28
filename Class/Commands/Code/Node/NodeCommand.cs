namespace Module.Commands.Code.Node;

public abstract class NpmCommand(string Verb) : NativeCommand
{
  [Parameter(
    Position = 50,
    HelpMessage = "Node package path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  private protected abstract List<string> ParseArguments();

  private protected sealed override CommandArguments BuildNativeCommand()
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
      d = false;
    }
    if (e)
    {
      arguments.Add("-E");
      e = false;
    }
    if (p)
    {
      arguments.Add("-P");
      p = false;
    }

    return new(command, arguments);
  }
}
