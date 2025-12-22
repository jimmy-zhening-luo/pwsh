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
        string normalPath = RemoveRelativeRootRegex().Replace(
          DuplicatePathSeparatorRegex().Replace(
            Escaper
              .Unescape(path)
              .Replace('/', '\\'),
            @"\"
          ),
          string.Empty
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

      public static bool IsPathHomeRooted(string path) => path.StartsWith('~')
        && (
          path.Length == 1
          || path[1] == '\\'
        );

      public static bool IsPathDescendantOf(
        string path,
        string location
      ) => IsPathDescendantOfRegex().IsMatch(
        Path.GetRelativePath(
          path,
          location
        )
      );
  
      public static string RemoveHomeRoot(string path) => RemoveHomeRootRegex().Replace(
        path,
        string.Empty
      );
    }
  } // namespace PathCompleter
} // namespace Completer
