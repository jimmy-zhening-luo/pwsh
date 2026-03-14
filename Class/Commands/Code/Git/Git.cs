namespace PowerModule.Commands.Code.Git;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Microsoft.Naming",
  "CA1724: Type names should not match namespaces"
)]
abstract public partial class Git(string? IntrinsicVerb) : NativeCodeCommand(
  Client.Environment.Application.Git,
  IntrinsicVerb,
  [
    "-c",
    "color.ui=always",
  ],
  "-C"
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
          Client.StringInput.InvariantCulture
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
      case "":
      case var path when System.IO.Path.Exists(
        System.IO.Path.Combine(
          Pwd(path),
          newable
            ? string.Empty
            : ".git"
        )
      ):
        break;

      case var path when System.IO.Path.Exists(
        System.IO.Path.Combine(
          Client.Environment.Folder.Code(path),
          newable
            ? string.Empty
            : ".git"
        )
      ):
        WorkingDirectoryLocation = Client.Environment.Folder.Code;

        break;

      default:
        (
          DeferredVerbArgument,
          WorkingDirectory
        ) = (
          WorkingDirectory,
          string.Empty
        );

        break;
    }
  }
}
