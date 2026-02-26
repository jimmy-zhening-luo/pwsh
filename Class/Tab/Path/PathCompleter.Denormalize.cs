namespace Module.Tab.Path;

public sealed partial class PathCompleter
{
  private static string CompletionString(
    string path,
    string accumulatedSubpath,
    bool trailingSeparator = default
  ) => Denormalize(
    System.IO.Path.GetFileName(
      path
    ),
    accumulatedSubpath,
    trailingSeparator
      ? @"\"
      : string.Empty
  );

  private static string Denormalize(
    string path,
    string location = "",
    string subpath = ""
  ) => System.IO
    .Path
    .Join(
      location,
      path,
      subpath
    )
    .Replace(
      '\\',
      '/'
    );
}
