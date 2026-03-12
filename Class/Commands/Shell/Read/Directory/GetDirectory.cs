namespace PowerModule.Commands.Shell.Read.Directory;

[Cmdlet(
  VerbsCommon.Get,
  "Directory",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectory : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  [Parameter(
    ParameterSetName = "LiteralItems",
    Mandatory = true
  )]
  [Alias("PSPath", "LP")]
  required public string[] LiteralPath
  {
    init => Discard();
  }
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectorySibling",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectorySibling : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.Parent,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryRelative",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lxx")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryRelative : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.ParentParent,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => () => Pwd(Client.File.PathString.ParentParent);
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryHome",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lh")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryHome : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryCode",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("lc")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryCode : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsCommon.Get,
  "DirectoryDrive",
  DefaultParameterSetName = "Items",
  HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096492"
)]
[Alias("l/")]
[OutputType(
  typeof(System.IO.DirectoryInfo),
  typeof(System.IO.FileInfo)
)]
sealed public class GetDirectoryDrive : WrappedGetDirectory
{
  [Parameter(
    ParameterSetName = "Items",
    Position = default,
    ValueFromPipeline = true
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.StringSeparator,
    Tab.PathItemType.Directory
  )]
  sealed override public string[] Path
  { get; init; } = [];

  sealed override private protected Localizer Location => Drive;
}
