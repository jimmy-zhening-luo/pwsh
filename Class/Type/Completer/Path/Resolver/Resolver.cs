using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public class Resolver
    {
      public static string Test(
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

      public static string AnchorHome(string path)
      {
        if (!path.StartsWith('~'))
        {
          return path;
        }

        string home = Home();
        if (path == "~")
        {
          return home;
        }

        if (
          path[1] == '\\'
          || path[1] == '/'
        )
        {
          return home + path[1..];
        }
        else
        {
          return path;
        }
      }
    }
  } // namespace PathCompleter
} // namespace Completer
