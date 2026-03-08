namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-cache"
)]
[Alias("ncc")]
public sealed class NodeClearCache() : NodeCommand("cache")
{
  private protected sealed override void PreprocessOtherArguments()
  {
    (
      WorkingDirectory,
      NoThrow,
      D,
      E,
      I,
      O,
      P,
      V
    ) = (
      string.Empty,
      default,
      default,
      default,
      default,
      default,
      default,
      default
    );

    Arguments.Clear();
    NativeArguments.Clear();
    NativeArguments.Add("--force");
  }

  private protected sealed override List<string> ParseArguments() => ["clean"];
}
