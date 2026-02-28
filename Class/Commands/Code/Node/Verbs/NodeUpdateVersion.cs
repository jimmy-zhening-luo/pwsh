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
    patch,
    minor,
    major,
    prerelease,
    prepatch = prerelease,
    preminor,
    premajor,
  }

  private sealed class NodeVersionCompletionsAttribute() : EnumCompletionsAttribute(
    typeof(NodeVersion),
    default,
    ["from-git"],
    ["prepatch"]
  );

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
    set
    {
      if (value is "")
      {
        version = nameof(NodeVersion.patch);
      }
      else if (value.ToLower() is "from-git")
      {
        version = "from-git";
      }
      else if (
        System.Enum.TryParse<NodeVersion>(
          value,
          true,
          out var namedVersion
        )
      )
      {
        version = namedVersion.ToString();
      }
      else
      {
        var numericVersion = value.StartsWith(
          "v",
          System.StringComparison.OrdinalIgnoreCase
        )
          ? value[1..]
          : value;

        if (
          SemanticVersion.TryParse(
            numericVersion,
            out var semver
          )
        )
        {
          version = semver.ToString();
        }
        else
        {
          version = string.Empty;
        }
      }
    }
  }
  private string version = nameof(NodeVersion.patch);

  private protected sealed override List<string> ParseArguments()
  {
    if (version is "")
    {
      Throw(
        "Provided version was neither a well-known version nor parseable as a semantic version."
      );
    }

    List<string> arguments = [version.ToLower()];

    if (
      WorkingDirectory is not ""
      && !IsNodePackage(WorkingDirectory)
    )
    {
      arguments.Add(WorkingDirectory);
      WorkingDirectory = string.Empty;
    }

    return arguments;
  }
}
