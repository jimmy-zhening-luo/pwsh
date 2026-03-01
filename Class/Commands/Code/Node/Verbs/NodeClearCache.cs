namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-cache"
)]
[Alias("ncc")]
public sealed class NodeClearCache() : NodeCommand("cache")
{
  private protected sealed override void PreprocessArguments()
  {
    ArgumentList = [];
    WorkingDirectory = string.Empty;
    noThrow = false;
    d = false;
    e = false;
    i = false;
    o = false;
    p = false;
    V = false;
  }

  private protected sealed override List<string> ParseArguments() => ["clean", "--force"];
}
