namespace PowerModule.Commands.Code.Node.Cli;

[Cmdlet(
  VerbsLifecycle.Invoke,
  "Npm",
  HelpUri = NpmHelpLink
)]
[Alias("n")]
sealed public class NpmCli() : Npm(default)
{
  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = "npm command"
  )]
  [ArgumentCompletions(
    "access",
    "adduser",
    "audit",
    "bugs",
    "cache",
    "ci",
    "completion",
    "config",
    "dedupe",
    "deprecate",
    "diff",
    "dist-tag",
    "docs",
    "doctor",
    "edit",
    "exec",
    "explain",
    "explore",
    "find-dupes",
    "fund",
    "help",
    "help-search",
    "init",
    "install",
    "install-ci-test",
    "install-test",
    "link",
    "login",
    "logout",
    "ls",
    "org",
    "outdated",
    "owner",
    "pack",
    "ping",
    "pkg",
    "prefix",
    "profile",
    "prune",
    "publish",
    "query",
    "rebuild",
    "repo",
    "restart",
    "root",
    "run",
    "sbom",
    "search",
    "shrinkwrap",
    "star",
    "stars",
    "start",
    "stop",
    "team",
    "test",
    "token",
    "undeprecate",
    "uninstall",
    "unpublish",
    "unstar",
    "update",
    "version",
    "view",
    "whoami"
  )]
  public string Verb
  {
    init => IntrinsicVerb = value.ToLower(
      Client.StringInput.InvariantCulture
    );
  }

  [Parameter(
    HelpMessage = "Show npm version if no command is specified. Otherwise, pass the -v flag as argument."
  )]
  [Alias("v")]
  public SwitchParameter Version
  {
    init => base.V = value;
  }
}
