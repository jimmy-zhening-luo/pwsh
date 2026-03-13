namespace PowerModule.Commands.Code.Node;

abstract public partial class Npm(string? IntrinsicVerb) : NativeCodeCommand(
  Client.Environment.Application.Npm,
  ["--color=always"],
  default,
  "--prefix=",
  IntrinsicVerb
)
{
  private protected const string NpmHelpLink = "https://docs.npmjs.com/cli/commands";

  override private protected SwitchBoard Uppercase
  { get; } = new(
    D: true,
    E: true,
    P: true
  );

  sealed override private protected void PreprocessIntrinsicVerb()
  {
    switch (
      IntrinsicVerb?.ToLower(
        Client.StringInput.InvariantCulture
      )
    )
    {
      case { } verb when Verbs.TryGetValue(
        verb,
        out var exactVerb
      ):
        IntrinsicVerb = exactVerb;

        break;

      case { } verb when Aliases.TryGetValue(
        verb,
        out var alias
      ):
        IntrinsicVerb = alias;

        break;

      default:
        break;
    }
  }

  sealed override private protected void PreprocessWorkingDirectory()
  {
    if (
      !System.IO.File.Exists(
        System.IO.Path.Combine(
          Pwd(WorkingDirectory),
          "package.json"
        )
      )
    )
    {
      (
        DeferredVerbArgument,
        WorkingDirectory
      ) = (
        WorkingDirectory,
        string.Empty
      );
    }
  }
}
