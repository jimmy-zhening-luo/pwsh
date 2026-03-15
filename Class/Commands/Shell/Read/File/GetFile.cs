namespace PowerModule.Commands.Shell.Read.File;

[Cmdlet(
  VerbsCommon.Get,
  "File",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("p")]
[OutputType(typeof(string))]
sealed public class GetFile : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "",
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  [Parameter(
    ParameterSetName = StandardParameter.LiteralPath,
    Mandatory = true
  )]
  [Alias(StandardAlias.PSPath, StandardAlias.LP)]
  required public string[] LiteralPath
  {
    init => _ = value;
  }
}

[Cmdlet(
  VerbsCommon.Get,
  "FileSibling",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("px")]
[OutputType(typeof(string))]
sealed public class GetFileSibling : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.Parent,
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  sealed override private protected Localizer Location => Parent;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileRelative",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("pxx")]
[OutputType(typeof(string))]
sealed public class GetFileRelative : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.ParentParent,
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  sealed override private protected Localizer Location => ParentParent;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileHome",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("ph")]
[OutputType(typeof(string))]
sealed public class GetFileHome : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    "~",
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Home;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileCode",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("pc")]
[OutputType(typeof(string))]
sealed public class GetFileCode : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    @"~\code",
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  sealed override private protected Localizer Location
  { get; } = Client.Environment.Folder.Code;
}

[Cmdlet(
  VerbsCommon.Get,
  "FileDrive",
  DefaultParameterSetName = StandardParameter.Path,
  HelpUri = $"{HelpLink}2096490"
)]
[Alias("p/")]
[OutputType(typeof(string))]
sealed public class GetFileDrive : WrappedGetFile
{
  [Parameter(
    ParameterSetName = StandardParameter.Path,
    Position = default
  )]
  [SupportsWildcards]
  [Tab.PathCompletions(
    Client.File.PathString.StringSeparator,
    Tab.PathItemType.File
  )]
  sealed override public string[] Path
  { private protected get; init; } = [];

  sealed override private protected Localizer Location => Drive;
}
