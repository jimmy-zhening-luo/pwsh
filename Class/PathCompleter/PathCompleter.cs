using System;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace PathCompleter
{
  public enum PathProvider
  {
    Any,
    FileSystem,
    Registry,
    Environment,
    Variable,
    Alias,
    Function
  }

  public enum PathItemType
  {
    Any,
    File,
    Directory,
    RegistryKey,
    EnvironmentVariable,
    Variable,
    Alias,
    Function
  }

  public static class PathSyntax
  {
    public readonly static string DirectorySeparator = @"\";
    public readonly static string FriendlyDirectorySeparator = "/";
    public readonly static string DirectorySeparatorPattern = @"\\";
    public readonly static string FriendlyDirectorySeparatorPattern = "/";
    public readonly static string DuplicateDirectorySeparatorPattern = @"(?<!^)\\\\+";
    public readonly static string DescendantPattern = @"^(?=(?>[.\\]*)$)";
    public readonly static string TildeRootedPattern = @"^(?=~(?>$|\\))";
    public readonly static string TildeRootPattern = @"^~(?>\\*)";
    public readonly static string CurrentRelativePattern = @"^\.(?>\\+)";
    public readonly static string TrailingSeparatorPattern = @"(?>\\+)$";

    public static string Format(
      string path,
      string separator = "",
      bool trimLeadingRelative = false,
      bool trimTrailing = false
    )
    {
      string normalPath = Regex.Replace(
        path.Replace(
          FriendlyDirectorySeparator,
          DirectorySeparator
        ),
        DuplicateDirectorySeparatorPattern,
        DirectorySeparator
      );

      string pretrimmedNormalPath = trimLeadingRelative
        ? Regex.Replace(
            normalPath,
            CurrentRelativePattern,
            string.Empty
          )
        : normalPath;
      string trimmedNormalPath = trimTrailing
        ? Regex.Replace(
            pretrimmedNormalPath,
            TrailingSeparatorPattern,
            string.Empty
          )
        : pretrimmedNormalPath;

      return separator.Length != 0 && separator != DirectorySeparator
        ? trimmedNormalPath.Replace(
            DirectorySeparator,
            separator
          )
        : trimmedNormalPath;
    }
  }
}
