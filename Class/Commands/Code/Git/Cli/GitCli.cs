namespace PowerModule.Commands.Code.Git.Cli;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Git",
  HelpUri = GitHelpLink
)]
[Alias("g")]
sealed public class GitCli() : Git(default)
{
  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = "Git command"
  )]
  [ValidateNotNullOrWhiteSpace]
  [ArgumentCompletions(
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
    "reset"
  )]
  public string Verb
  {
    init => IntrinsicVerb = value.ToLower(
      Client.StringInput.InvariantCulture
    );
  }

  [Parameter(
    HelpMessage = "Show git version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    init => base.V = value;
  }
}
