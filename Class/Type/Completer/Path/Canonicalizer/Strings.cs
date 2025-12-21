namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public readonly static char PathSeparatorChar = '\\';
      public readonly static char FriendlyPathSeparatorChar = '/';
      public readonly static char DriveSeparatorChar = ':';
      public readonly static char HomeChar = '~';
      public readonly static string PathSeparator = @"\";
      public readonly static string FriendlyPathSeparator = "/";
      public readonly static string DriveSeparator = ":";
      public readonly static string Home = "~";
      public readonly static string PathSeparatorPattern = @"\\";
      public readonly static string FriendlyPathSeparatorPattern = "/";
      public readonly static string DuplicatePathSeparatorPattern = @"(?<!^)\\\\+";
      public readonly static string IsPathHomeRootedPattern = @"^(?=~(?>$|\\))";
      public readonly static string IsPathRelativelyRootedPattern = @"^(?=\.(?>$|\\))";
      public readonly static string IsPathRelativelyDriveRootedPattern = @"^(?=(?>[^:\\]+):(?>$|[^\\]))";
      public readonly static string IsPathDescendantPattern = @"^(?=(?>[.\\]*)$)";
      public readonly static string HasTrailingPathSeparatorPattern = @"(?<=(?<!^)(?<!:)\\)$";
      public readonly static string RemoveHomeRootPattern = @"^~(?>$|\\+)";
      public readonly static string RemoveRelativeRootPattern = @"^\.(?>$|\\+)";
      public readonly static string RemoveTrailingPathSeparatorPattern = @"(?>(?<!^)(?<!:)\\+)$";
      public readonly static string SubstituteHomeRootPattern = @"^~(?=$|\\)";
      public readonly static string SubstituteRelativeRootPattern = @"^\.(?=$|\\)";
    }
  } // namespace PathCompleter
} // namespace Completer
