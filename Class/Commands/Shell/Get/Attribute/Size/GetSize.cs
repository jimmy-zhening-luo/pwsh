namespace Module.Commands.Shell.Get.Attribute.Size;

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
    [System.ComponentModel.Description("Bytes")]
    b,

    [System.ComponentModel.Description("Kilobytes")]
    kb,

    [System.ComponentModel.Description("Megabytes")]
    mb,

    [System.ComponentModel.Description("Gigabytes")]
    gb,

    [System.ComponentModel.Description("Terabytes")]
    tb,

    [System.ComponentModel.Description("Petabytes")]
    pb,
  }

  [Parameter(
    ParameterSetName = "String",
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Parameter(
    ParameterSetName = "Number",
    Position = default,
    ValueFromPipeline = true,
    ValueFromPipelineByPropertyName = true,
    HelpMessage = "Path of the file or directory of which to get the size"
  )]
  [Tab.Path.PathCompletions]
  public string[] Path
  {
    get => paths is []
      ? [""]
      : paths;
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
    HelpMessage = "Unit in which to return the size"
  )]
  public DiskSizeUnit Unit { get; set; } = DiskSizeUnit.kb;

  [Parameter(
    ParameterSetName = "Number",
    HelpMessage = "If specified, returns the size as a numeric value instead of a formatted string"
  )]
  public SwitchParameter Number { get; set; }

  private protected sealed override void Process()
  {
    foreach (var path in Path)
    {
      var fullPath = Pwd(path);

      if (!System.IO.Path.Exists(fullPath))
      {
        ThrowError(
          new System.IO.FileNotFoundException(
            $"The path '{path}' does not exist."
          ),
          ErrorCategory.InvalidOperation,
          fullPath,
          "PathNotFound"
        );
      }

      long bytes = default;

      if (System.IO.Directory.Exists(fullPath))
      {
        foreach (
          var file in new System.IO.DirectoryInfo(fullPath).EnumerateFiles(
            "*",
            System.IO.SearchOption.AllDirectories
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

      double scaledSize = (double)bytes / (
        1L << ((int)Unit * 10)
      );

      WriteObject(
        Number
          ? scaledSize
          : $"{System.Math.Round(scaledSize, 3)} {Unit.ToString().ToUpper()}"
      );
    }
  }
}
