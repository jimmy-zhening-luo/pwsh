namespace PowerModule.Commands.Code.Node;

abstract public class Npm(string? IntrinsicVerb) : NativeCodeCommand(
  "npm",
  IntrinsicVerb,
  ["--color=always"],
  WorkingDirectoryPrefix: "--prefix="
)
{
  private protected const string NpmHelpLink = "https://docs.npmjs.com/cli/commands";

  sealed override private protected string WorkingDirectoryArtifactSubpath
  { get; } = "package.json";

  override private protected SwitchBoard Uppercase
  { get; } = new(
    D: true,
    E: true,
    P: true
  );

  sealed override private protected void CanonicalizeVerb()
  { }
}
