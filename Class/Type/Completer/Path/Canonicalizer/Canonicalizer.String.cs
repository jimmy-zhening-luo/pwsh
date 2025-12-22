using System.Text.RegularExpressions;

namespace Completer
{
  namespace PathCompleter
  {
    public static partial class Canonicalizer
    {
      public static char[] RelativeChars = {
        '.',
        '\\'
      };
    }
  } // namespace PathCompleter
} // namespace Completer
