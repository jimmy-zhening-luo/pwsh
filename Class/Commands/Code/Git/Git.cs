namespace PowerModule.Commands.Code.Git;

abstract public partial class GitCommand(string? IntrinsicVerb) : CodeNativeCommand(
  Client.Environment.Application.Git,
  [
    "-c",
    "color.ui=always",
  ],
  IntrinsicVerb
)
{
  private protected const string GitHelpLink = "https://git-scm.com/docs";

  bool newable;

  override private protected SwitchBoard Uppercase
  { get; } = new(
    E: true,
    P: true
  );

  sealed override private protected IEnumerable<string> WorkingDirectoryArguments => [
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
        verb.ToLower(
          Client.StringInput.CurrentCulture
        ),
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;

        break;

      default:
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
        (DeferredVerbArgument, WorkingDirectory) = (
          WorkingDirectory,
          string.Empty
        );
      }

      repository = ResolveWorkingDirectory(pwd, newable);
    }

    WorkingDirectory = repository;

    System.ArgumentException.ThrowIfNullOrEmpty(
      WorkingDirectory,
      string.Join(
        Client.StringInput.Space,
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
        Client.Environment.Folder.Code(path),
        newable
          ? string.Empty
          : ".git"
      )
    )
      ? Client.Environment.Folder.Code(path)
      : string.Empty;
}
