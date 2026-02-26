namespace Module.Commands.Shell.Get.File;

public abstract class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  public abstract string[] Path { get; set; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  public required string Filter { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Include { get; set; }

  [Parameter]
  [SupportsWildcards]
  public required string[] Exclude { get; set; }

  [Parameter]
  public long ReadCount { get; set; }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(
    0,
    9223372036854775807
  )]
  public long TotalCount { get; set; }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(
    0,
    2147483647
  )]
  public int Tail { get; set; }

  [Parameter]
  public required string Delimiter { get; set; }

  [Parameter]
  public required string Stream { get; set; }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [EnumCompletions(
    typeof(Client.FileSystem.Encoding)
  )]
  public required string Encoding { get; set; }

  [Parameter]
  [Alias("f")]
  public SwitchParameter Force { get; set; }

  [Parameter]
  public SwitchParameter AsByteStream { get; set; }

  [Parameter]
  public SwitchParameter Raw { get; set; }

  [Parameter]
  public SwitchParameter Wait { get; set; }

  private protected sealed override void TransformPipelineInput()
  {
    if (!UsingDefaultLocation)
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
          Path[i] = Reanchor(
            Path[i]
          );
        }
      }

      BoundParameters["Path"] = Path;
    }
  }
}
