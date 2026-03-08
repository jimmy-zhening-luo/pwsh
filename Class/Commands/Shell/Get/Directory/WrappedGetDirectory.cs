namespace Module.Commands.Shell.Get.Directory;

abstract public class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem",
  AcceptsPipelineInput: true
)
{
  abstract public string[] Path { get; set; }

  sealed override private protected object? PipelineInput => Path;

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
      filter = value.Contains('*')
        ? value
        : $"{value}*";
    }
  }
  private string filter = string.Empty;

  [Parameter]
  [SupportsWildcards]
  required public string[] Include
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  required public string[] Exclude
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("de")]
  public uint Depth
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter Name
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System
  {
    private get;
    set;
  }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter FollowSymlink
  {
    private get;
    set;
  }

  [Parameter]
  [Tab.EnumCompletions(
    typeof(System.IO.FileAttributes),
    Case = Tab.CompletionCase.Lower
  )]
  required public FlagsExpression<System.IO.FileAttributes> Attributes
  {
    private get;
    set;
  }

  sealed override private protected Dictionary<string, object?> CoercedParameters => new()
  {
    ["Filter"] = filter,
  };

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = Path = ReanchorPath(Path);
    }
  }
}
