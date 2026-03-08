namespace Module.Commands.Shell.Get.Directory;

public abstract class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem",
  "Path"
)
{
  public abstract string[] Path { get; set; }
  private protected string[] paths = [];

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
  public required string[] Include
  {
    private get;
    set;
  }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude
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
  public required FlagsExpression<System.IO.FileAttributes> Attributes
  {
    private get;
    set;
  }

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["Filter"] = filter,
  };

  private protected sealed override void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      BoundParameters["Path"] = paths = ReanchorPath(paths);
    }
  }
}
