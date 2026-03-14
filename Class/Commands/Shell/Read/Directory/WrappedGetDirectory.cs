namespace PowerModule.Commands.Shell.Read.Directory;

abstract public class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem",
  StandardParameter.Path
)
{
  abstract public string[] Path
  { get; init; }

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
    init => filter = value switch
    {
      null or "" => "*",
      _ when value.Contains(
        '*',
        global::System.StringComparison.Ordinal
      ) => value,
      _ => $"{value}*",
    };
  }
  string filter = string.Empty;

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    init => Discard();
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => Discard();
  }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    init => Discard();
  }

  [Parameter]
  [Alias("de")]
  public uint Depth
  {
    init => Discard();
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => Discard();
  }

  [Parameter]
  public SwitchParameter Name
  {
    init => Discard();
  }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  {
    init => Discard();
  }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  {
    init => Discard();
  }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  {
    init => Discard();
  }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  {
    init => Discard();
  }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  {
    init => Discard();
  }

  [Parameter]
  public SwitchParameter FollowSymlink
  {
    init => Discard();
  }

  [Parameter]
  [Tab.EnumCompletions(
    typeof(System.IO.FileAttributes),
    Case = Tab.CompletionCase.Lower
  )]
  required public FlagsExpression<System.IO.FileAttributes> Attributes
  {
    init => Discard();
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      SetBoundParameter(
        StandardParameter.Path,
        ReanchorPath(Path)
      );
    }
  }
}
