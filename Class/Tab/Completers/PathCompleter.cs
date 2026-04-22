namespace PowerModule.Tab.Completers;

sealed class PathCompleter : Intrinsics.Completer
{
  sealed record SearchContext(
    System.IO.DirectoryInfo Container,
    string Filter,
    System.IO.EnumerationOptions Options,
    string Accumulator
  );

  readonly string Location;
  readonly PathItemType ItemType;
  readonly bool Flat;
  readonly bool AllowReanchor;

  bool matched;

  internal PathCompleter(
    CompletionCase Casing,
    string location,
    PathItemType itemType,
    bool flat
  ) : base(
    CompletionResultType.ProviderItem,
    Casing
  ) => (
    Location,
    ItemType,
    Flat,
    AllowReanchor
  ) = (
    Client.File.PathString.Normalize(location)
    is var normalPath
    && System.IO.Path.IsPathFullyQualified(
      normalPath
    )
      ? normalPath
      : Client.File.PathString.GetFullPathLocal(
        PowerModule.GetPowerShellHostLocation(),
        normalPath
      ),
    itemType,
    flat,
    location is ""
  );

  static SearchContext ParseLine(
    string wordToComplete,
    string location,
    bool allowReanchor
  )
  {
    var searchPath = location;
    var line = Client.File.PathString.Normalize(
      wordToComplete,
      true
    );
    var lineCaptured = string.Empty;
    var lineRemaining = string.Empty;

    while (line is not "")
    {
      var marker = line.LastIndexOf(
        Client.File.PathString.Separator
      );

      if (marker < 0)
      {
        (
          lineRemaining,
          line
        ) = (
          line,
          string.Empty
        );
      }
      else
      {
        var buffer = line[..marker];
        var next = marker + 1;

        if (next < line.Length)
        {
          lineRemaining = line[next..];
        }

        if (buffer is "")
        {
          line = string.Empty;
        }
        else
        {
          var fullPathCaptured = Client.File.PathString.GetFullPathLocal(
            location,
            buffer
          );

          if (
            System.IO.Directory.Exists(
              fullPathCaptured
            )
          )
          {
            (
              searchPath,
              lineCaptured,
              line
            ) = (
              fullPathCaptured,
              System.IO.Path.GetRelativePath(
                location,
                fullPathCaptured
              ),
              string.Empty
            );
          }
          else if (
            allowReanchor
            && System.IO.Directory.Exists(
              buffer
            )
          )
          {
            (
              searchPath,
              line
            ) = (
              lineCaptured = System.IO.Path.GetFullPath(
                buffer
              ),
              string.Empty
            );
          }
          else
          {
            (
              lineRemaining,
              line
            ) = (
              string.Empty,
              buffer
            );
          }
        }
      }
    }

    return new(
      new(searchPath),
      lineRemaining
      + Client.StringInput.StringWildcard,
      new()
      {
        IgnoreInaccessible = default,
        AttributesToSkip = System.IO.FileAttributes.NotContentIndexed,
      },
      lineCaptured
    );
  }

  static Intrinsics.ICompleter.Completion CreateCompletionRecord(
    string description,
    string accumulator,
    string filename,
    bool trailingSeparator = default
  ) => new(
    System.IO.Path.Join(
      accumulator,
      filename,
      trailingSeparator
        ? Client.File.PathString.StringSeparator
        : string.Empty
    )
      .Replace(
        Client.File.PathString.Separator,
        Client.File.PathString.AltSeparator
      ),
    description
  );

  sealed override private protected IEnumerable<Intrinsics.ICompleter.Completion> GenerateCompletion(string wordToComplete)
  {
    matched = default;

    var searchContext = ParseLine(
      wordToComplete,
      Location,
      AllowReanchor
    );

    switch (ItemType)
    {
      case PathItemType.Directory:
        foreach (
          var directory in Directories(
            searchContext,
            !Flat
          )
        )
        {
          yield return directory;
        }

        break;

      case PathItemType.File:
        foreach (
          var file in Files(
            searchContext
          )
        )
        {
          yield return file;
        }

        foreach (
          var directory in Directories(
            searchContext,
            true
          )
        )
        {
          yield return directory;
        }

        break;

      case PathItemType.Any:
        foreach (
          var directory in Directories(
            searchContext,
            !Flat
          )
        )
        {
          yield return directory;
        }

        foreach (
          var file in Files(
            searchContext
          )
        )
        {
          yield return file;
        }

        break;

      default:
        break;
    }

    if (searchContext.Accumulator is not "")
    {
      yield return CreateCompletionRecord(
        System.IO.Path.Join(
          searchContext.Container.FullName,
          Client.File.PathString.StringSeparator
        ),
        searchContext.Accumulator,
        Client.File.PathString.StringSeparator
      );
    }

    if (matched || searchContext.Accumulator is not "")
    {
      yield return CreateCompletionRecord(
        Client.File.PathString.GetFullPathLocal(
          searchContext.Container.FullName,
          Client.File.PathString.Parent
        ),
        searchContext.Accumulator,
        $@"{Client.File.PathString.Parent}{Client.File.PathString.StringSeparator}"
      );
    }

    yield break;
  }

  IEnumerable<Intrinsics.ICompleter.Completion> Directories(
    SearchContext searchContext,
    bool trailingSeparator = default
  ) => EnumerateResults(
    searchContext.Container.EnumerateDirectories(
      searchContext.Filter,
      searchContext.Options
    ),
    searchContext.Accumulator,
    trailingSeparator
  );

  IEnumerable<Intrinsics.ICompleter.Completion> Files(
    SearchContext searchContext
  ) => EnumerateResults(
    searchContext.Container.EnumerateFiles(
      searchContext.Filter,
      searchContext.Options
    ),
    searchContext.Accumulator
  );

  IEnumerable<Intrinsics.ICompleter.Completion> EnumerateResults(
    IEnumerable<System.IO.FileSystemInfo> items,
    string accumulator,
    bool trailingSeparator = default
  )
  {
    List<System.IO.FileSystemInfo> deferredItems = [];

    foreach (var item in items)
    {
      switch (
        (
          item.Attributes & System.IO.FileAttributes.Hidden,
          item.Attributes & System.IO.FileAttributes.System
        )
      )
      {
        case (0, _):
          matched = true;

          yield return CreateCompletionRecord(
            item.FullName,
            accumulator,
            item.Name,
            trailingSeparator
          );

          break;

        case (_, 0):
          deferredItems.Add(item);

          break;

        default:
          break;
      }
    }

    foreach (var item in deferredItems)
    {
      matched = true;

      yield return CreateCompletionRecord(
        item.FullName,
        accumulator,
        item.Name,
        trailingSeparator
      );
    }

    yield break;
  }
}
