namespace PowerModule.Commands.Shell.Read.Attribute;

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
sealed public class GetSize : CoreCommand
{
  public enum DiskSizeUnit
  {
    [System.ComponentModel.Description(
      "Bytes"
    )]
    b,

    [System.ComponentModel.Description(
      "Kilobytes"
    )]
    kb,

    [System.ComponentModel.Description(
      "Megabytes"
    )]
    mb,

    [System.ComponentModel.Description(
      "Gigabytes"
    )]
    gb,

    [System.ComponentModel.Description(
      "Terabytes"
    )]
    tb,

    [System.ComponentModel.Description(
      "Petabytes"
    )]
    pb,
  }

  [Parameter(
    ParameterSetName = "String",
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
  public Collection<string> Path
  {
    get => paths is []
      ? [""]
      : paths;
    init => paths = value;
  }
  private Collection<string> paths = [];

  [Parameter(
    ParameterSetName = "String",
    Position = 1,
    HelpMessage = "Unit in which to return the size"
  )]
  public DiskSizeUnit Unit
  { private get; init; } = DiskSizeUnit.kb;

  [Parameter(
    ParameterSetName = "Number",
    HelpMessage = "If specified, returns the size as the number of bytes instead of a formatted string"
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

      switch (ParameterSetName)
      {
        case "Number":
          WriteObject(bytes);
          break;

        default:
          var scaledSize = (double)bytes / (
            1L << ((int)Unit * 10)
          );

          WriteObject(
            $"{System.Math.Round(
              scaledSize,
              3
            )} {Unit
              .ToString()
              .ToUpper(
                Client.String.CurrentCulture
            )}"
          );
          break;
      }


    }
  }
}
