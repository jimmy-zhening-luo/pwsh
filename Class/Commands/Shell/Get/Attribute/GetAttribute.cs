namespace Module.Commands.Shell.Get.Attribute;

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
  public enum DiskSizeUnit
  {
    B,
    KB,
    MB,
    GB,
    TB,
    PB
  }

  private enum DiskSizeAlias
  {
    B,
    KB,
    K = KB,
    MB,
    M = MB,
    GB,
    G = GB,
    TB,
    T = TB,
    PB,
    P = PB
  }

  internal static readonly Dictionary<DiskSizeUnit, long> DiskSizeFactor = new() {
    { DiskSizeUnit.B, 1L },
    { DiskSizeUnit.KB, 1L << 10 },
    { DiskSizeUnit.MB, 1L << 20 },
    { DiskSizeUnit.GB, 1L << 30 },
    { DiskSizeUnit.TB, 1L << 40 },
    { DiskSizeUnit.PB, 1L << 50 }
  };

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
    set => unit = System.Enum.TryParse(
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

  private long factor = DiskSizeFactor[
    DiskSizeUnit.KB
  ];

  private protected sealed override void TransformParameters()
  {
    if (paths.Length == 0)
    {
      paths = [
        Pwd()
      ];
    }

    if (
      DiskSizeFactor.TryGetValue(
        unit,
        out long value
      )
    )
    {
      factor = value;
    }
  }

  private protected sealed override void ProcessRecordAction()
  {
    foreach (string path in paths)
    {
      if (
        !TestPath(
          path
        )
      )
      {
        Throw(
          new System.IO.FileNotFoundException(
            $"The path '{path}' does not exist."
          ),
          "PathNotFound",
          ErrorCategory.InvalidOperation,
          path
        );
      }

      long bytes = TestPath(
        path,
        FileSystemItemType.Directory
      )
        ? new System.IO.DirectoryInfo(
            Pwd(path)
          )
          .EnumerateFiles(
            "*",
            System.IO.SearchOption.AllDirectories
          )
          .Sum(
            file => file.Length
          )
        : new System.IO.FileInfo(
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
}
