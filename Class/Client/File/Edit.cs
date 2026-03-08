namespace Module.Client.File;

static internal class Handler
{
  static void Edit() => Edit(string.Empty);
  static void Edit(string path) => Start.CreateProcess(
    Environment.Known.Application.VSCode,
    path
  );
  static void Edit(
    string path,
    IList<string> arguments
  ) => Start.CreateProcess(
    Environment.Known.Application.VSCode,
    [
      path,
      .. arguments,
    ]
  );
  static void Edit(
    string path,
    string profile
  ) => Edit(
    path,
    profile,
    []
  );
  static void Edit(
    string path,
    string profile,
    IList<string> arguments
  ) => Edit(
    path,
    [
      $"--profile={profile}",
      .. arguments,
    ]
  );
  static void Edit(
    string path,
    bool newWindow,
    bool reuseWindow
  ) => Edit(
    path,
    newWindow,
    reuseWindow,
    []
  );
  static void Edit(
    string path,
    bool newWindow,
    bool reuseWindow,
    IList<string> arguments
  ) => Edit(
    path,
    [
      .. (
        newWindow
          ? ["--new-window"]
          : reuseWindow
            ? ["--reuse-window"]
            : []
      ),
      .. arguments,
    ]
  );
  static void Edit(
    string path,
    string profile,
    bool newWindow,
    bool reuseWindow
  ) => Edit(
    path,
    profile,
    newWindow,
    reuseWindow,
    []
  );
  static void Edit(
    string path,
    string profile,
    bool newWindow,
    bool reuseWindow,
    IList<string> arguments
  ) => Edit(
    path,
    newWindow,
    reuseWindow,
    [
      $"--profile={profile}",
      .. arguments,
    ]
  );
}
