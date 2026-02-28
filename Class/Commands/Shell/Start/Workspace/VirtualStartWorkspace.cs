namespace Module.Commands.Shell.Start.Workspace;

public abstract class VirtualStartWorkspace() : CoreCommand(true)
{
  public abstract string Path { get; set; }
  private protected string path = string.Empty;

  [Parameter(
    Position = 1
  )]
  public string Name { get; set; } = string.Empty;

  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  public string[] Argument { get; set; } = [];

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

  private protected sealed override void Postprocess()
  {
    List<string> argumentList = [
      UsingDefaultLocation
        ? Pwd(Path)
        : Reanchor(Path),
    ];

    switch (Name)
    {
      case "":
        break;

      case "se":
        argumentList.Add("--profile=Setting");

        break;

      case Name.StartsWith("--"):
        argumentList.Add(Name);

        break;

      default:
        WriteWarning(
          "Profiles not supported except for se (Setting) lmaoo."
        );

        break;
    }

    if (window)
    {
      argumentList.Add("--new-window");
    }
    else if (reuseWindow)
    {
      argumentList.Add("--reuse-window");
    }

    if (Argument is not [])
    {
      argumentList.AddRange(Argument);
    }

    Client.Start.CreateProcess(
      Client.Environment.Known.Application.VSCode,
      argumentList,
      true
    );
  }
}
