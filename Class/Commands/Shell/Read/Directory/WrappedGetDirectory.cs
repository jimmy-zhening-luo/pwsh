namespace PowerModule.Commands.Shell.Read.Directory;

abstract public class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem"
)
{
  sealed override private protected PipelineInputSource PipelineInput => () => (
    StandardParameter.Path,
    Path
  );

  abstract public string[] Path
  { get; set; }

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
    init => _ = value;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("de")]
  public uint Depth
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => _ = value;
  }

  [Parameter]
  public SwitchParameter Name
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  {
    init => _ = value;
  }

  [Parameter]
  public SwitchParameter FollowSymlink
  {
    init => _ = value;
  }

  [Parameter]
  [Tab.EnumCompletions(
    typeof(System.IO.FileAttributes),
    Casing = Tab.CompletionCase.Lower
  )]
  required public FlagsExpression<System.IO.FileAttributes> Attributes
  {
    init => _ = value;
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      Path = ReanchorPath(Path);
    }
  }
}
