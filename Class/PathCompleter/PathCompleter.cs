using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
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
    public static string EasyDirectorySeparator = "/";

    public static string NormalDirectorySeparator = @"\";

    public static string EasyDirectorySeparatorPattern = "/";

    public static string NormalDirectorySeparatorPattern = @"\\";

    public static string DuplicateDirectorySeparatorPattern = @"(?<!^)\\\\+";

    public static string DescendantPattern = @"^(?=(?>[.\\]*)$)";

    public static string TildeRootedPattern = @"^(?=~(?>$|\\))";

    public static string TildeRootPattern = @"^~(?>\\*)";

    public static string CurrentRelativePattern = @"^\.(?>\\+)";

    public static string TrailingSeparatorPattern = @"(?>\\+)$";

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
