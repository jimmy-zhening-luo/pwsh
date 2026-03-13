namespace PowerModule.Commands.Code.Git;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Microsoft.Naming",
  "CA1724: Type names should not match namespaces"
)]
abstract public partial class Git(string? IntrinsicVerb) : NativeCodeCommand(
  Client.Environment.Application.Git,
  [
    "-c",
    "color.ui=always",
  ],
  "-C",
  default,
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
    switch (WorkingDirectory)
    {
      case "" when !newable
      && !IsWorkingDirectoryLocal(Pwd()):
        throw new System.IO.DirectoryNotFoundException(
          "The current directory is not a git repository."
        );

      case "":
      case var path when IsWorkingDirectoryLocal(
        path,
        newable
      ):
        break;

      case var path when IsWorkingDirectoryRemote(
        path,
        newable
      ):
        WorkingDirectoryLocation = Client.Environment.Folder.Code;

        break;

      case var path when newable
      || IsWorkingDirectoryLocal(Pwd()):
        (
          DeferredVerbArgument,
          WorkingDirectory
        ) = (
          path,
          string.Empty
        );

        break;

      default:
        throw new System.IO.DirectoryNotFoundException(
          "The provided working directory is not a git repository, and the current directory is not a git repository."
        );
    }
  }

  private bool IsWorkingDirectoryLocal(
    string path,
    bool newable = default
  ) => System.IO.Directory.Exists(
    GetFullWorkingDirectoryTestPath(
      path,
      newable
    )
  );

  private bool IsWorkingDirectoryRemote(
    string path,
    bool newable = default
  ) => System.IO.Directory.Exists(
    GetFullWorkingDirectoryTestPath(
      path,
      newable,
      true
    )
  );

  private string GetFullWorkingDirectoryTestPath(
    string path,
    bool newable,
    bool remote = default
  ) => System.IO.Path.Combine(
    remote
      ? Client.Environment.Folder.Code(path)
      : Pwd(path),
    newable
      ? string.Empty
      : ".git"
  );
}
