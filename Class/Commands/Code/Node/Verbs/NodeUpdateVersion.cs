namespace Module.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Step,
  "NodePackageVersion",
  HelpUri = "https://docs.npmjs.com/cli/commands/npm-version"
)]
[Alias("nu")]
public sealed class NodeUpdateVersion() : NodeCommand("version")
{
  private enum NodeVersion
  {
    [System.ComponentModel.Description("Increment patch version (default)")]
    patch,

    [System.ComponentModel.Description("Increment minor version")]
    minor,

    [System.ComponentModel.Description("Increment major version")]
    major,

    [System.ComponentModel.Description("If current version is a release, increment patch version and add a prerelease tag. If current version is a prerelease, increment prerelease version.")]
    prerelease,

    [System.ComponentModel.Description("Increment patch version and add a prerelease tag")]
    prepatch,

    [System.ComponentModel.Description("Increment minor version and add a prerelease tag")]
    preminor,

    [System.ComponentModel.Description("Increment major version and add a prerelease tag")]
    premajor,
  }

  new public SwitchParameter V { get; set; }

  [Parameter(
    Position = default,
    HelpMessage = "New package version, default 'patch'"
  )]
  [Alias("v")]
  [Tab.EnumCompletions(
    typeof(NodeVersion),
    Include: ["from-git"],
    Exclude: ["prepatch"]
  )]
  public string Version
  {
    get => version;
    set => version = value switch
    {
      "" => nameof(NodeVersion.patch),
      var v when v.Equals("from-git", System.StringComparison.OrdinalIgnoreCase) => "from-git",
      var v when System.Enum.TryParse<NodeVersion>(
        v,
        true,
        out var named
      ) => named.ToString(),
      var v when SemanticVersion.TryParse(
        v is ['v' or 'V', .. var num]
          ? num
          : v,
        out var semver
      ) => semver.ToString(),
      _ => string.Empty,
    };
  }
  private string version = nameof(NodeVersion.patch);

  private protected sealed override void PreprocessArguments()
  {
    System.ArgumentException.ThrowIfNullOrEmpty(
      Version,
      $"{nameof(Version)} -> SemanticVersion | NodeVersion"
    );
  }

  private protected sealed override List<string> ParseArguments() => [Version];
}
