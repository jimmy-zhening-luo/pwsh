namespace Module.Tab.Path;

public sealed class PathCompleter : TabCompleter
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

  private static string Canonicalize(
    string path,
    bool preserveTrailingSeparator = default
  )
  {
    var normalPath = Client.FileSystem.PathString.Normalize(
      path,
      preserveTrailingSeparator
    );

    var homedNormalPath = normalPath.StartsWith(
      '~'
    )
      ? normalPath.Length is 1
        ? Client.Environment.Known.Folder.Home()
        : normalPath[1] is '\\'
          ? Client.Environment.Known.Folder.Home(
              normalPath[2..]
            )
          : normalPath
        : normalPath;

    return System.IO.Path.IsPathFullyQualified(
      homedNormalPath
    )
      ? homedNormalPath
      : PowerShellHost.CurrentDirectory(
          homedNormalPath
        );
  }

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

  private protected sealed override IEnumerable<string> GenerateCompletions(
    string wordToComplete
  )
  {
    var pathToComplete = Client.FileSystem.PathString.Normalize(
      wordToComplete,
      true
    );
    var accumulatedSubpath = string.Empty;
    var fullAccumulatedPath = string.Empty;
    var filter = string.Empty;

    while (pathToComplete is not "")
    {
      var pathEnd = pathToComplete.LastIndexOf(
        '\\'
      );

      if (pathEnd < 0)
      {
        filter = pathToComplete;
        pathToComplete = string.Empty;
      }
      else
      {
        var subpathPart = pathToComplete[..pathEnd]
          .Trim();

        var wordStart = pathEnd + 1;

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
          var anchoredPath = System.IO.Path.GetFullPath(
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
        && options is not
        {
          AttributesToSkip: System.IO.FileAttributes.System
        }
      )
      {
        options.AttributesToSkip = System.IO.FileAttributes.System;

        goto FileFirstMatch;
      }
    }

    var checkpoint = count;

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
      && options is not
      {
        AttributesToSkip: System.IO.FileAttributes.System
      }
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
        && options is not
        {
          AttributesToSkip: System.IO.FileAttributes.System
        }
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
