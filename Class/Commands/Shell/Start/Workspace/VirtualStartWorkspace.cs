namespace Module.Commands.Shell.Start.Workspace;

public abstract class VirtualStartWorkspace() : NativeCommand(true, true)
{
  private const string FlagNewWindow = "--new-window";
  private const string FlagReuseWindow = "--reuse-window";

  public abstract string Path { get; set; }
  private protected string path = string.Empty;

  [Parameter(Position = 1)]
  public string Name { get; set; } = string.Empty;

  [Parameter]
  public SwitchParameter Window
  {
    get => window;
    set => window = value;
  }
  private bool window;

  [Parameter]
  public SwitchParameter ReuseWindow
  {
    get => reuseWindow;
    set => reuseWindow = value;
  }
  private bool reuseWindow;

  private protected sealed override string CommandPath => Client.Environment.Known.Application.VSCode;

  private protected sealed override void PreprocessArguments()
  {
    switch (Name)
    {
      case "":
        break;

      case "se":
        NativeArguments.Add("--profile=Setting");
        break;

      case var profile when NativeArgumentRegex().IsMatch(profile):
        NativeArguments.Add(Name);
        break;

      default:
        WriteWarning("Profiles not supported except for se (Setting)");
        break;
    }

    if (window && !NativeArguments.Contains(FlagNewWindow))
    {
      _ = NativeArguments.Remove(FlagReuseWindow);

      NativeArguments.Add(FlagNewWindow);
    }
    if (
      reuseWindow
      && !NativeArguments.Contains(FlagReuseWindow)
      && !NativeArguments.Contains(FlagNewWindow)
    )
    {
      NativeArguments.Add(FlagReuseWindow);
    }
  }

  private protected sealed override List<string> BuildNativeCommand() => [
    InCurrentLocation
      ? Path is ""
        ? Pwd() == Client.Environment.Known.Folder.Code()
        || Pwd() == Client.Environment.Known.Folder.Home()
          ? string.Empty
          : Reanchor(Pwd())
        : Pwd(Path)
      : Reanchor(Path),
  ];
}
