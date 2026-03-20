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

  [Parameter(Position = default)]
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

  [Alias(nameof(V))]
  public SwitchParameter Version
  {
    init => base.V = value;
  }
}
