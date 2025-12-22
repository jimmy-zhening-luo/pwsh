namespace Completer
{
  namespace PathCompleter
  {
    public interface IResolver
    {
      public static abstract string Test(
        string path,
        string location,
        PathItemType type
      );

      public static abstract string Resolve(
        string path,
        string location,
        PathItemType type
      );
    }
  } // namespace PathCompleter
} // namespace Completer
