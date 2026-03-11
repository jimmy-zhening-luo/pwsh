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
    set => filter = value switch
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
  required public Collection<string> Include
  {
    init => Bind();
  }

  [Parameter]
  [SupportsWildcards]
  required public Collection<string> Exclude
  {
    init => Bind();
  }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    init => Bind();
  }

  [Parameter]
  [Alias("de")]
  public uint Depth
  {
    init => Bind();
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    init => Bind();
  }

  [Parameter]
  public SwitchParameter Name
  {
    init => Bind();
  }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  {
    init => Bind();
  }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  {
    init => Bind();
  }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  {
    init => Bind();
  }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  {
    init => Bind();
  }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  {
    init => Bind();
  }

  [Parameter]
  public SwitchParameter FollowSymlink
  {
    init => Bind();
  }

  [Parameter]
  [Tab.EnumCompletions(
    typeof(System.IO.FileAttributes),
    Tab.CompletionCase.Lower
  )]
  required public FlagsExpression<System.IO.FileAttributes> Attributes
  {
    init => Bind();
  }

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
