namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = $"{NpmHelpLink}/npm-cache"
)]
[Alias("ncc")]
sealed public class NodeClearCache() : NodeCommand("cache")
{
  sealed override private protected void PreprocessOtherArguments()
  {
    (
      WorkingDirectory,
      DeferredVerbArgument,
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
      default,
      default
    );

    Arguments.Clear();
    NativeArguments.Clear();
    _ = NativeArguments.AddLast("--force");
  }

  sealed override private protected IEnumerable<string> ParseArguments() => ["clean"];
}
