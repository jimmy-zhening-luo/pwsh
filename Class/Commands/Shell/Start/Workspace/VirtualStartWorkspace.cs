namespace Module.Commands.Shell.Start.Workspace;

public abstract class VirtualStartWorkspace() : CoreCommand(
  true
)
{
  private protected string path = string.Empty;

  [Parameter(
    Position = 1
  )]
  public string Name
  {
    get => profileName;
    set => profileName = value;
  }
  private string profileName = string.Empty;

  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  public string[] Argument
  {
    get => arguments;
    set => arguments = value;
  }
  private string[] arguments = [];

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

  private protected sealed override void AfterEndProcessing()
  {
    path = UsingCurrentLocation
      ? Pwd(path)
      : Reanchor(path);

    var argumentList = new List<string>()
    {
      path
    };

    if (!string.IsNullOrEmpty(profileName))
    {
      if (
        profileName.StartsWith("--")
      )
      {
        argumentList.Add(
          profileName
        );
      }
      else
      {
        if (profileName == "se")
        {
          argumentList.Add(
            "--profile=Setting"
          );
        }
        else
        {
          WriteWarning(
            "Profiles not supported except for se == Setting lmaoo."
          );
        }
      }
    }

    if (window)
    {
      argumentList.Add(
        "--new-window"
      );
    }
    else if (reuseWindow)
    {
      argumentList.Add(
        "--reuse-window"
      );
    }

    if (arguments.Length != 0)
    {
      argumentList.AddRange(
        arguments
      );
    }

    Client.Invocation.CreateProcess(
      Client.Environment.Known.Application.VSCode,
      argumentList,
      true
    );
  }
}
