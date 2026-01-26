namespace Module.Command.Shell.Start.Workspace;

public abstract class WrappedStartWorkspace : WrappedCommandShouldProcess
{
  private protected WrappedStartWorkspace() : base(
    "Start-Process"
  )
  { }

  [Parameter(
    Position = 0
  )]
  [AllowEmptyString]
  public string Path
  {
    get => path;
    set => path = value;
  }
  private string path = "";

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
  { }
}
