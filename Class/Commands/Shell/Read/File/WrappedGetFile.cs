namespace PowerModule.Commands.Shell.Read.File;

abstract public class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  abstract public string[] Path
  { init; }
  private protected string[] paths = [];

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    init => Discard();
  }

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
  public long ReadCount
  {
    init => Discard();
  }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long TotalCount
  {
    init => Discard();
  }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public int Tail
  {
    init => Discard();
  }

  [Parameter]
  required public string Delimiter
  {
    init => Discard();
  }

  [Parameter]
  required public string Stream
  {
    init => Discard();
  }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
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
  public SwitchParameter AsByteStream
  {
    init => Discard();
  }

  [Parameter]
  public SwitchParameter Raw
  {
    init => Discard();
  }

  [Parameter]
  public SwitchParameter Wait
  {
    init => Discard();
  }

  sealed override private protected void TransformPipelineInput()
  {
    if (!InCurrentLocation)
    {
      SetBoundParameter(
        "Path",
        ReanchorPath(paths)
      );
    }
  }
}
