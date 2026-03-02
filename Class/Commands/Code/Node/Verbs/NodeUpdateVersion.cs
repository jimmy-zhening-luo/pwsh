namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Step,
  "NodePackageVersion",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-version"
)]
[Alias("nu")]
public sealed class NodeUpdateVersion() : NodeCommand("version")
{
  private sealed class NodeVersionCompletionsAttribute() : Tab.Completer.EnumCompletionsAttribute(
    typeof(NodeVersion),
    default,
    ["from-git"],
    ["prepatch"]
  );

  private enum NodeVersion
  {
    patch,
    minor,
    major,
    prerelease,
    prepatch = prerelease,
    preminor,
    premajor,
  }

  new public SwitchParameter V { get; set; }

  [Parameter(
    Position = default,
    HelpMessage = "New package version, default 'patch'"
  )]
  [Alias("v")]
  [NodeVersionCompletions]
  public string Version
  {
    get => version;
    set => version = value.Trim() switch
    {
      "" => nameof(NodeVersion.patch),
      var v when v.Equals("from-git", System.StringComparison.OrdinalIgnoreCase) => "from-git",
      var v when System.Enum.TryParse<NodeVersion>(
        v,
        true,
        out var named
      ) => named.ToString(),
      var v when SemanticVersion.TryParse(
        v is ['v' or 'V', .. var num] ? num : v,
        out var semver
      ) => semver.ToString(),
      _ => string.Empty,
    };
  }
  private string version = nameof(NodeVersion.patch);

  private protected sealed override void PreprocessArguments()
  {
    if (Version is "")
    {
      Throw(
        "Provided version was neither a well-known version nor parseable as a semantic version."
      );
    }
  }

  private protected sealed override List<string> ParseArguments() => [Version];
}
