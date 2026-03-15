namespace PowerModule.Client.File;

static class Handler
{
  internal enum EditorWindow
  {
    Default,
    New,
    Reuse,
  }

  internal const string DefaultEditorProfile = "Default";
  internal const string EditorProfileSetting = "Setting";
  internal const string EditorProfileSvelte = "Svelte";

  static internal HashSet<string> EditorProfile = new(
    System.StringComparer.OrdinalIgnoreCase
  ) {
    DefaultEditorProfile,
    EditorProfileSetting,
    EditorProfileSvelte,
  };

  static internal void Edit() => Edit(string.Empty);
  static internal void Edit(string path) => Start.CreateProcess(
    Environment.Application.VSCode,
    path
  );
  static internal void Edit(
    string path,
    string[] arguments
  ) => Start.CreateProcess(
    Environment.Application.VSCode,
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
    string[] arguments
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
    string[] arguments
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
    string profile,
    EditorWindow window
  ) => Edit(
    path,
    profile,
    window,
    []
  );
  static internal void Edit(
    string path,
    string profile,
    EditorWindow window,
    string[] arguments
  ) => Edit(
    path,
    window,
    EditorProfile.TryGetValue(
      profile,
      out var exactProfile
    )
    && exactProfile is not DefaultEditorProfile
      ? [
          $"--profile={exactProfile}",
          .. arguments,
        ]
      : arguments
  );
}
