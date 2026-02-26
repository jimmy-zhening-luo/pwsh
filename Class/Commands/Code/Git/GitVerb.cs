namespace Module.Commands.Code.Git;

public static class GitVerb
{
  public enum NewableVerb
  {
    clone,
    config,
    init
  }

  public static readonly HashSet<string> Verbs = [
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
