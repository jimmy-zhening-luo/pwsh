namespace Module.Commands.Pwsh.Model.Type;

[Cmdlet(
  VerbsCommon.Get,
  "Type"
)]
[Alias("ty")]
[OutputType(typeof(System.Reflection.TypeInfo))]
public sealed class GetType : CoreCommand
{
  [Parameter(
    Mandatory = true,
    Position = 0,
    ValueFromPipeline = true,
    HelpMessage = "The object whose type to check"
  )]
  public object InputObject
  {
    get => inputObject ?? "";
    set => inputObject = value;
  }
  private object? inputObject;

  private protected sealed override void ProcessRecordAction()
  {
    if (inputObject is null)
    {
      Throw(
        new System.ArgumentException(
          "Cannot get type of null InputObject.",
          nameof(InputObject)
        ),
        ErrorCategory.InvalidArgument,
        inputObject
      );
    }

    if (inputObject is PSObject inputPsObject)
    {
      WriteObject(
        inputPsObject
          .BaseObject
          .GetType()
      );
    }
    else
    {
      WriteObject(
        inputObject.GetType()
      );
    }
  }
}
