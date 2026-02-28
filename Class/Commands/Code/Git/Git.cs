namespace Module.Commands.Code.Git;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = "https://git-scm.com/docs"
)]
[Alias("g")]
public sealed class InvokeGit : CoreCommand
{
  [Parameter(
    Position = default,
    ValueFromRemainingArguments = true,
    DontShow = true,
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
    HelpMessage = "If git returns a non-zero exit code, warn and continue instead of the default behavior of throwing a terminating error."
  )]
  public SwitchParameter NoThrow
  {
    get => noThrow;
    set => noThrow = value;
  }
  private bool noThrow;

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as git argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    get => version;
    set => version = value;
  }
  private bool version;

  [Parameter(
    HelpMessage = "Pass -d flag as git argument"
  )]
  public SwitchParameter D
  {
    get => d;
    set => d = value;
  }
  private bool d;

  [Parameter(
    HelpMessage = "Pass -E flag as git argument"
  )]
  public SwitchParameter E
  {
    get => e;
    set => e = value;
  }
  private bool e;

  [Parameter(
    HelpMessage = "Pass -i flag as git argument"
  )]
  public SwitchParameter I
  {
    get => i;
    set => i = value;
  }
  private bool i;

  [Parameter(
    HelpMessage = "Pass -o flag as git argument"
  )]
  public SwitchParameter O
  {
    get => o;
    set => o = value;
  }
  private bool o;

  [Parameter(
    HelpMessage = "Pass -P flag as git argument"
  )]
  public SwitchParameter P
  {
    get => p;
    set => p = value;
  }
  private bool p;

  private static string Git => Client.Environment.Known.Application.Git;

  private protected sealed override void Postprocess()
  {
    List<string> gitArguments = [];
    bool newable = default;

    if (Verb is not "")
    {
      if (GitVerb.Verbs.Contains(Verb.ToLower()))
      {
        if (System.Enum.TryParse<GitVerb.NewableVerb>(Verb, true, out var newableVerb))
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
        gitArguments.Insert(0, WorkingDirectory);

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
      Git,
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

    AddScript(string.Join(' ', gitCommand));

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
