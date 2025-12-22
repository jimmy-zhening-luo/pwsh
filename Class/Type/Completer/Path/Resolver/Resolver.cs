namespace Completer
{
  namespace PathCompleter
  {
    public interface IResolver
    {
      public static abstract string Test(
        string path,
        string location = "",
        PathItemType itemType = PathItemType.Any,
        bool newable = false,
        bool requireSubpath = false
      );

      public static abstract string Resolve(
        string path,
        string location = "",
          PathItemType itemType = PathItemType.Any,
        bool newable = false,
        bool requireSubpath = false
      );
    }
  } // namespace PathCompleter
} // namespace Completer
