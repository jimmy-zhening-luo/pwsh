namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string Verb,
  bool Newable = default
) : NativeCommand
{
  [Parameter(
    Position = default,
    HelpMessage = "Repository path"
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    Position = 100,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional git arguments"
  )]
  public string[] ArgumentList { get; set; } = [];

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as argument"
  )]
  public SwitchParameter V
  {
    get => v;
    set => v = value;
  }
  private bool v;

  private protected abstract List<string> ParseArguments();

  private protected sealed override void Postprocess()
  {
    List<string> arguments = [];

    arguments.AddRange(ParseArguments());

    var repository = GitWorkingDirectory.Resolve(
      Pwd(),
      WorkingDirectory,
      Newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        arguments.Insert(default, WorkingDirectory);

        repository = GitWorkingDirectory.Resolve(
          Pwd(),
          Pwd(),
          Newable
        );
      }

      if (repository is "")
      {
        Throw(
          $"Path {WorkingDirectory} is not a git repository."
        );
      }
    }

    List<string> gitCommand = [
      "&",
      Client.Environment.Known.Application.Git,
      "-c",
      "color.ui=always",
      "-C",
      repository,
      Verb
    ];

    if (d)
    {
      arguments.Add("-d");
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
      gitCommand.AddRange(arguments);
    }

    List<string> escapedGitCommand = [];

    foreach (var word in gitCommand)
    {
      escapedGitCommand.Add(
        Client.Console.String.EscapeDoubleQuoted(word)
      );
    }

    AddScript(string.Join(' ', escapedGitCommand));

    ProcessSteppablePipeline();
    EndSteppablePipeline();

    if (HadNativeErrors)
    {
      if (noThrow)
      {
        WriteWarning("git error");
      }
      else
      {
        Throw("git error");
      }
    }
  }
}
