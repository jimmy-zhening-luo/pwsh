namespace Module.Tab.Path;

public sealed partial class PathCompleter : TabCompleter
{
  private readonly string Location;

  private readonly PathItemType ItemType;

  private readonly bool Flat;

  private readonly bool Hidden;

  private readonly bool AllowReanchor;

  public PathCompleter(
    string location,
    PathItemType itemType,
    bool flat,
    bool hidden
  ) => (
    Location,
    ItemType,
    Flat,
    Hidden,
    AllowReanchor
  ) = (
    Canonicalize(
      location
    ),
    itemType,
    flat,
    hidden,
    location is ""
  );

  private protected sealed override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    string pathToComplete = Client.FileSystem.PathString.Normalize(
      wordToComplete,
      true
    );
    string accumulatedSubpath = string.Empty;
    string fullAccumulatedPath = string.Empty;
    string filter = string.Empty;

    while (pathToComplete is not "")
    {
      int pathEnd = pathToComplete.LastIndexOf(
        '\\'
      );

      if (pathEnd < 0)
      {
        filter = pathToComplete;
        pathToComplete = string.Empty;
      }
      else
      {
        string subpathPart = pathToComplete[..pathEnd]
          .Trim();

        int wordStart = pathEnd + 1;

        if (wordStart < pathToComplete.Length)
        {
          filter = pathToComplete[wordStart..]
            .Trim();
        }

        if (subpathPart is "")
        {
          pathToComplete = string.Empty;
        }
        else
        {
          string anchoredPath = System.IO.Path.GetFullPath(
            subpathPart,
            Location
          );

          if (
            System.IO.Directory.Exists(
              anchoredPath
            )
          )
          {
            accumulatedSubpath = System.IO.Path.GetRelativePath(
              Location,
              anchoredPath
            );
            fullAccumulatedPath = anchoredPath;
            pathToComplete = string.Empty;
          }
          else if (
            AllowReanchor
            && System.IO.Directory.Exists(
              subpathPart
            )
          )
          {
            accumulatedSubpath = System.IO.Path.GetFullPath(
              subpathPart
            );
            fullAccumulatedPath = accumulatedSubpath;
            pathToComplete = string.Empty;
          }
          else
          {
            filter = string.Empty;
            pathToComplete = subpathPart;
          }
        }
      }
    }

    if (fullAccumulatedPath is "")
    {
      fullAccumulatedPath = Location;
    }

    int count = default;
    filter += "*";
    System.IO.EnumerationOptions options = new()
    {
      IgnoreInaccessible = default
    };

    if (Hidden)
    {
      options.AttributesToSkip = System.IO.FileAttributes.System;
    }

    if (ItemType is PathItemType.File)
    {
FileFirstMatch:
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          fullAccumulatedPath,
          filter,
          options
        )
      )
      {
        ++count;
        yield return CompletionString(
          file,
          accumulatedSubpath
        );
      }

      if (
        count is 0
        && filter.Length > 1
        && options.AttributesToSkip is not System.IO.FileAttributes.System
      )
      {
        options.AttributesToSkip = System.IO.FileAttributes.System;

        goto FileFirstMatch;
      }
    }

    int checkpoint = count;

Match:
    foreach (
      var directory in System.IO.Directory.EnumerateDirectories(
        fullAccumulatedPath,
        filter,
        options
      )
    )
    {
      ++count;
      yield return CompletionString(
        directory,
        accumulatedSubpath,
        !Flat
      );
    }

    if (
      count == checkpoint
      && filter.Length > 1
      && options.AttributesToSkip is not System.IO.FileAttributes.System
    )
    {
      options.AttributesToSkip = System.IO.FileAttributes.System;

      goto Match;
    }

    checkpoint = count;

    if (ItemType is PathItemType.Any)
    {
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          fullAccumulatedPath,
          filter,
          options
        )
      )
      {
        ++count;
        yield return CompletionString(
          file,
          accumulatedSubpath
        );
      }

      if (
        count == checkpoint
        && filter.Length > 1
        && options.AttributesToSkip is not System.IO.FileAttributes.System
      )
      {
        options.AttributesToSkip = System.IO.FileAttributes.System;

        goto Match;
      }
    }

    if (accumulatedSubpath is not "")
    {
      yield return Denormalize(
        @"\",
        accumulatedSubpath
      );
    }

    if (
      accumulatedSubpath is not ""
      || count is not 0
    )
    {
      yield return Denormalize(
        @"..\",
        accumulatedSubpath
      );
    }

    yield break;
  }
}
