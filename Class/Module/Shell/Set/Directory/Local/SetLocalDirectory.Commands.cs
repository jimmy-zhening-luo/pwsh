namespace Module.Shell.Set.Directory.Local
{
  namespace Commands
  {
    using System.Management.Automation;
    using Completer.PathCompleter;

    [Cmdlet(
      VerbsCommon.Set,
      "DirectorySibling",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c.")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectorySibling : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        "..",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => "..";

      protected override string LocationRoot() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryRelative",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c..")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryRelative : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"..\..",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => @"..\..";

      protected override string LocationRoot() => Pwd();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryHome",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("ch")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryHome : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [PathCompletions(
        "~",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => "";

      protected override string LocationRoot() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DirectoryCode",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("cc")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDirectoryCode : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [PathCompletions(
        @"~\code",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => "code";

      protected override string LocationRoot() => Context.Home();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "Drive",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("c/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDrive : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        @"\",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => "";

      protected override string LocationRoot() => Drive();
    }

    [Cmdlet(
      VerbsCommon.Set,
      "DriveD",
      SupportsTransactions = true,
      HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097049"
    )]
    [Alias("d/")]
    [OutputType(
      typeof(PathInfo)
    )]
    public class SetDriveD : SetLocalDirectoryCommand
    {
      [Parameter(
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true
      )]
      [AllowEmptyString]
      [SupportsWildcards]
      [RelativePathCompletions(
        "D:",
        PathItemType.Directory
      )]
      public new string Path;

      protected override string RelativeLocation() => "";

      protected override string LocationRoot() => @"D:\";
    }
  }
}
