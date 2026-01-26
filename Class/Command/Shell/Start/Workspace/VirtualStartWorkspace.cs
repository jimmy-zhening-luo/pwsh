namespace Module.Command.Shell.Start.Workspace;

public abstract class VirtualStartWorkspace : CoreCommand
{
  [Parameter(
    Position = 0
  )]
  [AllowEmptyString]
  public string? Path;

  [Parameter(
    Position = 1
  )]
  [AllowEmptyString]
  public string Name
  {
    get => profileName;
    set => profileName = value;
  }
  private string profileName = "";

  [Parameter(
    Position = 2,
    ValueFromRemainingArguments = true,
    DontShow = true
  )]
  [AllowEmptyCollection]
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

  private protected sealed override bool NoSsh => true;

  private protected sealed override void TransformParameters()
  {
    if (string.IsNullOrEmpty(Path))
    {
      Path = Here
        ? Pwd()
        : Reanchor();
    }
    else if (!Here)
    {
      Path = Reanchor(Path);
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
    var argumentList = new List<string>(
      arguments
    );

    argumentList.Insert(
      0,
      Path!
    );

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

    CreateProcess(
      IO.Path.GetFullPath(
        @"Programs\Microsoft VS Code\bin\code.cmd",
        Env(
          "LOCALAPPDATA"
        )
      ),
      string.Join(
        ' ',
        argumentList
      ),
      true
    );
  }
}
