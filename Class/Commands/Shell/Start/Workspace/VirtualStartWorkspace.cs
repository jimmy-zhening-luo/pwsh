namespace Module.Commands.Shell.Start.Workspace;

abstract public class VirtualStartWorkspace() : NativeCommand(true, true)
{
  private const string FlagNewWindow = "--new-window";
  private const string FlagReuseWindow = "--reuse-window";

  abstract public string Path { set; }
  private protected string path = string.Empty;

  [Parameter(Position = 1)]
  [ValidateNotNullOrWhiteSpace]
  public string Name
  {
    private get;
    set;
  } = string.Empty;

  [Parameter]
  public SwitchParameter Window
  {
    private get;
    set;
  }

  [Parameter]
  public SwitchParameter ReuseWindow
  {
    private get;
    set;
  }

  sealed override private protected string CommandPath { get; } = Client.Environment.Known.Application.VSCode;

  sealed override private protected void PreprocessArguments()
  {
    switch (Name)
    {
      case "":
        break;

      case "se":
        NativeArguments.Add("--profile=Setting");
        break;

      case var profile when IsNativeArgument(profile):
        NativeArguments.Add(Name);
        break;

      default:
        WriteWarning("Profiles not supported except for se (Setting)");
        break;
    }

    if (Window || ReuseWindow)
    {
      _ = NativeArguments.RemoveAll(a => a is FlagNewWindow);
      _ = NativeArguments.RemoveAll(a => a is FlagReuseWindow);

      NativeArguments.Add(
        Window
          ? FlagNewWindow
          : FlagReuseWindow
      );
    }
  }

  sealed override private protected List<string> BuildNativeCommand() => [
    InCurrentLocation
    && path is ""
    && (
      Pwd() == Client.Environment.Known.Folder.Code()
      || Pwd() == Client.Environment.Known.Folder.Home()
    )
      ? string.Empty
      : ReanchorPath(path),
  ];
}
