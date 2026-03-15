namespace PowerModule.Commands.Shell.Read.File;

abstract public class WrappedGetFile() : WrappedCommand(
  @"Microsoft.PowerShell.Management\Get-Content"
)
{
  abstract public string[] Path
  { private protected get; init; }

  [Parameter]
  [SupportsWildcards]
  required public string Filter
  {
    init => _ = value;
  }

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
  public long ReadCount
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("First", "Head")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public long TotalCount
  {
    init => _ = value;
  }

  [Parameter]
  [Alias("Last")]
  [ValidateRange(ValidateRangeKind.NonNegative)]
  public int Tail
  {
    init => _ = value;
  }

  [Parameter]
  required public string Delimiter
  {
    init => _ = value;
  }

  [Parameter]
  required public string Stream
  {
    init => _ = value;
  }

  [Parameter]
  [ValidateNotNullOrEmpty]
  [Tab.EnumCompletions(typeof(Client.File.Encoding))]
  required public string Encoding
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
  public SwitchParameter AsByteStream
  {
    init => _ = value;
  }

  [Parameter]
  public SwitchParameter Raw
  {
    init => _ = value;
  }

  [Parameter]
  public SwitchParameter Wait
  {
    init => _ = value;
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
