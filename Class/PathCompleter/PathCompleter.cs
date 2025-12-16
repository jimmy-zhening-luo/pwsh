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
    public readonly static string NormalDirectorySeparator = @"\";
    public readonly static string EasyDirectorySeparator = "/";
    public readonly static string NormalDirectorySeparatorPattern = @"\\";
    public readonly static string EasyDirectorySeparatorPattern = "/";
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
          EasyDirectorySeparator,
          NormalDirectorySeparator
        ),
        DuplicateDirectorySeparatorPattern,
        NormalDirectorySeparator
      );

      string pretrimmedNormalPath = trimLeadingRelative
        ? Regex.Replace(
            normalPath,
            CurrentRelativePattern,
            String.Empty
          )
        : normalPath;
      string trimmedNormalPath = trimTrailing
        ? Regex.Replace(
            pretrimmedNormalPath,
            TrailingSeparatorPattern,
            String.Empty
          )
        : pretrimmedNormalPath;

      return separator.Length != 0 && separator != NormalDirectorySeparator
        ? trimmedNormalPath.Replace(
            NormalDirectorySeparator,
            separator
          )
        : trimmedNormalPath;
    }
  }
}
