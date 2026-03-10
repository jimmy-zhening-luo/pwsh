namespace PowerModule.Tab.Completers.Intrinsics;

sealed internal record CompletionResultRecord(
  string Result,
  string? DisplayName = default,
  string? Description = default,
  CompletionResultType? CompletionType = default
);
