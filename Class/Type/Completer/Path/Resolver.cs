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
        bool nonexistent = false,
        bool requireSubpath = false
      )
      {
        // placeholder
        if (nonexistent && requireSubpath && itemType == PathItemType.Any)
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
        bool nonexistent = false,
        bool requireSubpath = false
      )
      {
        // placeholder
        if (nonexistent && requireSubpath && itemType == PathItemType.Any)
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
