using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public static string Normalize(
        string path,
        bool preserveTrailingSeparator = false
      )
      {
        string normalPath = RemoveRelativeRoot(
          DuplicateSeparatorRegex().Replace(
            Environment
              .ExpandEnvironmentVariables(path)
              .Replace('/', '\\'),
            @"\"
          )
        );

        return preserveTrailingSeparator
          ? normalPath
          : Path.TrimEndingDirectorySeparator(
              normalPath
            );
      }

      public static string Denormalize(
        string path,
        string location = "",
        string subpath = ""
      ) => Path
        .Join(
          location,
          path,
          subpath
        )
        .Replace('\\', '/');

      public static bool IsRelativelyRooted(string path) => path.StartsWith('.')
        && (
          path.Length == 1
          || path[1] == '\\'
        );

      public static string ExpandEnvironmentVariables(string path) => Environment.ExpandEnvironmentVariables(
        path
      );

      public static bool IsHomeRooted(string path) => path.StartsWith('~')
        && (
          path.Length == 1
          || path[1] == '\\'
        );

      public static bool IsDescendantOf(
        string path,
        string location
      ) => Path
        .GetRelativePath(
          path,
          location
        )
        .Replace(".", string.Empty)
        .Replace(@"\", string.Empty) == string.Empty;

      public static string RemoveRelativeRoot(string path) => IsRelativelyRooted(path)
        ? path.Length == 1
          ? string.Empty
          : path[2..]
        : path;

      public static string RemoveHomeRoot(string path) => IsHomeRooted(path)
        ? path.Length == 1
          ? string.Empty
          : path[2..]
        : path;

      public static string Home() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
      );

      public static string AnchorHome(string path) => IsHomeRooted(path)
        ? Home() + path[1..]
        : path;
    }
  } // namespace PathCompleter
} // namespace Completer
