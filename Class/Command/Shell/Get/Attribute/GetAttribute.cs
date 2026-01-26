namespace Module.Command.Shell.Get.Attribute;

using System.Linq;

[Cmdlet(
  VerbsCommon.Get,
  "Size",
  DefaultParameterSetName = "String",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("sz", "size")]
[OutputType(
  typeof(string),
  ParameterSetName = ["String"]
)]
[OutputType(
  typeof(double),
  ParameterSetName = ["Number"]
)]
public sealed class GetSize : CoreCommand
{
  [Parameter(
    ParameterSetName = "String",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Parameter(
    ParameterSetName = "Number",
    Position = 0,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "The path of the file or directory to be measured."
  )]
  [PathCompletions]
  public string[] Path
  {
    get => paths;
    set => paths = value;
  }
  private string[] paths = [];

  [Parameter(
    ParameterSetName = "String",
    Position = 1
  )]
  [Parameter(
    ParameterSetName = "Number",
    Position = 1,
    HelpMessage = "The unit in which to return the size."
  )]
  [EnumCompletions(
    typeof(DiskSizeUnit)
  )]
  public string Unit
  {
    get => unit.ToString();
    set => unit = Enum.TryParse(
      value,
      true,
      out DiskSizeUnit parsedUnit
    )
      ? parsedUnit
      : DiskSizeUnit.KB;
  }
  private DiskSizeUnit unit = DiskSizeUnit.KB;

  [Parameter(
    ParameterSetName = "Number",
    HelpMessage = "If specified, returns the size as a numeric value instead of a formatted string."
  )]
  public SwitchParameter Number
  {
    get => number;
    set => number = value;
  }
  private bool number;

  private long factor = DiskSize.Factor[DiskSizeUnit.KB];

  protected sealed override void ProcessRecord()
  {
    foreach (string path in paths)
    {
      if (!TestPath(path))
      {
        Throw(
          new IO.FileNotFoundException(
            $"The path '{path}' does not exist."
          ),
          "PathNotFound",
          ErrorCategory.InvalidOperation,
          path
        );
      }

      long bytes = TestPath(path, FileSystemItemType.Directory)
        ? new DirectoryInfo(
            Pwd(path)
          )
          .EnumerateFiles(
            "*",
            SearchOption.AllDirectories
          )
          .Sum(
            file => file.Length
          )
        : new FileInfo(
            Pwd(path)
          )
          .Length;

      double scaledSize = (double)bytes / factor;

      WriteObject(
        number
          ? scaledSize
          : System.Math.Round(
              scaledSize,
              3
            )
            .ToString()
            + " "
            + unit.ToString()
      );
    }
  }

  private protected sealed override void TransformParameters()
  {
    if (paths.Length == 0)
    {
      paths = [Pwd()];
    }

    if (DiskSize.Factor.TryGetValue(unit, out long value))
    {
      factor = value;
    }
  }
}
