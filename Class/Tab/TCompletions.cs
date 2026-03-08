namespace Module.Tab;

internal enum CompletionCase
{
  [System.ComponentModel.Description(
    "Preserve the original case of the matched completion"
  )]
  Preserve,

  [System.ComponentModel.Description(
    "Convert the matched completion to lowercase"
  )]
  Lower,

  [System.ComponentModel.Description(
    "Convert the matched completion to uppercase"
  )]
  Upper,
}

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
abstract internal class TCompletionsAttribute(
  CompletionCase Case = default,
  CompletionResultType CompletionType = CompletionResultType.ParameterValue
) : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Case { get; init; } = Case;

  public CompletionResultType CompletionType { get; init; } = CompletionType;

  abstract public IArgumentCompleter Create();
}
