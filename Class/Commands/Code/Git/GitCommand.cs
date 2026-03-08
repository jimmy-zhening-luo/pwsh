namespace Module.Commands.Code.Git;

abstract public class GitCommand(string? IntrinsicVerb) : RemoteNativeVerbCommand(IntrinsicVerb)
{
  sealed private protected class GitVerbCompletionsAttribute() : Tab.CompletionsAttribute([.. Verbs]);

  private enum NewableVerb
  {
    clone,
    config,
    init,
  }

  static private readonly HashSet<string> Verbs = [
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

  private bool newable;

  sealed override private protected string CommandPath { get; } = Client.Environment.Known.Application.Git;

  override private protected SwitchBoard Uppercase { get; set; } = new(
    E: true,
    P: true
  );

  sealed override private protected string[] NativeCommandBaseArguments { get; } = [
    "-c",
    "color.ui=always",
  ];

  sealed override private protected string[] WorkingDirectoryArguments => WorkingDirectory is ""
    ? []
    : [
        "-C",
        WorkingDirectory,
      ];

  sealed override private protected void PreprocessIntrinsicVerb()
  {
    switch (IntrinsicVerb)
    {
      case null when V:
        newable = true;
        break;

      case null:
        IntrinsicVerb = "status";
        break;

      case var verb when System.Enum.TryParse<NewableVerb>(
        verb,
        true,
        out var newableVerb
      ):
        (
          newable,
          IntrinsicVerb
        ) = (
          true,
          newableVerb.ToString()
        );
        break;

      case var verb when Verbs.TryGetValue(
        verb.ToLower(),
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;
        break;
    }
  }

  sealed override private protected void PreprocessWorkingDirectory()
  {
    var pwd = Pwd();
    var repository = ResolveWorkingDirectory(
      WorkingDirectory,
      newable
    );

    if (repository is "")
    {
      if (WorkingDirectory is not "")
      {
        DeferredVerbArguments.Insert(default, WorkingDirectory);

        WorkingDirectory = string.Empty;
      }

      repository = ResolveWorkingDirectory(pwd, newable);
    }

    WorkingDirectory = repository;

    System.ArgumentException.ThrowIfNullOrEmpty(
      WorkingDirectory,
      string.Join(
        Client.Console.String.Space,
        [
          nameof(WorkingDirectory),
          newable
            ? "is not a directory path"
            : "is not a git repository",
        ]
      )
    );

    if (WorkingDirectory == pwd)
    {
      WorkingDirectory = string.Empty;
    }
  }

  private protected string ResolveWorkingDirectory(
    string path,
    bool newable = default
  ) => System.IO.Directory.Exists(
    System.IO.Path.Combine(
      Pwd(path),
      newable
        ? string.Empty
        : ".git"
    )
  )
    ? Pwd(path)
    : System.IO.Directory.Exists(
      System.IO.Path.Combine(
        Client.Environment.Known.Folder.Code(path),
        newable
          ? string.Empty
          : ".git"
      )
    )
      ? Client.Environment.Known.Folder.Code(path)
      : string.Empty;
}
