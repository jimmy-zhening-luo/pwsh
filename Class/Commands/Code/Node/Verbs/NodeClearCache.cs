namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Clear,
  "NodeModuleCache",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-cache"
)]
[Alias("ncc")]
public sealed class NodeClearCache() : NpmCommand("cache")
{
  private protected sealed override List<string> ParseArguments()
  {
    ArgumentList = [];
    WorkingDirectory = string.Empty;
    NoThrow = false;
    D = false;
    E = false;
    I = false;
    O = false;
    P = false;
    V = false;

    return [
      "clean",
      "--force"
    ];
  }
}
