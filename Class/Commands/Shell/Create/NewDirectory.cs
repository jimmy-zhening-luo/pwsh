namespace PowerModule.Commands.Shell.Create;

[Cmdlet(
  VerbsCommon.New,
  "Directory",
  DefaultParameterSetName = "pathSet",
  SupportsShouldProcess = true,
  ConfirmImpact = ConfirmImpact.Medium,
  HelpUri = $"{HelpLink}2096592"
)]
[Alias("mk")]
[OutputType(typeof(System.IO.DirectoryInfo))]
sealed public class NewDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\New-Item"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    StandardParameter.Value,
    Value
  );

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    ["ItemType"] = itemType ?? "directory",
  };

  [Parameter(
    ParameterSetName = "pathSet",
    Mandatory = true,
    Position = default
  )]
  [Parameter(
    ParameterSetName = "nameSet",
    Position = default
  )]
  [Tab.PathCompletions]
  required public string[] Path
  {
    init => _ = value;
  }

  [Parameter(
    ParameterSetName = "nameSet",
    Mandatory = true
  )]
  [AllowNull]
  [AllowEmptyString]
  required public string Name
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("Type")]
  [Tab.Completions(
    "file",
    "symboliclink",
    "hardlink",
    "junction",
    "directory"
  )]
  public string ItemType
  {
    init => itemType = value;
  }
  string? itemType;

  [Parameter(ValueFromPipeline = true)]
  [Alias("Target")]
  [Tab.PathCompletions]
  required public object Value
  { get; init; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => _ = value;
  }
}
