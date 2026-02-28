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
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional npm arguments"
  )]
  [WorkingDirectoryCompletions]
  public string[] ArgumentList { get; set; } = [];

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

  private protected sealed override void BuildNativeCommand()
  {
    List<string> nodeCommand = [
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
          nodeCommand.Add(packagePrefix);
        }
      }
      else
      {
        arguments.Insert(default, WorkingDirectory);

        WorkingDirectory = string.Empty;
      }
    }

    nodeCommand.Add(Verb.ToLower());

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

    if (ArgumentList is not [])
    {
      arguments.AddRange(ArgumentList);
    }

    if (arguments is not [])
    {
      nodeCommand.AddRange(arguments);
    }

    List<string> escapedNodeCommand = [];

    foreach (var word in nodeCommand)
    {
      escapedNodeCommand.Add(
        Client.Console.String.EscapeDoubleQuoted(word)
      );
    }

    AddScript(string.Join(' ', escapedNodeCommand));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      if (noThrow)
      {
        WriteWarning("npm error");
      }
      else
      {
        Throw("npm error");
      }
    }
  }
}
