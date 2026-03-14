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

  sealed override private protected string WorkingDirectoryArtifactSubpath => newable
    ? string.Empty
    : ".git";

  override private protected SwitchBoard Uppercase
  { get; } = new(
    E: true,
    P: true
  );

  sealed override private protected void CanonicalizeVerb()
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
        default,
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

      default:
        break;
    }
  }
}
