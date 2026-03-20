namespace PowerModule.Commands.Shell.Read.Directory;

abstract public class WrappedGetDirectory() : WrappedCommand(
  $@"{StandardModule.Management}\Get-ChildItem"
)
{
  private protected const string DefaultParameterSet = "Items";

  sealed override private protected PipelineInputSource PipelineInput => () => (
    nameof(Path),
    Path
  );

  abstract public string[] Path
  { get; set; }

  [Parameter(
    Position = 1
  )]
  [SupportsWildcards]
  [ValidateNotNullOrEmpty]
  public string Filter
  {
    private get;
    init => field = value.Contains(
      Client.StringInput.Wildcard,
      global::System.StringComparison.Ordinal
    )
      ? value
      : $@"{value}{Client.StringInput.StringWildcard}";
  } = string.Empty;

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

  sealed override private protected void TransformParameters()
  {
    if (Filter is not "")
    {
      CoercedParameters[nameof(Filter)] = Filter;
    }
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      Path = ReanchorPath(Path);
    }
  }
}
