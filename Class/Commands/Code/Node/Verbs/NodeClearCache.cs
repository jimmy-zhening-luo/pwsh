namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-cache"
)]
[Alias("ncc")]
sealed public class NodeClearCache() : NodeCommand("cache")
{
  sealed override private protected void PreprocessOtherArguments()
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

  sealed override private protected IEnumerable<string> ParseArguments() => ["clean"];
}
