namespace PowerModule.Commands.Code.Node;

abstract public partial class NodeCommand(string? IntrinsicVerb) : CodeNativeCommand(IntrinsicVerb)
{
  private protected const string NpmHelpLink = "https://docs.npmjs.com/cli/commands";

  sealed override private protected string CommandPath
  { get; } = Client.Environment.Application.Npm;

  override private protected SwitchBoard Uppercase
  { get; } = new(
    D: true,
    E: true,
    P: true
  );

  sealed override private protected IEnumerable<string> CommandBaseArguments
  { get; } = ["--color=always"];

  sealed override private protected IEnumerable<string> WorkingDirectoryArguments => [$"--prefix={Pwd(WorkingDirectory)}"];

  sealed override private protected void PreprocessIntrinsicVerb()
  {
    switch (IntrinsicVerb)
    {
      case { } verb when Aliases.TryGetValue(
        verb.ToLower(
          Client.StringInput.CurrentCulture
        ),
        out var alias
      ):
        IntrinsicVerb = alias;

        break;

      case { } verb when Verbs.TryGetValue(
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
      case { } path when !IsNodePackage(path):
        (
          DeferredVerbArgument,
          WorkingDirectory
        ) = (
          WorkingDirectory,
          string.Empty
        );

        break;

      case { } path when Pwd(path) == Pwd():
        WorkingDirectory = string.Empty;

        break;

      default:
        break;
    }
  }

  private protected bool IsNodePackage(string path) => System.IO.File.Exists(
    System.IO.Path.Combine(
      Pwd(path),
      "package.json"
    )
  );
}
