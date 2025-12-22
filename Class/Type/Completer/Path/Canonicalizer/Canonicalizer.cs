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
            Escaper
              .Unescape(path)
              .Replace('/', '\\'),
            @"\"
          )
        );

        return !preserveTrailingSeparator
          && normalPath.EndsWith('\\')
          ? normalPath[..^1]
          : normalPath;
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

      public static bool IsHomeRooted(string path) => path.StartsWith('~')
        && (
          path.Length == 1
          || path[1] == '\\'
        );

      public static bool IsDescendantOf(
        string path,
        string location
      ) => IsDescendantOfRegex().IsMatch(
        Path.GetRelativePath(
          path,
          location
        )
      );

      public static string RemoveRelativeRoot(string path) => IsRelativelyRooted(path)
        ? path.Length == 1
          ? string.Empty
          : path[2..]
        : path;

      public static string RemoveHomeRoot(string path) => RemoveHomeRootRegex().Replace(
        path,
        string.Empty
      );
    }
  } // namespace PathCompleter
} // namespace Completer
