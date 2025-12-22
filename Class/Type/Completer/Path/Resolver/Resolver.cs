using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public class Resolver
    {
      public static bool Test(
        string path,
        string location = "",
        PathItemType itemType = PathItemType.Any,
        bool newable = false,
        bool requireSubpath = false
      )
      {
        return true;
      }

      public static string Resolve(
        string path,
        string location = "",
          PathItemType itemType = PathItemType.Any,
        bool newable = false,
        bool requireSubpath = false
      )
      {
        return path;
      }

      public static string Home() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
      );

      public static string AnchorHome(string path) => Canonicalizer.IsPathHomeRooted(path)
        ? Home() + path[1..]
        : path;
    }
  } // namespace PathCompleter
} // namespace Completer
