namespace PowerModule.Client.File;

static class Handler
{
  internal enum EditorWindow
  {
    Any,
    New,
    Reuse,
  }

  internal enum EditorProfile
  {
    Default,
    Setting,
    Svelte,
  }

  static internal void Edit() => Edit(string.Empty);
  static internal void Edit(string path) => Start.CreateProcess(
    Environment.Application.VSCode,
    path
  );
  static internal void Edit(
    string path,
    IEnumerable<string> arguments
  ) => Start.CreateProcess(
    Environment.Application.VSCode,
    [
      path,
      .. arguments,
    ]
  );
  static internal void Edit(
    string path,
    EditorProfile profile
  ) => Edit(
    path,
    profile,
    []
  );
  static internal void Edit(
    string path,
    EditorProfile profile,
    IEnumerable<string> arguments
  ) => Edit(
    path,
    profile,
    default,
    arguments
  );
  static internal void Edit(
    string path,
    EditorWindow window
  ) => Edit(
    path,
    window,
    []
  );
  static internal void Edit(
    string path,
    EditorWindow window,
    IEnumerable<string> arguments
  ) => Edit(
    path,
    window switch
    {
      EditorWindow.New => [
        "--new-window",
        .. arguments,
      ],
      EditorWindow.Reuse => [
        "--reuse-window",
        .. arguments,
      ],
      _ => arguments,
    }
  );
  static internal void Edit(
    string path,
    EditorProfile profile,
    EditorWindow window
  ) => Edit(
    path,
    profile,
    window,
    []
  );
  static internal void Edit(
    string path,
    EditorProfile profile,
    EditorWindow window,
    IEnumerable<string> arguments
  ) => Edit(
    path,
    window,
    profile switch
    {
      EditorProfile.Default => arguments,
      _ => [
        $"--profile={profile}",
        .. arguments,
      ],
    }
  );
}
