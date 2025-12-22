using System;
using System.IO;

namespace Completer
{
  namespace PathCompleter
  {
    public class Resolver
    {
      public static string Home() => Environment.GetFolderPath(
        Environment
          .SpecialFolder
          .UserProfile
      );

      public static string AnchorHome(string path) => Canonicalizer.IsHomeRooted(path)
        ? Home() + path[1..]
        : path;
    }
  } // namespace PathCompleter
} // namespace Completer
