using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static class Resolver
    {
      public static string Test(
        string path,
        string location = "",
        PathItemType itemType = PathItemType.Any
        bool newable = false,
        bool requireSubpath = false
      )
      {
        // placeholder
        if (newable && requireSubpath && itemType == PathItemType.Any)
        {
          return path;
        }
        else
        {
          return location;
        }
      }

      public static string Resolve(
        string path,
        string location = "",
        PathItemType itemType = PathItemType.Any
        bool newable = false,
        bool requireSubpath = false
      )
      {
        // placeholder
        if (newable && requireSubpath && itemType == PathItemType.Any)
        {
          return path;
        }
        else
        {
          return location;
        }
      }
    }
  }
}
