namespace Module.Commands.Code.Git;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = "https://git-scm.com/docs"
)]
[Alias("g")]
public sealed class GitInvoke : NativeCommand
{
  private enum NewableVerb
  {
    clone,
    config,
    init
  }

  private static readonly HashSet<string> Verbs = [
    "switch",
    "merge",
    "diff",
    "stash",
    "tag",
    "config",
    "remote",
    "submodule",
    "fetch",
    "checkout",
    "branch",
    "rm",
    "mv",
    "ls-files",
    "ls-tree",
    "init",
    "status",
    "clone",
    "pull",
    "add",
    "commit",
    "push",
    "reset",
  ];

  [Parameter(
    Position = default,
    HelpMessage = "Git command"
  )]
  [GitVerbCompletions]
  public string Verb { get; set; } = string.Empty;

  [Parameter(
    Position = 1,
    HelpMessage = "Repository path. For all verbs except 'clone', 'config', and 'init', the command will throw an error if there is no Git repository at the path."
  )]
  [WorkingDirectoryCompletions]
  public string WorkingDirectory { get; set; } = string.Empty;

  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true,
    HelpMessage = "Additional git arguments"
  )]
  public string[] ArgumentList { get; set; } = [];

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => version;
    set => version = value;
  }
  private bool version;

  private protected sealed override void Postprocess()
  {
    List<string> gitArguments = [];
    bool newable = default;

    if (Verb is not "")
    {
      if (Verbs.Contains(Verb.ToLower()))
      {
        if (System.Enum.TryParse<NewableVerb>(Verb, true, out var newableVerb))
        {
          newable = true;

          Verb = newableVerb.ToString();

          if (GitArgument.Regex().IsMatch(WorkingDirectory))
          {
            gitArguments.Add(WorkingDirectory);

            WorkingDirectory = string.Empty;
          }
        }
        else
        {
          if (
            WorkingDirectory is not ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              Pwd()
            ) is ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              WorkingDirectory
            ) is ""
            && GitWorkingDirectory.Resolve(
              Pwd(),
              Verb
            ) is not ""
          )
          {
            gitArguments.Add(WorkingDirectory);

            WorkingDirectory = Verb;
            Verb = "status";
          }
        }

        Verb = Verb.ToLower();
      }
    }
    else
    {
      if (version && ArgumentList is [])
      {
        newable = true;
      }
      else
      {
        Verb = "status";
      }
    }

    var repository = GitWorkingDirectory.Resolve(
      Pwd(),
      WorkingDirectory,
      newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        gitArguments.Insert(default, WorkingDirectory);

        repository = GitWorkingDirectory.Resolve(
          Pwd(),
          Pwd(),
          newable
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
      repository
    ];

    if (Verb is not "")
    {
      gitCommand.Add(Verb);
    }

    if (d)
    {
      gitArguments.Add("-d");
    }
    if (e)
    {
      gitArguments.Add("-E");
    }
    if (i)
    {
      gitArguments.Add("-i");
    }
    if (o)
    {
      gitArguments.Add("-o");
    }
    if (p)
    {
      gitArguments.Add("-P");
    }
    if (version)
    {
      gitArguments.Add("-v");
    }

    if (ArgumentList is not [])
    {
      gitArguments.AddRange(ArgumentList);
    }

    if (gitArguments is not [])
    {
      gitCommand.AddRange(gitArguments);
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
