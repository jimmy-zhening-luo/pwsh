namespace Module.Tab.Path;

public sealed class PathCompleter(
  string Root,
  PathItemType Type,
  bool Flat,
  bool Hidden,
  bool Reanchor
) : TabCompleter
{
  private protected sealed override IEnumerable<string> FulfillCompletion(
    string wordToComplete
  )
  {
    string pathToComplete = Client.FileSystem.PathString.Normalize(
      wordToComplete,
      true
    );
    string accumulatedSubpath = string.Empty;
    string location = string.Empty;
    string filter = string.Empty;

    while (
      !string.IsNullOrEmpty(
        pathToComplete
      )
    )
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

        if (
          string.IsNullOrEmpty(
            subpathPart
          )
        )
        {
          pathToComplete = string.Empty;
        }
        else
        {
          string anchoredPath = System.IO.Path.GetFullPath(
            subpathPart,
            Root
          );

          if (
            System.IO.Directory.Exists(
              anchoredPath
            )
          )
          {
            accumulatedSubpath = System.IO.Path.GetRelativePath(
              Root,
              anchoredPath
            );
            location = anchoredPath;
            pathToComplete = string.Empty;
          }
          else if (
            Reanchor
            && System.IO.Directory.Exists(
              subpathPart
            )
          )
          {
            accumulatedSubpath = System.IO.Path.GetFullPath(
              subpathPart
            );
            location = accumulatedSubpath;
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

    if (
      string.IsNullOrEmpty(
        location
      )
    )
    {
      location = Root;
    }

    int count = default;
    filter += "*";
    var options = new System.IO.EnumerationOptions()
    {
      IgnoreInaccessible = default
    };

    if (Hidden)
    {
      options.AttributesToSkip = System.IO.FileAttributes.System;
    }

    if (Type == PathItemType.File)
    {
FileFirstMatch:
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          location,
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
        count == 0
        && filter.Length > 1
        && options.AttributesToSkip != System.IO.FileAttributes.System
      )
      {
        options.AttributesToSkip = System.IO.FileAttributes.System;

        goto FileFirstMatch;
      }
    }

    int checkpoint = count;
    string directoryCap = Flat
      ? string.Empty
      : @"\";

Match:
    foreach (
      var directory in System.IO.Directory.EnumerateDirectories(
        location,
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
      && options.AttributesToSkip != System.IO.FileAttributes.System
    )
    {
      options.AttributesToSkip = System.IO.FileAttributes.System;

      goto Match;
    }

    checkpoint = count;

    if (Type == PathItemType.Any)
    {
      foreach (
        var file in System.IO.Directory.EnumerateFiles(
          location,
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
        && options.AttributesToSkip != System.IO.FileAttributes.System
      )
      {
        options.AttributesToSkip = System.IO.FileAttributes.System;

        goto Match;
      }
    }

    if (
      !string.IsNullOrEmpty(
        accumulatedSubpath
      )
    )
    {
      yield return CompletionString(
        @"\",
        accumulatedSubpath
      );
    }

    if (
      !string.IsNullOrEmpty(
        accumulatedSubpath
      )
      || count != 0
    )
    {
      yield return CompletionString(
        @"..\",
        accumulatedSubpath
      );
    }

    yield break;
  }

  private string CompletionString(
    string path,
    string accumulatedSubpath,
    bool trailingSeparator = default
  ) => Canonicalizer.Denormalize(
    System.IO.Path.GetFileName(
      path
    ),
    accumulatedSubpath,
    trailingSeparator
      ? @"\"
      : string.Empty
  );
}
