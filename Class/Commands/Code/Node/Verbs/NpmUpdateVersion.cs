namespace PowerModule.Commands.Code.Node.Verbs;

[Cmdlet(
  VerbsCommon.Step,
  "NodePackageVersion",
  HelpUri = $"{NpmHelpLink}/npm-version"
)]
[Alias("nu")]
sealed public class NpmUpdateVersion() : Npm("version")
{
  sealed class NpmVersionCompletionsAttribute() : Tab.Factory.DomainCompleterFactory(NodePackageVersion);

  const string DefaultNodePackageVersion = "patch";

  static readonly HashSet<string> NodePackageVersion = [
    DefaultNodePackageVersion,
    "minor",
    "major",
    "prerelease",
    "preminor",
    "premajor",
    "prepatch",
    "from-git",
  ];

  new internal SwitchParameter V
  { get; }

  [Parameter(
    Position = default,
    HelpMessage = $"New package version, default '{DefaultNodePackageVersion}'"
  )]
  [Alias(nameof(V))]
  [ValidateNotNullOrWhiteSpace]
  [NpmVersionCompletions]
  public string Version
  {
    private get;
    init => field = value.ToLower(
      Client.StringInput.InvariantCulture
    )
    switch
    {
      var version
      when NodePackageVersion.Contains(
        version
      ) => version,
      var version
      when SemanticVersion.TryParse(
        version is
        [
          'v',
          .. var numericPart,
        ]
          ? numericPart
          : version,
        out var semver
      ) => semver.ToString(),
      _ => string.Empty,
    };
  } = DefaultNodePackageVersion;

  sealed override private protected void FinishSetup() => System.ArgumentException.ThrowIfNullOrEmpty(
    Version,
    $"{nameof(Version)} is neither SemanticVersion nor NodeVersion"
  );

  sealed override private protected string[] GetVerbBaseArguments() => [Version];
}
