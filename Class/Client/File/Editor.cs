namespace PowerModule.Client.File;

static class Editor
{
  internal enum Window
  {
    Default,
    New,
    Reuse,
  }

  internal const string DefaultProfile = "Default";
  internal const string ProfileSetting = "Setting";
  internal const string ProfileSvelte = "Svelte";

  static internal HashSet<string> EditorProfile = new(
    System.StringComparer.OrdinalIgnoreCase
  ) {
    ProfileSetting,
    ProfileSvelte,
    DefaultProfile,
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
    Window window
  ) => Edit(
    path,
    window,
    []
  );
  static internal void Edit(
    string path,
    Window window,
    string[] arguments
  ) => Edit(
    path,
    window switch
    {
      Window.New => [
        "--new-window",
        .. arguments,
      ],
      Window.Reuse => [
        "--reuse-window",
        .. arguments,
      ],
      _ => arguments,
    }
  );
  static internal void Edit(
    string path,
    string profile,
    Window window
  ) => Edit(
    path,
    profile,
    window,
    []
  );
  static internal void Edit(
    string path,
    string profile,
    Window window,
    string[] arguments
  ) => Edit(
    path,
    window,
    EditorProfile.TryGetValue(
      profile,
      out var exactProfile
    )
    && exactProfile is not DefaultProfile
      ? [
          $"--profile={exactProfile}",
          .. arguments,
        ]
      : arguments
  );
}
