namespace Module.Client.File;

static internal class Handler
{
  static internal void Edit() => Edit(string.Empty);
  static internal void Edit(string path) => Start.CreateProcess(
    Environment.Known.Application.VSCode,
    path
  );
  static internal void Edit(
    string path,
    IList<string> arguments
  ) => Start.CreateProcess(
    Environment.Known.Application.VSCode,
    [
      path,
      .. arguments,
    ]
  );
  static internal void Edit(
    string path,
    string profile
  ) => Edit(
    path,
    profile,
    []
  );
  static internal void Edit(
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
  static internal void Edit(
    string path,
    bool newWindow,
    bool reuseWindow
  ) => Edit(
    path,
    newWindow,
    reuseWindow,
    []
  );
  static internal void Edit(
    string path,
    bool newWindow,
    bool reuseWindow,
    IList<string> arguments
  ) => Edit(
    path,
    newWindow
      ? [
          "--new-window",
          .. arguments,
        ]
      : reuseWindow
        ? [
            "--reuse-window",
            .. arguments,
          ]
        : arguments
  );
  static internal void Edit(
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
  static internal void Edit(
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
