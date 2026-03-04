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
  public SwitchParameter Window { get; set; }

  [Parameter]
  public SwitchParameter ReuseWindow { get; set; }

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

      case var profile when IsNativeArgument(profile):
        NativeArguments.Add(Name);
        break;

      default:
        WriteWarning("Profiles not supported except for se (Setting)");
        break;
    }

    _ = NativeArguments.RemoveAll(a => a is FlagNewWindow);
    _ = NativeArguments.RemoveAll(a => a is FlagReuseWindow);

    if (Window)
    {
      NativeArguments.Add(FlagNewWindow);
    }
    else if (ReuseWindow)
    {
      NativeArguments.Add(FlagReuseWindow);
    }
  }

  private protected sealed override List<string> BuildNativeCommand() => [
    InCurrentLocation
    && Path is ""
    && (
      Pwd() == Client.Environment.Known.Folder.Code()
      || Pwd() == Client.Environment.Known.Folder.Home()
    )
      ? string.Empty
      : ReanchorPath(Path),
  ];
}
