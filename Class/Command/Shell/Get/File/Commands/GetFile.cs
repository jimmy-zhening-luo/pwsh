namespace Module.Command.Shell.Get.File.Commands;

[Cmdlet(
  VerbsCommon.Get,
  "File",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p")]
[OutputType(typeof(string))]
public sealed class GetFile : WrappedGetFile
{
  [PathCompletions(
    "",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Mandatory = true,
    ValueFromPipelineByPropertyName = true
  )]
  [Alias("PSPath", "LP")]
  public string[]? LiteralPath;

  [Parameter(
    ParameterSetName = "LiteralPath",
    Position = 1
  )]
  public override string Filter => base.Filter;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileSibling",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("px")]
[OutputType(typeof(string))]
public sealed class GetFileSibling : WrappedGetFile
{
  [PathCompletions(
    "..",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  private protected sealed override string LocationSubpath => "..";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileRelative",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pxx")]
[OutputType(typeof(string))]
public sealed class GetFileRelative : WrappedGetFile
{
  [PathCompletions(
    @"..\..",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  private protected sealed override string LocationSubpath => @"..\..";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileHome",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("ph")]
[OutputType(typeof(string))]
public sealed class GetFileHome : WrappedGetFile
{
  [PathCompletions(
    "~",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  private protected sealed override string Location => Home();
}

[Cmdlet(
  VerbsCommon.Get,
  "FileCode",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("pc")]
[OutputType(typeof(string))]
public sealed class GetFileCode : WrappedGetFile
{
  [PathCompletions(
    @"~\code",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  private protected sealed override string Location => Home();

  private protected sealed override string LocationSubpath => "code";
}

[Cmdlet(
  VerbsCommon.Get,
  "FileDrive",
  DefaultParameterSetName = "Path",
  SupportsTransactions = true,
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096490"
)]
[Alias("p/")]
[OutputType(typeof(string))]
public sealed class GetFileDrive : WrappedGetFile
{
  [PathCompletions(
    @"\",
    PathItemType.File
  )]
  public override string[] Path => base.Path;

  private protected sealed override string Location => Drive();
}
