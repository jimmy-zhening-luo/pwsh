namespace Module.Commands.Shell.Get.Attribute.Size;

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
public sealed partial class GetSize : CoreCommand
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
  public string[] Path { get; set; } = [];

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
      : System.Enum.TryParse(
          value,
          true,
          out DiskSizeUnitAlias parsedUnitAlias
        )
        ? (DiskSizeUnit)parsedUnitAlias
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

  private protected sealed override void ProcessRecordAction()
  {
    if (Path.Length == 0)
    {
      Path = [
        Pwd()
      ];
    }

    var bits = (int)unit * 10;
    long factor = 1L << bits;

    foreach (var path in Path)
    {
      var absolutePath = Pwd(
        path
      );

      if (
        !System.IO.Path.Exists(
          absolutePath
        )
      )
      {
        Throw(
          new System.IO.FileNotFoundException(
            $"The path '{path}' does not exist."
          ),
          "PathNotFound",
          ErrorCategory.InvalidOperation,
          absolutePath
        );
      }

      long bytes = System.IO.Directory.Exists(
        absolutePath
      )
        ? new System.IO.DirectoryInfo(
            absolutePath
          )
          .EnumerateFiles(
            "*",
            System.IO.SearchOption.AllDirectories
          )
          .Sum(
            file => file.Length
          )
        : new System.IO.FileInfo(
            absolutePath
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
