namespace PowerModule.Client.File;

static partial class PathString
{
  internal const char Here = '.';
  internal const char Home = '~';
  internal const char Separator = '\\';
  internal const char AltSeparator = '/';
  internal const string StringSeparator = @"\";
  internal const string StringHere = ".";
  internal const string Parent = "..";
  internal const string ParentParent = $@"{Parent}{StringSeparator}{Parent}";

  static internal string GetFullPathLocal(
    string location,
    string path
  ) => System.IO.Path.GetFullPath(
    Normalize(path),
    location
  );
  static internal string GetFullPathLocal(
    string location,
    string path,
    bool preserveTrailingSeparator
  ) => System.IO.Path.GetFullPath(
    Normalize(
      path,
      preserveTrailingSeparator
    ),
    location
  );

  static internal string Normalize(
    string path,
    bool preserveTrailingSeparator
  ) => preserveTrailingSeparator
    ? Normalize(path)
    : System.IO.Path.TrimEndingDirectorySeparator(
      Normalize(path)
    );
  static internal string Normalize(string path) => TrimRelativePrefix(
    ExpandHomePrefix(
      DuplicateSeparatorRegex().Replace(
        System.Environment
          .ExpandEnvironmentVariables(
            path
          )
          .Replace(
            AltSeparator,
            Separator
          ),
        StringSeparator
      )
    )
  );

  [System.Text.RegularExpressions.GeneratedRegex(
    @"(?<!^)(?>\\{2,})"
  )]
  static private partial System.Text.RegularExpressions.Regex DuplicateSeparatorRegex();

  static string ExpandHomePrefix(string path) => path switch
  {
    [Home] => Environment.Folder.Home(),
    [
      Home,
      Separator,
      .. var subpath,
    ] => Environment.Folder.Home(subpath),
    _ => path,
  };

  static string TrimRelativePrefix(string path) => path switch
  {
    [Here] => string.Empty,
    [
      Here,
      Separator,
      .. var subpath,
    ] => subpath,
    _ => path,
  };
}
