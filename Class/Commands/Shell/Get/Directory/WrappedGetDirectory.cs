namespace Module.Commands.Shell.Get.Directory;

public abstract class WrappedGetDirectory() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-ChildItem"
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
    get => filter;
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
  public required string[] Include { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude { get; set; }

  [Parameter]
  [Alias("s", "r")]
  public SwitchParameter Recurse { get; set; }

  [Parameter]
  [Alias("de")]
  public uint Depth { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force { get; set; }

  [Parameter]
  public SwitchParameter Name { get; set; }

  [Parameter]
  [Alias("ad", "d")]
  public SwitchParameter Directory { get; set; }

  [Parameter]
  [Alias("af", "fi")]
  public SwitchParameter File { get; set; }

  [Parameter]
  [Alias("ah", "h")]
  public SwitchParameter Hidden { get; set; }

  [Parameter]
  [Alias("as")]
  public SwitchParameter System { get; set; }

  [Parameter]
  [Alias("ar")]
  public SwitchParameter ReadOnly { get; set; }

  [Parameter]
  public SwitchParameter FollowSymlink { get; set; }

  [Parameter]
  [EnumCompletions(typeof(System.IO.FileAttributes))]
  public required FlagsExpression<System.IO.FileAttributes> Attributes { get; set; }

  private protected sealed override Dictionary<string, object?> CoercedParameters => new()
  {
    ["Filter"] = filter,
  };

  private protected sealed override void TransformPipelineInput()
  {
    if (!UsingCurrentLocation)
    {
      if (Path is [])
      {
        Path = [
          Reanchor(),
        ];
      }
      else
      {
        for (
          int i = default;
          i < Path.Length;
          ++i
        )
        {
          Path[i] = Reanchor(Path[i]);
        }
      }

      BoundParameters["Path"] = Path;
    }
  }
}
