namespace PowerModule.Commands.Code.Git;

partial class Git
{
  sealed private protected class GitVerbCompletionsAttribute() : Tab.CompletionsAttribute<HashSet<string>>(Verbs);


  static readonly HashSet<string> NewableVerb = [
    "switch",
    "merge",
    "diff",
  ];

  static readonly HashSet<string> Verbs = [
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
}
