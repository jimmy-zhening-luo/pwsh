namespace PowerModule.Tab.Factory.Intrinsics;

[System.AttributeUsage(
  System.AttributeTargets.Property
  | System.AttributeTargets.Field
)]
abstract class TCompleterFactory(
  CompletionCase Case = default,
  CompletionResultType CompletionType = CompletionResultType.ParameterValue
) : ArgumentCompleterAttribute, IArgumentCompleterFactory
{
  public CompletionCase Case
  { get; init; } = Case;

  public CompletionResultType CompletionType
  { get; init; } = CompletionType;

  abstract public IArgumentCompleter Create();
}
