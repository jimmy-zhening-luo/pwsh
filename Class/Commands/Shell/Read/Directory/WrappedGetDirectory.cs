namespace PowerModule.Commands.Shell.Read.Directory;

abstract public class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem",
  AcceptsPipelineInput: true
)
{
  abstract public Collection<string> Path
  { get; init; }

  sealed override private protected object? PipelineInput => Path;

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    ["Filter"] = filter,
  };

  [Parameter(
    Position = 1
  )]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  public string Filter
  {
    private get => filter;
    set
    {
      filter = value switch
      {
        null or "" => "*",
        _ when value.Contains('*') => value,
        _ => $"{value}*",
      };
    }
  }
  private string filter = string.Empty;

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Include
  { private get; init; }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  { private get; init; }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  { private get; init; }

  [Parameter]
  [Alias("de")]
  public uint Depth
  { private get; init; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  { private get; init; }

  [Parameter]
  public SwitchParameter Name
  { private get; init; }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  { private get; init; }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  { private get; init; }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  { private get; init; }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  { private get; init; }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  { private get; init; }

  [Parameter]
  public SwitchParameter FollowSymlink
  { private get; init; }

  [Parameter]
  [Tab.EnumCompletions(
    typeof(System.IO.FileAttributes),
    Tab.CompletionCase.Lower
  )]
  required public FlagsExpression<System.IO.FileAttributes> Attributes
  { private get; init; }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      SetBoundParameter(
        "Path",
        ReanchorPath(Path)
      );
    }
  }
}
