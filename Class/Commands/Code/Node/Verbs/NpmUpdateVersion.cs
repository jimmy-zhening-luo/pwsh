namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Step,
  "NodePackageVersion",
  HelpUri = $"{NpmHelpLink}/npm-version"
)]
[Alias("nu")]
sealed public class NpmUpdateVersion() : Npm("version")
{
  enum NodeVersion
  {
    [System.ComponentModel.Description(
      "Increment patch version (default)"
    )]
    patch,

    [System.ComponentModel.Description(
      "Increment minor version"
    )]
    minor,

    [System.ComponentModel.Description(
      "Increment major version"
    )]
    major,

    [System.ComponentModel.Description(
      "If current version is a release, increment patch version and add a prerelease tag. If current version is a prerelease, increment prerelease version."
    )]
    prerelease,

    [System.ComponentModel.Description(
      "Increment patch version and add a prerelease tag"
    )]
    prepatch,

    [System.ComponentModel.Description(
      "Increment minor version and add a prerelease tag"
    )]
    preminor,

    [System.ComponentModel.Description(
      "Increment major version and add a prerelease tag"
    )]
    premajor,
  }

  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = "New package version, default 'patch'"
  )]
  [Alias("v")]
  [ValidateNotNullOrWhiteSpace]
  [Tab.EnumCompletions(
    typeof(NodeVersion),
    Include = ["from-git"],
    Exclude = ["prepatch"]
  )]
  public string Version
  {
    private get => version;
    set => version = value switch
    {
      "" => nameof(NodeVersion.patch),
      _ when value.Equals("from-git", System.StringComparison.OrdinalIgnoreCase) => "from-git",
      _ when System.Enum.TryParse<NodeVersion>(
        value,
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
  string version = nameof(NodeVersion.patch);

  sealed override private protected void PreprocessOtherArguments() => System.ArgumentException.ThrowIfNullOrEmpty(
    Version,
    $"{nameof(Version)} is neither SemanticVersion nor NodeVersion"
  );

  sealed override private protected string[] ParseArguments() => [Version];
}
