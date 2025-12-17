using System;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace Typed
{
  public static class TypedPath
  {
    public readonly static char PathSeparatorChar = '\\';
    public readonly static char FriendlyPathSeparatorChar = '/';
    public readonly static string PathSeparator = @"\";
    public readonly static string FriendlyPathSeparator = "/";
    public readonly static string PathSeparatorPattern = @"\\";
    public readonly static string FriendlyPathSeparatorPattern = "/";
    public readonly static string DuplicatePathSeparatorPattern = @"(?<!^)\\\\+";
    public readonly static string TrailingPathSeparatorPattern = @"(?>\\+)$";
    public readonly static string RelativeRootPattern = @"^\.(?>\\+)";
    public readonly static string TildeRootPattern = @"^~(?>\\*)";
    public readonly static string IsPathDescendantPattern = @"^(?=(?>[.\\]*)$)";
    public readonly static string IsPathTildeRootedPattern = @"^(?=~(?>$|\\))";

    public static string Format(
      string path,
      string separator = "",
      bool trimLeadingRelative = false,
      bool trimTrailing = false
    )
    {
      string normalPath = Regex.Replace(
        path.Replace(
          FriendlyPathSeparatorChar,
          PathSeparatorChar
        ),
        DuplicatePathSeparatorPattern,
        PathSeparator
      );

      string pretrimmedNormalPath = trimLeadingRelative
        ? Regex.Replace(
            normalPath,
            RelativeRootPattern,
            string.Empty
          )
        : normalPath;
      string trimmedNormalPath = trimTrailing
        ? Regex.Replace(
            pretrimmedNormalPath,
            TrailingPathSeparatorPattern,
            string.Empty
          )
        : pretrimmedNormalPath;

      return separator != string.Empty && separator != PathSeparator
        ? trimmedNormalPath.Replace(
            PathSeparator,
            separator
          )
        : trimmedNormalPath;
    }
  }
}
