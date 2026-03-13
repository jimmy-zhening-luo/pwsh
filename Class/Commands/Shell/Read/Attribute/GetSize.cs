namespace PowerModule.Commands.Shell.Read.Attribute;

[Cmdlet(
  VerbsCommon.Get,
  "Size",
  DefaultParameterSetName = "Unit"
)]
[Alias("sz", "size")]
[OutputType(
  typeof(string),
  ParameterSetName = ["Unit"]
)]
[OutputType(
  typeof(double),
  ParameterSetName = ["Number"]
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
    ParameterSetName = "Unit",
    Position = default,
    ValueFromPipeline = true
  )]
  [Parameter(
    ParameterSetName = "Number",
    Position = default,
    ValueFromPipeline = true,
    HelpMessage = "Path of the file or directory of which to get the size"
  )]
  [ValidateLength(1, int.MaxValue)]
  [Tab.PathCompletions]
  public string[] Path
  {
    get => paths is []
      ? [""]
      : paths;
    init => paths = value;
  }
  string[] paths = [];

  [Parameter(
    ParameterSetName = "Unit",
    Position = 1,
    HelpMessage = "Get the size as the specified unit"
  )]
  public DiskSizeUnit Unit
  { private get; init; } = DiskSizeUnit.kb;

  [Parameter(
    ParameterSetName = "Number",
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
            "*",
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
        ParameterSetName is "Number"
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
