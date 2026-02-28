namespace Module.Commands.Code.Git;

public abstract class GitCommand(
  string Verb,
  bool Newable = default
) : CoreCommand
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
    HelpMessage = "If git returns a non-zero exit code, warn and continue instead of the default behavior of throwing a terminating error."
  )]
  public SwitchParameter NoThrow
  {
    get => noThrow;
    set => noThrow = value;
  }
  private bool noThrow;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -v flag as git argument"
  )]
  public SwitchParameter V
  {
    get => v;
    set => v = value;
  }
  private bool v;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -d flag as git argument"
  )]
  public SwitchParameter D
  {
    get => d;
    set => d = value;
  }
  private bool d;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -E flag as git argument"
  )]
  public SwitchParameter E
  {
    get => e;
    set => e = value;
  }
  private bool e;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -i flag as git argument"
  )]
  public SwitchParameter I
  {
    get => i;
    set => i = value;
  }
  private bool i;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -o flag as git argument"
  )]
  public SwitchParameter O
  {
    get => o;
    set => o = value;
  }
  private bool o;

  [Parameter(
    DontShow = true,
    HelpMessage = "Pass -P flag as git argument"
  )]
  public SwitchParameter P
  {
    get => p;
    set => p = value;
  }
  private bool p;

  private protected abstract List<string> ParseGitArguments();

  private protected sealed override void Postprocess()
  {
    List<string> arguments = [];

    arguments.AddRange(ParseGitArguments());

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
