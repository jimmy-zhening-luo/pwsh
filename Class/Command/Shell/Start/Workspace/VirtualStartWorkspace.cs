namespace Module.Command.Shell.Start.Workspace;

public abstract class VirtualStartWorkspace : CoreCommand
{
  public abstract string Path;
  private protected string path = "";

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
    if (string.IsNullOrEmpty(path))
    {
      path = Here
        ? Pwd()
        : Reanchor();
    }
    else if (!Here)
    {
      path = Reanchor(path);
    }
  }

  private protected sealed override void AfterEndProcessing()
  {
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

    CreateProcess(
      LocalAppData(
        @"Programs\Microsoft VS Code\bin\code.cmd"
      ),
      argumentList,
      true
    );
  }
}
