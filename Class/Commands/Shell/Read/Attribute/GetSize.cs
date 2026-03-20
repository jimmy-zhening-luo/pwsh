namespace PowerModule.Commands.Shell.Read.Attribute;

[Cmdlet(
  VerbsCommon.Get,
  "Size",
  DefaultParameterSetName = nameof(Unit)
)]
[Alias("sz", "size")]
[OutputType(
  typeof(string),
  ParameterSetName = [nameof(Unit)]
)]
[OutputType(
  typeof(double),
  ParameterSetName = [nameof(Number)]
)]
sealed public class GetSize : CoreCommand
{
  public enum DiskSizeUnit
  {
    b,
    kb,
    mb,
    gb,
    tb,
    pb,
  }

  [Parameter(
    ParameterSetName = nameof(Unit),
    Position = default,
    ValueFromPipeline = true
  )]
  [Parameter(
    ParameterSetName = nameof(Number),
    Position = default,
    ValueFromPipeline = true
  )]
  [ValidateNotNullOrEmpty]
  [ValidateNotNullOrWhiteSpace]
  [Tab.PathCompletions]
  public string[] Path
  {
    get => field ?? [string.Empty];
    init;
  }

  [Parameter(
    ParameterSetName = nameof(Unit),
    Position = 1
  )]
  public DiskSizeUnit Unit
  { private get; init; } = DiskSizeUnit.kb;

  [Parameter(
    ParameterSetName = nameof(Number),
    HelpMessage = "Get the size as total byte count"
  )]
  public SwitchParameter Number
  { private get; init; }

  sealed override private protected void Process()
  {
    foreach (var path in Path)
    {
      var fullPath = Pwd(path);

      if (!System.IO.Path.Exists(fullPath))
      {
        throw new System.IO.FileNotFoundException(
          $"Path does not exist: {fullPath}"
        );
      }

      long bytes = default;

      if (System.IO.Directory.Exists(fullPath))
      {
        foreach (
          var file in new System.IO.DirectoryInfo(fullPath).EnumerateFiles(
            Client.StringInput.StringWildcard,
            new System.IO.EnumerationOptions()
            {
              RecurseSubdirectories = true,
              AttributesToSkip = System.IO.FileAttributes.ReparsePoint,
            }
          )
        )
        {
          bytes += file.Length;
        }
      }
      else
      {
        bytes = new System.IO.FileInfo(fullPath).Length;
      }

      WriteObject(
        ParameterSetName is nameof(Number)
          ? bytes
          : $"{System.Math.Round(
              (double)bytes / (1L << ((int)Unit * 10)),
              3
            )} {Unit
              .ToString()
              .ToUpper(
                Client.StringInput.CurrentCulture
            )}"
      );
    }
  }
}
